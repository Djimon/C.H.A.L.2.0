using CHAL.Core;  // IWallet
using CHAL.Data;
using CHAL.Systems.Inventory; // IInventoryDomain, ItemStack
using CHAL.Systems.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CHAL.Data.GameBalanceConfig;

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

            // optionale Einzel-Flags (hilfreich fÃ¼rs UI, ohne Listen):
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
        /// <summary>
        /// Gets a preview of the recipe output based on the provided parameters.
        /// </summary>
        /// <param name="recipe">The recipe definition to preview.</param>
        /// <param name="outputInventoryId">The ID of the output inventory.</param>
        /// <param name="inv">The inventory domain to check against.</param>
        /// <param name="wallet">The wallet used for transactions.</param>
        /// <returns>A RecipePreview object representing the recipe output.</returns>
        public static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)
        {
            var outType = ItemTypeUtils.FromId(recipe.outputItemId);
            var isGear = outType == ItemType.Gear;
            var isModule = outType == ItemType.Module;


            var outStack = isGear ? new ItemStackRef(recipe.outputItemId, 1, "__preview__")
                                  : new ItemStackRef(recipe.outputItemId, Mathf.Max(1, recipe.outputCount));

            bool OutputOk()
            {
                var ok = inv.CanAccept(outputInventoryId, outStack);
                if (!ok)
                    DebugOutputReject(inv, outputInventoryId, outStack); // <â€” NEU: detailliertes Logging
                return ok;
            }

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

/// <summary>
/// Determines if a recipe can be crafted with the given inventory and wallet.
/// </summary>
/// <param name="recipe">The recipe definition to check.</param>
/// <param name="inv">The inventory domain to use.</param>
/// <param name="outputInventoryId">The ID of the output inventory.</param>
/// <param name="wallet">The wallet to check resources against.</param>
/// <returns>True if the recipe can be crafted; otherwise, false.</returns>
        public static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)
            => GetPreview(recipe,  outputInventoryId, inv, wallet).canCraft;

        private static void DebugOutputReject(InventoryDomain inv, string instanceId, ItemStackRef outStack)
        {
            // Existiert die Instanz?
            bool hasInst = inv.HasInstance(instanceId);
            int slotCount = hasInst ? inv.SlotCount(instanceId) : 0;

            int empty = 0, sameItemStacks = 0, sameItemTotal = 0, filled = 0;
            const int SAMPLE_MAX = 6; // kurze Stichprobe fÃ¼r Logs
            var sample = new System.Text.StringBuilder();

            if (hasInst && slotCount > 0)
            {
                for (int s = 0; s < slotCount; s++)
                {
                    var peek = inv.Peek(instanceId, s); // bereits in Helpers verwendet :contentReference[oaicite:3]{index=3}
                    if (!peek.HasValue)
                    {
                        empty++;
                        if (sample.Length < 1 && s < SAMPLE_MAX) sample.Append($"[{s}: empty] ");
                        continue;
                    }

                    filled++;
                    var st = peek.Value;
                    if (s < SAMPLE_MAX) sample.Append($"[{s}: {st.itemID} x{st.count}] ");

                    if (st.itemID == outStack.itemID)
                    {
                        sameItemStacks++;
                        sameItemTotal += st.count;
                    }
                }
            }

            // Kompakte, action-able Logzeile
            DebugManager.Log(
                $"[Craft Preview] Output REJECT â†’ inst='{instanceId}', item={outStack.itemID} x{outStack.count} | " +
                $"exists={hasInst}, slots={slotCount}, empty={empty}, filled={filled}, sameItemStacks={sameItemStacks}, sameItemTotal={sameItemTotal} | sample: {sample}",
                DebugManager.EDebugLevel.Test, "Crafting");
        }

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
                case ItemType.Core: instanceId = "player_core"; break;
                case ItemType.Module: instanceId = "player_module"; break;
                default: instanceId = null; break; // Gear/Unknown â†’ kein Material-Inventar
            }

            return !string.IsNullOrEmpty(instanceId) && inv.HasInstance(instanceId);
        }

        // ---- COMMIT (atomar) ----
/// <summary>
/// Attempts to craft an item from a recipe and add it to the specified inventory.
/// Returns true if successful, otherwise false with a failure reason.
/// </summary>
/// <param name="recipe">The recipe to craft from.</param>
/// <param name="inv">The inventory domain to use.</param>
/// <param name="wallet">The wallet for transaction purposes.</param>
/// <param name="outputInventoryId">The ID of the inventory to receive the output.</param>
/// <param name="failReason">An output parameter that describes the failure reason, if any.</param>
/// <returns>True if crafting was successful; otherwise, false.</returns>
        public static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)
        {
            failReason = null;

            var outType = ItemTypeUtils.FromId(recipe.outputItemId);
            var isGear = outType == ItemType.Gear;

            if (isGear && recipe.outputCount != 1)
            {
                failReason = "Gear output must be unstackable (outputCount must be 1).";
                return false;
            }

            var outStack = isGear ? new ItemStackRef(recipe.outputItemId, 1, "__guard__")
                                  : new ItemStackRef(recipe.outputItemId, Mathf.Max(1, recipe.outputCount));

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
            var removed = new List<(string instId, int slot, ItemStackRef oldStack, int amount)>();

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

            //// 3) Output
            //if (!inv.TryAdd(outputInventoryId, outStack, out var addTx) || !addTx.success)
            //{
            //    if (gold > 0) wallet.Refund("gold", gold);
            //    foreach (var rem in removed)
            //        inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

            //    failReason = $"Output inventory full: {outputInventoryId}";
            //    return false;
            //}

            // 3) Output
            if (isGear)
            {
                // Create concrete gear instance
                var baseTier = recipe.tier <= 1 ? GearBaseTier.T1 : (recipe.tier == 2 ? GearBaseTier.T2 : GearBaseTier.T3);

                var gear = GearInstance.CreateNew(recipe.outputItemId, baseTier);

                // Resolve ArmorClass (V1 fallback: parse from itemId; replace with ItemDef lookup when available)
                var armorClass = ResolveArmorClass(recipe.outputItemId);

                var gm = GameManager.Instance;
                if (gm == null || gm.gearRoller == null)
                {
                    if (gold > 0) wallet.Refund("gold", gold);
                    foreach (var rem in removed)
                        inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

                    failReason = "Missing GameManager or GearRoller.";
                    return false;
                }

                // Roll implicits
                var rng = new System.Random(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
                var rolls = gm.gearRoller.RollImplicits(recipe.slotType, armorClass, baseTier, rng);

                // Apply to instance (capacity is enforced by GearInstance)
                var caps = gm.BalanceConfig.gear.slotCapsByTier.GetCaps(baseTier);
                for (int i = 0; i < rolls.Count; i++)
                    gear.TryAddImplicit(rolls[i], caps.maxImplicits);

                // Register runtime instance before placing the ref into inventory
                gm.RegisterGearInstance(gear);


                // Add instanced ref to inventory
                outStack = new ItemStackRef(recipe.outputItemId, 1, gear.instanceId);

                if (!inv.TryAdd(outputInventoryId, outStack, out var addTx) || !addTx.success)
                {
                    // Cleanup: don't keep dangling instances if inventory add fails
                    gm.RemoveGearInstance(gear.instanceId);

                    if (gold > 0) wallet.Refund("gold", gold);
                    foreach (var rem in removed)
                        inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

                    failReason = $"Output inventory full: {outputInventoryId}";
                    return false;
                }
            }
            else
            {
                outStack = new ItemStackRef(recipe.outputItemId, Mathf.Max(1, recipe.outputCount));

                if (!inv.TryAdd(outputInventoryId, outStack, out var addTx) || !addTx.success)
                {
                    if (gold > 0) wallet.Refund("gold", gold);
                    foreach (var rem in removed)
                        inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

                    failReason = $"Output inventory full: {outputInventoryId}";
                    return false;
                }
            }

            GameManager.Instance.Stats.OnCraftExecuted(recipe.Id);

            return true;
        }

        private static ArmorClass ResolveArmorClass(string gearItemId)
        {
            // Preferred: ItemDef -> GearData -> armorClass
            if (!string.IsNullOrEmpty(gearItemId))
            {
                var reg = ItemRegistry.Instance;
                if (reg != null && reg.TryGet(gearItemId, out var def) && def != null && def.gearData != null)
                    return def.gearData.armorClass;
            }

            // Fallback heuristic
            return InferArmorClassFromGearId(gearItemId);
        }

        private static ArmorClass InferArmorClassFromGearId(string gearItemId)
        {
            if (string.IsNullOrEmpty(gearItemId))
                return ArmorClass.Medium;

            var id = gearItemId.ToLowerInvariant();

            // Conservative heuristics; replace with ItemDef lookup as soon as you have it.
            if (id.Contains("plate") || id.Contains("heavy"))
                return ArmorClass.Heavy;

            if (id.Contains("cloth") || id.Contains("light"))
                return ArmorClass.Light;

            // leather/medium/default
            return ArmorClass.Medium;
        }

    }

    public enum CraftBlocker
    {
        None = 0,             // alles ok
        LockedByResearch,      // UI/Controller setzt das, Service bleibt research-agnostisch
        OutputInventoryFull,  // kein Platz / Filter blockt
        MissingMaterials,     // mind. ein benÃ¶tigtes Material zu wenig
        NotEnoughCurrency,    // Gold (oder andere Currency) reicht nicht
        InvalidRefinement,    // Slider/Material ungÃ¼ltig (nur wenn Feature aktiv)
        UnknownError          // Fallback
    }
}
