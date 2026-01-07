using CHAL.Core;  // IWallet
using CHAL.Data;
using CHAL.Systems.Inventory; // IInventoryDomain, ItemStack
using CHAL.Systems.Items;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public readonly struct SkillModuleCraftPreview
        {
            public readonly bool canCraft;
            public readonly CraftBlocker blocker;
            public readonly IReadOnlyList<MaterialLine> materials;
            public readonly int goldCost;

            public SkillModuleCraftPreview(
                bool canCraft,
                CraftBlocker blocker,
                List<MaterialLine> materials,
                int goldCost)
            {
                this.canCraft = canCraft;
                this.blocker = blocker;
                this.materials = materials ?? new List<MaterialLine>();
                this.goldCost = goldCost;
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


            ItemStackRef outStack;

            // Gear: preview guard instance
            if (isGear)
            {
                outStack = new ItemStackRef(recipe.outputItemId, 1, "__preview__");
            }
            else
            {
                outStack = new ItemStackRef(recipe.outputItemId, Mathf.Max(1, recipe.outputCount));
            }

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
            var isModule = outType == ItemType.Module;

            if (isGear && recipe.outputCount != 1)
            {
                failReason = "Gear output must be unstackable (outputCount must be 1).";
                return false;
            }

            if (isModule && recipe.outputCount != 1)
            {
                failReason = "SkillModule output must be 1 (stacking happens via instanceId).";
                return false;
            }


            ItemStackRef outStack;

            // pre-guard stack: gear uses __guard__, module uses real deterministic instanceId
            if (isGear)
            {
                outStack = new ItemStackRef(recipe.outputItemId, 1, "__guard__");
            }
            else
            {
                outStack = new ItemStackRef(recipe.outputItemId, Mathf.Max(1, recipe.outputCount));
            }

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
            else if (isModule)
            {
                DebugManager.Error("Wrong Method used. for SkillModules use: [TryCraftSkillModuleToInventory]", "Crafting");
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

        #region Skill-Crafting

/// <summary>
/// Previews the crafting of a skill module with the specified items and parameters.
/// </summary>
/// <param name="moduleItem">The item definition of the module to craft.</param>
/// <param name="frameTier">The tier level of the frame.</param>
/// <param name="coreItem">The item definition of the core item.</param>
/// <param name="inv">The inventory domain for crafting.</param>
/// <param name="wallet">The wallet used for transactions.</param>
/// <param name="outputInventoryId">The ID for the output inventory (default is "player_module").</param>
/// <returns>A preview of the skill module crafting process.</returns>
        public static SkillModuleCraftPreview PreviewSkillModuleCraft(
            ItemDef moduleItem,
            int frameTier,
            ItemDef coreItem,
            InventoryDomain inv,
            IWallet wallet,
            string outputInventoryId = "player_module")
        {
            var materials = new List<MaterialLine>();
            var goldCost = 0;

            // Basic sanity
            if (moduleItem == null || moduleItem.moduleData == null || moduleItem.moduleData.skillDef == null)
                return new SkillModuleCraftPreview(false, CraftBlocker.InvalidRefinement, materials, goldCost);

            if (coreItem == null || coreItem.coreData == null)
                return new SkillModuleCraftPreview(false, CraftBlocker.InvalidRefinement, materials, goldCost);

            var skillDef = moduleItem.moduleData.skillDef;
            var selectedCore = coreItem.coreData.coreType;

            // Core-Whitelist laut Spec
            if (!IsCoreAllowedForModule(skillDef, selectedCore))
                return new SkillModuleCraftPreview(false, CraftBlocker.InvalidRefinement, materials, goldCost);

            // Tier-Kosten aus Balance
            frameTier = Mathf.Max(1, frameTier);
            if (!TryGetSkillModuleTierCost(frameTier, out var tierCost))
                return new SkillModuleCraftPreview(false, CraftBlocker.InvalidRefinement, materials, goldCost);

            goldCost = Mathf.Max(0, tierCost.goldCost);

            // Materials aufbauen: 1x Core + TierIngredients
            // Core
            materials.Add(new MaterialLine
            {
                itemId = coreItem.itemId,
                required = 1,
                playerAmount = CountItemInInventory(inv, coreItem.itemId)
            });

            // Weitere Zutaten aus Config
            if (tierCost.Ingredients != null)
            {
                for (int i = 0; i < tierCost.Ingredients.Count; i++)
                {
                    var ing = tierCost.Ingredients[i];
                    if (ing.Ingredient == null || string.IsNullOrEmpty(ing.Ingredient.itemId))
                        continue;

                    var itemId = ing.Ingredient.itemId;
                    var required = Mathf.Max(1, ing.Amount);

                    materials.Add(new MaterialLine
                    {
                        itemId = itemId,
                        required = required,
                        playerAmount = CountItemInInventory(inv, itemId)
                    });
                }
            }

            // Output-Check
            var outStack = new ItemStackRef(moduleItem.itemId, 1, "__preview__");
            var outputOk = inv.CanAccept(outputInventoryId, outStack);

            // MaterialsOk
            var materialsOk = true;
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].playerAmount < materials[i].required)
                {
                    materialsOk = false;
                    break;
                }
            }

            // CurrencyOk
            var currencyOk = goldCost <= 0 || wallet.CanSpend("gold", goldCost);

            CraftBlocker blocker;
            if (!outputOk)
                blocker = CraftBlocker.OutputInventoryFull;
            else if (!materialsOk)
                blocker = CraftBlocker.MissingMaterials;
            else if (!currencyOk)
                blocker = CraftBlocker.NotEnoughCurrency;
            else
                blocker = CraftBlocker.None;

            var canCraft = blocker == CraftBlocker.None;
            return new SkillModuleCraftPreview(canCraft, blocker, materials, goldCost);
        }

/// <summary>
/// Attempts to craft a skill module and add it to the inventory.
/// Returns true if successful, otherwise false with a failure reason.
/// </summary>
/// <param name="moduleItem">The item definition of the module to craft.</param>
/// <param name="frameTier">The tier level of the frame.</param>
/// <param name="coreItem">The core item definition required for crafting.</param>
/// <param name="inv">The inventory domain where the item will be added.</param>
/// <param name="wallet">The wallet interface for transaction handling.</param>
/// <param name="outputInventoryId">The ID of the inventory to output the result.</param>
/// <param name="failReason">An output parameter that describes the failure reason if crafting fails.</param>
/// <returns>True if the crafting was successful; otherwise, false.</returns>
        public static bool TryCraftSkillModuleToInventory(ItemDef moduleItem, int frameTier, ItemDef coreItem, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)
        {
            failReason = null;

            if (moduleItem == null || moduleItem.moduleData == null || moduleItem.moduleData.skillDef == null)
            {
                failReason = "Invalid module item.";
                return false;
            }

            if (coreItem == null || coreItem.coreData == null)
            {
                failReason = "Invalid core item.";
                return false;
            }

            var gm = GameManager.Instance;
            if (gm == null)
            {
                failReason = "Missing GameManager.";
                return false;
            }

            var skillDef = moduleItem.moduleData.skillDef;
            var selectedCore = coreItem.coreData.coreType;

            // Core-Whitelist
            if (!IsCoreAllowedForModule(skillDef, selectedCore))
            {
                failReason = CraftBlocker.InvalidRefinement.ToString();
                return false;
            }

            // Tier-Kosten
            frameTier = Mathf.Max(1, frameTier);
            if (!TryGetSkillModuleTierCost(frameTier, out var tierCost))
            {
                failReason = "Missing SkillModule tier cost.";
                return false;
            }

            // Guard-Preview (Output + Mats + Gold)
            var preview = PreviewSkillModuleCraft(moduleItem, frameTier, coreItem, inv, wallet, outputInventoryId);
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

            // 1) Materialien konsumieren: 1x Core + TierIngredients
            if (!TryConsumeOne(coreItem.itemId, 1))
            {
                failReason = $"Missing core: {coreItem.itemId}";
                return false;
            }

            if (tierCost.Ingredients != null)
            {
                for (int i = 0; i < tierCost.Ingredients.Count; i++)
                {
                    var ing = tierCost.Ingredients[i];
                    if (ing.Ingredient == null || string.IsNullOrEmpty(ing.Ingredient.itemId))
                        continue;

                    var itemId = ing.Ingredient.itemId;
                    var want = Mathf.Max(1, ing.Amount);

                    if (!TryConsumeOne(itemId, want))
                    {
                        // Rollback Mats
                        foreach (var rem in removed)
                            inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

                        failReason = $"Missing materials: {itemId}";
                        return false;
                    }
                }
            }

            // 2) Gold
            var gold = Mathf.Max(0, tierCost.goldCost);
            if (gold > 0 && !wallet.SpendCurrency("gold", gold))
            {
                foreach (var rem in removed)
                    inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

                failReason = "Gold spend failed.";
                return false;
            }

            // 3) Output: SkillModuleInstance bauen + registrieren + ins Inventory legen
            // weiter oben: var skillDef = moduleItem.moduleData.skillDef;
            // weiter oben: var selectedCore = coreItem.coreData.coreType;

            var skillId = skillDef.SkillId;
            var moduleItemId = moduleItem.itemId;

            var smInstance = SkillModuleInstance.Create(moduleItemId, skillId, frameTier, selectedCore);
            gm.RegisterSkillModuleInstance(smInstance);

            var outStack = new ItemStackRef(moduleItemId, 1, smInstance.instanceId);

            if (!inv.TryAdd(outputInventoryId, outStack, out var addTx) || !addTx.success)
            {
                // Cleanup: Instanz wieder entfernen + Rollback
                gm.RemoveSkillModuleInstance(smInstance.instanceId);

                if (gold > 0) wallet.Refund("gold", gold);
                foreach (var rem in removed)
                    inv.TryAdd(rem.instId, rem.oldStack.WithCount(rem.amount), out _);

                failReason = $"Output inventory full: {outputInventoryId}";
                return false;
            }

            // Stats (synthetischer "RecipeId"-Key)
            gm.Stats.OnCraftExecuted($"skillModule:{moduleItemId}:tier{frameTier}:core{selectedCore}");

            return true;
        }

        static bool IsCoreAllowedForModule(SkillModuleDef skillDef, CoreType selectedCore)
        {
            if (skillDef == null)
                return false;

            // 1) Default-Core ist immer erlaubt
            if (selectedCore == skillDef.defaultCore)
                return true;

            // 2) Sonst nur, wenn explizit in changeCoreTypesAllowed enthalten
            var list = skillDef.changeCoreTypesAllowed;
            if (list == null || list.Count == 0)
                return false;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == selectedCore)
                    return true;
            }

            return false;
        }

        static bool TryGetSkillModuleTierCost(int frameTier, out SMTierCost cost)
        {
            cost = default;

            var gm = GameManager.Instance;
            if (gm == null || gm.BalanceConfig == null)
            {
                DebugManager.Error("[Crafting] GameManager / BalanceConfig missing for SkillModule craft.", "Crafting");
                return false;
            }

            var cfg = gm.BalanceConfig.skillSettings.skillModuleCosts;
            if (cfg.TierBasedCosts == null || cfg.TierBasedCosts.Count == 0)
            {
                DebugManager.Error("[Crafting] SkillModuleCosts.TierBasedCosts is null/empty.", "Crafting");
                return false;
            }

            for (int i = 0; i < cfg.TierBasedCosts.Count; i++)
            {
                if (cfg.TierBasedCosts[i].tier == frameTier)
                {
                    cost = cfg.TierBasedCosts[i];
                    return true;
                }
            }

            DebugManager.Error($"[Crafting] No SkillModule tier cost configured for tier {frameTier}.", "Crafting");
            return false;
        }

        static int CountItemInInventory(InventoryDomain inv, string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                return 0;

            if (!TryGetMaterialsInventoryIdByConvention(itemId, inv, out var instId))
                return 0;

            var inst = inv.GetInstance(instId);
            if (inst == null || inst.slots == null)
                return 0;

            var have = 0;
            for (int i = 0; i < inst.slots.Length; i++)
            {
                var st = inst.slots[i].stack;
                if (st.HasValue && st.Value.itemID == itemId)
                    have += st.Value.count;
            }

            return have;
        }

        #endregion

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
