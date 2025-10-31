using System.Collections.Generic;
using CHAL.Systems.Inventory; // IInventoryDomain, ItemStack
using CHAL.Systems.Crafting;
using CHAL.Systems.Economy;
using UnityEngine;  // IWallet

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

        public readonly struct RecipePreview
        {
            public readonly bool canCraft;         // true nur wenn alle Guards ok
            public readonly CraftBlocker blocker;  // erster harte Blockiergrund in Guard-Reihenfolge

            // optionale Einzel-Flags (hilfreich fürs UI, ohne Listen):
            public readonly bool outputOk;
            public readonly bool materialsOk;
            public readonly bool currencyOk;

            public RecipePreview(bool canCraft, CraftBlocker blocker,
                                 bool outputOk, bool materialsOk, bool currencyOk)
            {
                this.canCraft = canCraft;
                this.blocker = blocker;
                this.outputOk = outputOk;
                this.materialsOk = materialsOk;
                this.currencyOk = currencyOk;
            }
        }

        // ---- PREVIEW ----
        public static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, string materialsInventoryId, IWallet wallet)
        {
            var outStack = new ItemStack(recipe.outputItemId, Mathf.Max(1, recipe.outputCount));

            // --- lokale Helfer (nur Lesen) ---
            bool OutputOk() => inv.CanAccept(outputInventoryId, outStack);

            bool MaterialsOk()
            {
                var inst = inv.GetInstance(materialsInventoryId);
                if (inst == null || inst.slots == null) return false;

                // zählt total "have" je benötigter itemId
                int CountInInv(string itemId)
                {
                    var total = 0;
                    foreach (var slot in inst.slots)
                        if (slot.stack.HasValue && slot.stack.Value.itemID == itemId)
                            total += slot.stack.Value.count;
                    return total;
                }

                foreach (var need in recipe.inputs)
                {
                    var have = CountInInv(need.itemId);
                    if (have < Mathf.Max(1, need.qty)) return false;
                }
                return true;
            }

            int GoldNeed()
            {
                if (recipe.currencyCosts == null) return 0;
                var sum = 0;
                foreach (var c in recipe.currencyCosts)
                    if (!string.IsNullOrEmpty(c.currencyId) && c.currencyId == "gold")
                        sum += Mathf.Max(0, c.amount);
                return sum;
            }

            bool CurrencyOk()
            {
                var gold = GoldNeed();
                return gold <= 0 || wallet.CanSpend("gold", gold);
            }

            // --- Guards & Blocker ---
            var outputOk = OutputOk();
            var materialsOk = outputOk && MaterialsOk();   // erst prüfen, wenn Output ok
            var currencyOk = materialsOk && CurrencyOk();   // erst prüfen, wenn Mats ok

            CraftBlocker blocker =
                !outputOk ? CraftBlocker.OutputInventoryFull :
                !materialsOk ? CraftBlocker.MissingMaterials :
                !currencyOk ? CraftBlocker.NotEnoughCurrency :
                               CraftBlocker.None;

            var canCraft = outputOk && materialsOk && currencyOk;
            return new RecipePreview(canCraft, blocker, outputOk, materialsOk, currencyOk);
        }

        public static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, string materialsInventoryId, IWallet wallet)
            => GetPreview(recipe,  outputInventoryId, inv, materialsInventoryId, wallet).canCraft;

        // ---- COMMIT (atomar) ----
        public static bool TryCraftToInventory(
    RecipeDef recipe,
    InventoryDomain inv,
    string materialsInventoryId,
    IWallet wallet,
    string outputInventoryId,
    out string failReason)
        {
            failReason = null;

            // [G0] Output-Kapazität vor allen Abzügen
            var outStack = new ItemStack(recipe.outputItemId, Mathf.Max(1, recipe.outputCount));
            if (!inv.CanAccept(outputInventoryId, outStack))
            {
                failReason = $"Output inventory cannot accept: {outputInventoryId}";
                return false;
            }

            // [G1] Anforderungen lesen (ohne Seiteneffekte): Materials + Currency
            // Wenn du bereits ein CanCraft(...) hast, kannst du es hier belassen.
            // Andernfalls identisch wie im Preview prüfen (Material- & Gold-Check).
            if (!CanCraft(recipe, inv, outputInventoryId, materialsInventoryId, wallet))
            {
                failReason = "Requirements not met.";
                return false;
            }

            // ===== Commit-Phase (atomar, mit Rollback bei jedem Fail) =====
            // 1) Materials entfernen
            var removed = new List<(int slot, ItemStack oldStack, int amount)>();
            if (!TryConsumeMaterials(recipe, inv, materialsInventoryId, removed, out failReason))
            {
                // Sollte nichts entfernt haben, aber defensiv: Rollback
                RollbackMaterials(inv, materialsInventoryId, removed);
                return false;
            }

            // 2) Currency abbuchen (i. d. R. nur gold)
            var spent = new List<(string id, int amt)>();
            if (!TrySpendCurrencies(recipe, wallet, spent, out failReason))
            {
                RefundCurrencies(wallet, spent);
                RollbackMaterials(inv, materialsInventoryId, removed);
                return false;
            }

            // 3) Item erzeugen (hier ggf. später Refinement anwenden) & Add
            if (!inv.TryAdd(outputInventoryId, outStack, out var addTx) || !addTx.success)
            {
                // Vollständiger Rollback
                RefundCurrencies(wallet, spent);
                RollbackMaterials(inv, materialsInventoryId, removed);
                failReason = $"Output inventory full: {outputInventoryId}";
                return false;
            }

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

    public enum CraftBlocker
    {
        None = 0,             // alles ok
        LockedByResearch,      // UI/Controller setzt das, Service bleibt research-agnostisch
        OutputInventoryFull,  // kein Platz / Filter blockt
        MissingMaterials,     // mind. ein benötigtes Material zu wenig
        NotEnoughCurrency,    // Gold (oder andere Currency) reicht nicht
        InvalidRefinement,    // Slider/Material ungültig (nur wenn Feature aktiv)
        UnknownError          // Fallback
    }
}
