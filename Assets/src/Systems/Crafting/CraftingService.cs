using CHAL.Core;  // IWallet
using CHAL.Data;
using CHAL.Systems.Inventory; // IInventoryDomain, ItemStack
using CHAL.Systems.Items;
using System.Collections.Generic;
using UnityEngine;

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
        public static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)
        {
            var outStack = new ItemStack(recipe.outputItemId, Mathf.Max(1, recipe.outputCount));

            bool OutputOk() => inv.CanAccept(outputInventoryId, outStack);

            bool MaterialsOk()
            {
                if (recipe.inputs == null || recipe.inputs.Count == 0) return true;

                foreach (var need in recipe.inputs)
                {
                    if (!TryGetMaterialsInventoryIdByConvention(need.itemId, inv, out var instId))
                        return false;

                    var inst = inv.GetInstance(instId);
                    if (inst == null || inst.slots == null) return false;

                    var have = 0;
                    for (int i = 0; i < inst.slots.Length; i++)
                    {
                        var st = inst.slots[i].stack;
                        if (st.HasValue && st.Value.itemID == need.itemId)
                            have += st.Value.count;
                    }

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

            bool CurrencyOk() { var g = GoldNeed(); return g <= 0 || wallet.CanSpend("gold", g); }

            var outputOk = OutputOk();
            var materialsOk = outputOk && MaterialsOk();
            var currencyOk = materialsOk && CurrencyOk();

            var blocker =
                !outputOk ? CraftBlocker.OutputInventoryFull :
                !materialsOk ? CraftBlocker.MissingMaterials :
                !currencyOk ? CraftBlocker.NotEnoughCurrency :
                               CraftBlocker.None;

            return new RecipePreview(outputOk && materialsOk && currencyOk, blocker, outputOk, materialsOk, currencyOk);
        }

        public static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)
            => GetPreview(recipe,  outputInventoryId, inv, wallet).canCraft;

        private static bool TryGetMaterialsInventoryIdByConvention(string itemId, InventoryDomain inv, out string instanceId)
        {
            instanceId = null;
            if (string.IsNullOrEmpty(itemId)) return false;

            var t = ItemTypeUtils.FromId(itemId);
            switch (t)
            {
                case ItemType.Remains: instanceId = "player_remains"; break;
                case ItemType.Part: instanceId = "player_part"; break;
                case ItemType.Rune: instanceId = "player_rune"; break;
                case ItemType.Module: instanceId = "player_module"; break;
                default: instanceId = null; break; // Gear/Unknown → kein Material-Inventar
            }

            return !string.IsNullOrEmpty(instanceId) && inv.HasInstance(instanceId);
        }

        // ---- COMMIT (atomar) ----
        public static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)
        {
            failReason = null;

            var outStack = new ItemStack(recipe.outputItemId, Mathf.Max(1, recipe.outputCount));

            // [G0] Output zuerst
            if (!inv.CanAccept(outputInventoryId, outStack))
            {
                failReason = $"Output inventory cannot accept: {outputInventoryId}";
                return false;
            }

            // [G1] Guards read-only
            var preview = GetPreview(recipe, outputInventoryId, inv, wallet);
            if (!preview.canCraft)
            {
                failReason = preview.blocker.ToString();
                return false;
            }

            // ===== Commit-Phase =====
            var removed = new List<(string instId, int slot, ItemStack oldStack, int amount)>();

            bool TryConsumeOne(string itemId, int qty)
            {
                if (!TryGetMaterialsInventoryIdByConvention(itemId, inv, out var instId))
                    return false;

                var inst = inv.GetInstance(instId);
                if (inst == null || inst.slots == null) return false;

                var left = qty;
                for (int i = 0; i < inst.slots.Length && left > 0; i++)
                {
                    var st = inst.slots[i].stack;
                    if (!st.HasValue || st.Value.itemID != itemId) continue;

                    var take = Mathf.Min(st.Value.count, left);
                    if (take <= 0) continue;

                    if (!inv.TryRemove(instId, i, take, out var tx) || !tx.success)
                        return false;

                    removed.Add((instId, i, st.Value, take));
                    left -= take;
                }
                return left <= 0;
            }

            if (recipe.inputs != null)
            {
                foreach (var need in recipe.inputs)
                {
                    var want = Mathf.Max(1, need.qty);
                    if (!TryConsumeOne(need.itemId, want))
                    {
                        // Rollback Mats
                        foreach (var rem in removed)
                            inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

                        failReason = $"Missing materials: {need.itemId}";
                        return false;
                    }
                }
            }

            // 2) Currency
            var gold = 0;
            if (recipe.currencyCosts != null)
                foreach (var c in recipe.currencyCosts)
                    if (!string.IsNullOrEmpty(c.currencyId) && c.currencyId == "gold")
                        gold += Mathf.Max(0, c.amount);

            if (gold > 0 && !wallet.SpendCurrency("gold", gold))
            {
                foreach (var rem in removed)
                    inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

                failReason = "Gold spend failed.";
                return false;
            }

            // 3) Output
            if (!inv.TryAdd(outputInventoryId, outStack, out var addTx) || !addTx.success)
            {
                if (gold > 0) wallet.Refund("gold", gold);
                foreach (var rem in removed)
                    inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

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
