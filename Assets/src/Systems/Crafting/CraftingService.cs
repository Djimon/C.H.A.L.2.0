using System.Collections.Generic;
using CHAL.Systems.Inventory; // IInventoryDomain, ItemStack
using CHAL.Systems.Crafting;
using CHAL.Systems.Economy;  // IWallet

namespace CHAL.Systems.Crafting
{

    public static class CraftingService
    {
        // ---- Preview-Datentypen ----
        public struct MaterialLine
        {
            public string itemId;
            public int required;
            public int playerAmount;
            public bool enough => playerAmount >= required;
        }

        public struct CurrencyLine
        {
            public string currencyId;
            public int required;
            public int playerAmount;
            public bool enough => playerAmount >= required;
        }

        public struct RecipePreview
        {
            public List<MaterialLine> materials;
            public List<CurrencyLine> currencies;
            public bool canCraft;
        }

        // ---- PREVIEW ----
        public static RecipePreview GetPreview(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)
        {
            var mats = new List<MaterialLine>(recipe.inputs?.Count ?? 0);
            if (recipe.inputs != null)
            {
                for (int i = 0; i < recipe.inputs.Count; i++)
                {
                    var c = recipe.inputs[i];
                    int have = CountOf(inv, materialsInventoryId, c.itemId);
                    mats.Add(new MaterialLine { itemId = c.itemId, required = c.qty, playerAmount = have });
                }
            }

            var curr = new List<CurrencyLine>(recipe.currencyCosts?.Count ?? 0);
            if (recipe.currencyCosts != null)
            {
                for (int i = 0; i < recipe.currencyCosts.Count; i++)
                {
                    var c = recipe.currencyCosts[i];
                    int have = wallet.GetCurrency(c.currencyId);
                    curr.Add(new CurrencyLine { currencyId = c.currencyId, required = c.amount, playerAmount = have });
                }
            }

            bool ok = true;
            for (int i = 0; i < mats.Count; i++) if (!mats[i].enough) { ok = false; break; }
            if (ok) for (int i = 0; i < curr.Count; i++) if (!curr[i].enough) { ok = false; break; }

            return new RecipePreview
            {
                materials = mats,
                currencies = curr,
                canCraft = ok
            };
        }

        public static bool CanCraft(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)
            => GetPreview(recipe, inv, materialsInventoryId, wallet).canCraft;

        // ---- COMMIT (atomar) ----
        public static bool TryCraftToInventory(RecipeDef recipe,
                                               IInventoryDomain inv,
                                               string materialsInventoryId,
                                               IWallet wallet,
                                               string outputInventoryId,                 // <— Ziel-Inventar
                                               out string failReason)
        {
            failReason = null;

            // 0) Vorab-Prüfung (wie bisher)
            if (!CanCraft(recipe, inv, materialsInventoryId, wallet))
            {
                failReason = "Requirements not met.";
                return false;
            }

            // 1) Materials entfernen (wie bisher)
            var removed = new List<(int slot, ItemStack oldStack, int amount)>();
            if (!TryConsumeMaterials(recipe, inv, materialsInventoryId, removed, out failReason))
            {
                RollbackMaterials(inv, materialsInventoryId, removed);
                return false;
            }

            // 2) Currency abbuchen (wie bisher)
            var spent = new List<(string id, int amt)>();
            if (!TrySpendCurrencies(recipe, wallet, spent, out failReason))
            {
                RefundCurrencies(wallet, spent);
                RollbackMaterials(inv, materialsInventoryId, removed);
                return false;
            }

            // 3) Output ins Ziel-Inventar legen (atomar)
            var outputStack = new ItemStack(recipe.outputItemId, recipe.outputCount);
            if (!inv.TryAdd(outputInventoryId, outputStack, out var _))
            {
                // Kein Platz → alles zurück
                RefundCurrencies(wallet, spent);
                RollbackMaterials(inv, materialsInventoryId, removed);
                failReason = $"Output inventory full: {outputInventoryId}";
                return false;
            }

            // Erfolg (optional Telemetry)
            return true;
        }


        // ---- Helpers ----
        private static int CountOf(IInventoryDomain inv, string instanceId, string itemId)
        {
            int sum = 0;
            int slots = inv.SlotCount(instanceId);
            for (int i = 0; i < slots; i++)
            {
                var st = inv.Peek(instanceId, i);
                if (st.HasValue && st.Value.itemID == itemId)
                    sum += st.Value.count;
            }
            return sum;
        }

        private static bool TryConsumeMaterials(RecipeDef recipe,
                                                IInventoryDomain inv, string instanceId,
                                                List<(int slot, ItemStack oldStack, int amount)> removed,
                                                out string reason)
        {
            reason = null;
            if (recipe.inputs == null || recipe.inputs.Count == 0) return true;

            for (int i = 0; i < recipe.inputs.Count; i++)
            {
                var need = recipe.inputs[i];
                int remaining = need.qty;

                int slots = inv.SlotCount(instanceId);
                for (int s = 0; s < slots && remaining > 0; s++)
                {
                    var peek = inv.Peek(instanceId, s);
                    if (!peek.HasValue) continue;
                    var st = peek.Value;
                    if (st.itemID != need.itemId) continue;

                    int take = System.Math.Min(st.count, remaining);
                    if (take <= 0) continue;

                    if (!inv.TryRemove(instanceId, s, take, out var _))
                    {
                        reason = $"Remove failed @slot {s} ({need.itemId})";
                        return false;
                    }

                    removed.Add((s, st, take));
                    remaining -= take;
                }

                if (remaining > 0)
                {
                    reason = $"Insufficient after scan: {need.itemId}";
                    return false;
                }
            }
            return true;
        }

        private static void RollbackMaterials(IInventoryDomain inv, string instanceId,
            List<(int slot, ItemStack oldStack, int amount)> removed)
        {
            // Simplest: add back as new stacks (du hast Platz, weil sie gerade entnommen wurden)
            for (int i = 0; i < removed.Count; i++)
            {
                var (_, old, amount) = removed[i];
                inv.TryAdd(instanceId, new ItemStack(old.itemID, amount), out _);
            }
            removed.Clear();
        }

        private static bool TrySpendCurrencies(RecipeDef recipe, IWallet wallet,
                                               List<(string id, int amt)> spent, out string reason)
        {
            reason = null;
            if (recipe.currencyCosts == null || recipe.currencyCosts.Count == 0) return true;

            for (int i = 0; i < recipe.currencyCosts.Count; i++)
            {
                var c = recipe.currencyCosts[i];
                // Sicherheit: vor Spend nochmals prüfen
                if (!wallet.CanSpend(c.currencyId, c.amount))
                {
                    reason = $"Currency missing: {c.currencyId}";
                    return false;
                }
            }

            // tatsächliches Abbuchen
            for (int i = 0; i < recipe.currencyCosts.Count; i++)
            {
                var c = recipe.currencyCosts[i];
                if (!wallet.SpendCurrency(c.currencyId, c.amount))
                {
                    reason = $"Spend failed: {c.currencyId}";
                    return false;
                }
                spent.Add((c.currencyId, c.amount));
            }
            return true;
        }

        private static void RefundCurrencies(IWallet wallet, List<(string id, int amt)> spent)
        {
            for (int i = 0; i < spent.Count; i++)
            {
                var (id, amt) = spent[i];
                wallet.Refund(id, amt);
            }
            spent.Clear();
        }
    }
}
