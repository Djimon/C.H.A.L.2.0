using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{
    public sealed class CheatMenuController : IngameUI
    {
        [Header("UI")]
        [SerializeField] private UIDocument document;

        [SerializeField] private string partsPrefix = "part";
        [SerializeField] private string remainsPrefix = "remains";
        [SerializeField] private string gearPrefix = "gear";
        [SerializeField] private string modulesPrefix = "module";
        [SerializeField] private string corePrefix = "core";

        private ItemRegistry Registry => ItemRegistry.Instance;

        private const int MaxImplicitsPrototype = 4;
        private const int MaxAffixesPrototype = 4;

        private VisualElement _root;

        // Containers for dynamic lists
        private VisualElement _implicitsContainer;
        private VisualElement _affixesContainer;

        private int _implicitRows;
        private int _affixRows;

        private readonly List<ImplicitDef> _tmpImplicitDefs = new(128);
        private readonly List<AffixDef> _tmpAffixDefs = new(256);

        private void OnEnable()
        {
            if (document == null)
            {
                document = GetComponent<UIDocument>();
            }

            if (document == null)
            {
                DebugManager.Log("CheatMenuController: UIDocument missing.", DebugManager.EDebugLevel.Dev, "UI", LogType.Error);
                return;
            }

            _root = document.rootVisualElement;
            if (_root == null)
            {
                DebugManager.Log("CheatMenuController: rootVisualElement is null.", DebugManager.EDebugLevel.Dev, "UI", LogType.Error);
                return;
            }

            Show(false);

            CacheContainers();
            WireButtons();
            FillGearEnumDropdowns();
            FillDropdownsFromRegistry();
            WireGearTypeRefresh();
            WireModuleItemRefresh();       
            ApplyModuleSelectionConstraints();
        }

        private void CacheContainers()
        {
            _implicitsContainer = _root.Q<VisualElement>("container_implicits");
            _affixesContainer = _root.Q<VisualElement>("container_affixes");
        }

        private void WireButtons()
        {
            //Currency
            // Gold
            BindClick("btn_gold_add", () =>
            {
                var amount = GetIntValue("int_gold_amount", 1000);
                AddGold(amount);
            });

            BindClick("btn_gold_set", () =>
            {
                var amount = GetIntValue("int_gold_amount", 0);
                SetGold(amount);
            });


            // Items / Inventory quick deletes (names must match CheatMenu.uxml)
            BindClick("btn_delete_parts", () => ClearInventoryAndCleanupGearInstances(PlayerInventoryType.Part));
            BindClick("btn_delete_remains", () => ClearInventoryAndCleanupGearInstances(PlayerInventoryType.Remains));
            BindClick("btn_delete_gear", () => ClearInventoryAndCleanupGearInstances(PlayerInventoryType.Gear));
            BindClick("btn_delete_modules", () => ClearInventoryAndCleanupGearInstances(PlayerInventoryType.Module));
            BindClick("btn_delete_cores", () => ClearInventoryAndCleanupGearInstances(PlayerInventoryType.Core));


            // Add basic
            BindClick("btn_add_parts", () =>
            {
                var item = GetDropdownValue("dd_parts_item");
                var amount = GetIntValue("int_parts_amount", 1);
                TryAddToInventoryDomain(item, amount, "Add Parts");
                //LogAction("Inventory", $"Add Parts: {item} x{amount}");
            });

            BindClick("btn_add_remains", () =>
            {
                var item = GetDropdownValue("dd_remains_item");
                var amount = GetIntValue("int_remains_amount", 1);
                TryAddToInventoryDomain(item, amount, "Add Remains");
                //LogAction("Inventory", $"Add Remains: {item} x{amount}");
            });

            BindClick("btn_add_cores", () =>
            {
                var item = GetDropdownValue("dd_core_item");
                var amount = GetIntValue("int_core_amount", 1);
                TryAddToInventoryDomain(item, amount, "Add Cores");
            });


            BindClick("btn_add_gear_rolled", () =>
            {
                var item = GetDropdownValue("dd_gear_item");
                var tier = GetDropdownValue("dd_gear_tier");
                AddGearRolled(item, tier);
            });

            BindClick("btn_add_gear_custom", () =>
            {
                var item = GetDropdownValue("dd_gear_custom_item");
                var tierStr = GetDropdownValue("dd_gear_tier"); // reuse same tier dropdown
                AddGearCustom(item, tierStr);
            });

            // Dynamic list controls
            BindClick("btn_add_implicit_row", AddImplicitRow);
            BindClick("btn_clear_implicits", ClearImplicitRows);

            BindClick("btn_add_affix_row", AddAffixRow);
            BindClick("btn_clear_affixes", ClearAffixRows);

            // Modules custom
            BindClick("btn_add_module_custom", () =>
            {
                var moduleItemId = GetDropdownValue("dd_module_item");
                var tierStr = GetDropdownValue("dd_module_tier");
                var coreStr = GetDropdownValue("dd_module_core");

                AddSkillModuleCustom(moduleItemId, tierStr, coreStr);
            });

            // Heroes
            BindClick("btn_reset_all_heroes", ResetAllHeroesProgress);
            BindClick("btn_unlock_all_heroes", () => LogAction("Heroes", "Unlock all Heroes"));
            BindClick("btn_lock_all_heroes", () => LogAction("Heroes", "Lock all Heroes"));

            BindClick("btn_reset_level_1", () =>
            {
                var heroId = GetDropdownValue("dd_hero_select");
                ResetHeroToLevel1(heroId);
            });

            BindClick("btn_hero_levelup", () =>
            {
                var heroId = GetDropdownValue("dd_hero_select");
                var levels = GetIntValue("int_hero_add_levels", 1);
                levels = Mathf.Clamp(levels, 0, 100);

                LevelUpHeroProgress(heroId, levels);
            });

            BindClick("btn_hero_reset_orbit", () =>
            {
                var heroId = GetDropdownValue("dd_hero_select");
                ResetOrbitPoints(heroId);
            });

            // Research
            BindClick("btn_reset_all_research", ResetAllResearch);
            BindClick("btn_unlock_all_research", UnlockAllResearch);
        }

        private void AddGold(int amount)
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Profile == null)
            {
                DebugManager.Log("CheatMenu: missing GameManager/Profile for AddGold.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            amount = Mathf.Max(0, amount);
            if (amount <= 0) return;

            int before = gm.Profile.GetCurrency("gold");
            gm.Profile.AddCurrency("gold", amount); 
            int after = gm.Profile.GetCurrency("gold");

            DebugManager.Log($"CheatMenu: Gold +{amount} ({before}->{after})",
                DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);
        }

        private void SetGold(int amount)
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Profile == null)
            {
                DebugManager.Log("CheatMenu: missing GameManager/Profile for SetGold.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            amount = Mathf.Max(0, amount);

            int before = gm.Profile.GetCurrency("gold");
            int delta = amount - before;

            if (delta > 0)
            {
                gm.Profile.AddCurrency("gold", delta);
            }
            else if (delta < 0)
            {
                // sauber runtersetzen über SpendCurrency (blockt, wenn nicht genug – aber wir sind im cheat menu)
                gm.Profile.SpendCurrency("gold", -delta);
            }

            int after = gm.Profile.GetCurrency("gold");
            DebugManager.Log($"CheatMenu: Gold set {before}->{after}",
                DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);
        }

        private void AddSkillModuleCustom(string moduleItemId, string tierStr, string coreStr)
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Inventory == null)
            {
                DebugManager.Log("CheatMenu: GameManager/Inventory missing (AddSkillModuleCustom).",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            moduleItemId = moduleItemId?.Trim();
            if (string.IsNullOrEmpty(moduleItemId))
            {
                DebugManager.Log("CheatMenu: moduleItemId empty (AddSkillModuleCustom).",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            // --- Parse tier (1..5) ---
            if (!int.TryParse(tierStr, out var tierInt))
                tierInt = 1;
            tierInt = Mathf.Clamp(tierInt, 1, 5);

            // --- Parse core ---
            if (!Enum.TryParse(coreStr, ignoreCase: true, out CoreType selectedCore))
                selectedCore = CoreType.Kinetic;

            // --- Resolve module item + skill def reference from item ---
            var reg = ItemRegistry.Instance;
            if (reg == null || !reg.TryGet(moduleItemId, out var itemDef) || itemDef == null)
            {
                DebugManager.Log($"CheatMenu: module item not found in ItemRegistry: '{moduleItemId}'.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            var moduleItem = itemDef;
            if (moduleItem.moduleData == null || moduleItem.moduleData.skillDef == null)
            {
                DebugManager.Log($"CheatMenu: Item '{moduleItemId}' is not a ModuleItemDef with moduleData.skillDef.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            var skillDef = moduleItem.moduleData.skillDef;
            var skillId = skillDef.SkillId;

            if (string.IsNullOrWhiteSpace(skillId))
            {
                DebugManager.Log($"CheatMenu: ModuleItem '{moduleItemId}' has empty skillDef.SkillId.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            // --- Guard: tier must be >= skillDef.minRequiredTier, max 5 ---
            var minTier = Mathf.Clamp(skillDef.minRequiredTier, 1, 5);
            if (tierInt < minTier)
            {
                DebugManager.Log($"CheatMenu: tier {tierInt} < skillDef.minRequiredTier {minTier} (skill='{skillId}'). Clamping up.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                tierInt = minTier;
            }

            // --- Guard: core must be in AllowedCores OR be DefaultCore ---
            var allowed = moduleItem.moduleData.skillDef.changeCoreTypesAllowed;
            var defCore = moduleItem.moduleData.skillDef.defaultCore;

            bool coreOk =
                selectedCore == defCore ||
                (allowed != null && allowed.Contains(selectedCore));

            if (!coreOk)
            {
                DebugManager.Log(
                    $"CheatMenu: Core '{selectedCore}' not allowed for '{moduleItemId}'. Default={defCore}, Allowed={FormatCores(allowed)}",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            // --- Resolve output inventory ---
            if (!gm.TryResolveByItemId(moduleItemId, out var invType, out var outputInventoryId))
            {
                DebugManager.Log($"CheatMenu: Unknown inventory prefix for moduleItemId='{moduleItemId}'.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }
            gm.EnsureInstance(outputInventoryId, invType);

            // --- Create instance + register + add to inventory (like crafting) ---
            var frameTier = tierInt; 
            var smInstance = SkillModuleInstance.Create(moduleItemId, skillId, frameTier, selectedCore);

            gm.RegisterSkillModuleInstance(smInstance);

            var outStack = new ItemStackRef(moduleItemId, 1, smInstance.instanceId);

            if (!gm.Inventory.TryAdd(outputInventoryId, outStack, out var tx) || !tx.success)
            {
                gm.RemoveSkillModuleInstance(smInstance.instanceId);
                DebugManager.Log($"CheatMenu: TryAdd failed for module {moduleItemId} -> {tx.reason}",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            DebugManager.Log(
                $"CheatMenu: Added SkillModule '{moduleItemId}' skill='{skillId}' tier={tierInt} core={selectedCore} inst={smInstance.instanceId}",
                DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);
        }

        private static string FormatCores(IReadOnlyList<CoreType> list)
        {
            if (list == null || list.Count == 0) return "[]";
            var sb = new System.Text.StringBuilder();
            sb.Append('[');
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(list[i]);
            }
            sb.Append(']');
            return sb.ToString();
        }

        private void WireModuleItemRefresh()
        {
            var dd = _root.Q<DropdownField>("dd_module_item");
            if (dd == null)
            {
                DebugManager.Log("CheatMenu: dd_module_item not found for refresh hook.",
                    DebugManager.EDebugLevel.Dev, "UI", LogType.Warning);
                return;
            }

            dd.RegisterValueChangedCallback(_ =>
            {
                ApplyModuleSelectionConstraints();
            });
        }

        private void ApplyModuleSelectionConstraints()
        {
            var moduleItemId = GetDropdownValue("dd_module_item")?.Trim();
            if (string.IsNullOrEmpty(moduleItemId) || moduleItemId == "<null>")
                return;

            var reg = ItemRegistry.Instance;
            if (reg == null || !reg.TryGet(moduleItemId, out var itemDef) || itemDef == null)
                return;

            if (itemDef.moduleData == null || itemDef.moduleData.skillDef == null)
                return;

            var skillDef = itemDef.moduleData.skillDef;

            ApplyModuleTierConstraints(skillDef);
            ApplyModuleCoreConstraints(skillDef);
        }

        private void ApplyModuleTierConstraints(SkillModuleDef skillDef)
        {
            var ddTier = _root.Q<DropdownField>("dd_module_tier");
            if (ddTier == null) return;

            // minRequiredTier clamp + choices restrict
            var minTier = Mathf.Clamp(skillDef.minRequiredTier, 1, 5);

            // choices: only legal tiers [min..5]
            var choices = new List<string>(5 - minTier + 1);
            for (int t = minTier; t <= 5; t++)
                choices.Add(t.ToString());

            // preserve old selection if possible, else clamp up
            var oldStr = ddTier.value;
            ddTier.choices = choices;

            int oldTier = 1;
            if (!int.TryParse(oldStr, out oldTier))
                oldTier = minTier;

            oldTier = Mathf.Clamp(oldTier, minTier, 5);
            ddTier.value = oldTier.ToString();
        }

        private void ApplyModuleCoreConstraints(SkillModuleDef skillDef)
        {
            var ddCore = _root.Q<DropdownField>("dd_module_core");
            if (ddCore == null) return;

            var defCore = skillDef.defaultCore;
            var allowed = skillDef.changeCoreTypesAllowed;

            // Build legal core list = default + allowed (unique). Default first.
            var set = new HashSet<CoreType>();
            var legal = new List<CoreType>(16);

            void Add(CoreType c)
            {
                if (set.Add(c))
                    legal.Add(c);
            }

            Add(defCore);
            if (allowed != null)
            {
                for (int i = 0; i < allowed.Count; i++)
                    Add(allowed[i]);
            }

            // Convert to string choices (default first, rest sorted for UX)
            var choices = new List<string>(legal.Count);

            // keep default at top, sort the rest alphabetically
            choices.Add(defCore.ToString());

            if (legal.Count > 1)
            {
                var rest = new List<string>(legal.Count - 1);
                for (int i = 0; i < legal.Count; i++)
                {
                    var c = legal[i];
                    if (c.Equals(defCore)) continue;
                    rest.Add(c.ToString());
                }
                rest.Sort(StringComparer.OrdinalIgnoreCase);
                choices.AddRange(rest);
            }

            var old = ddCore.value;
            ddCore.choices = choices;

            // keep old if still legal, else snap to default
            if (!string.IsNullOrEmpty(old) && choices.Contains(old))
                ddCore.value = old;
            else
                ddCore.value = defCore.ToString();
        }


        private void ClearInventoryAndCleanupGearInstances(PlayerInventoryType t)
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Inventory == null) return;

            var instanceId = gm.InstanceIdFor(t); // gives "player_<type>" 
            if (string.IsNullOrWhiteSpace(instanceId) || !gm.Inventory.HasInstance(instanceId))
                return;

            // If this inventory can contain gear refs, remove referenced instances first
            int slots = gm.Inventory.SlotCount(instanceId);
            for (int i = 0; i < slots; i++)
            {
                var st = gm.Inventory.Peek(instanceId, i);
                if (!st.HasValue) continue;

                var instId = st.Value.instanceId;
                if (!string.IsNullOrWhiteSpace(instId))
                {
                    gm.RemoveGearInstance(instId); // safe even if non-gear; it just won't exist 
                    gm.RemoveSkillModuleInstance(instId);
                }              
            }

            gm.Inventory.ClearAllSlots(instanceId);
            DebugManager.Log($"CheatMenu: Cleared {instanceId}", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);
        }


        private void FillDropdownsFromRegistry()
        {
            var reg = Registry;
            if (reg == null)
            {
                DebugManager.Log("CheatMenu: ItemRegistry.Instance is null. Dropdowns stay empty.", DebugManager.EDebugLevel.Dev, "UI", LogType.Warning);
                return;
            }

            // Fill item dropdowns from registry
            SetDropdownChoices("dd_parts_item", GetIdsByPrefix(reg, partsPrefix));
            SetDropdownChoices("dd_remains_item", GetIdsByPrefix(reg, remainsPrefix));
            SetDropdownChoices("dd_gear_item", GetIdsByPrefix(reg, gearPrefix));
            SetDropdownChoices("dd_gear_custom_item", GetIdsByPrefix(reg, gearPrefix));
            SetDropdownChoices("dd_module_item", GetIdsByPrefix(reg, modulesPrefix));
            SetDropdownChoices("dd_core_item", GetIdsByPrefix(reg, corePrefix));

            // Tiers (still hardcoded prototype values)
            SetDropdownChoices("dd_gear_tier", new List<string> { "1", "2", "3" });
            SetDropdownChoices("dd_module_tier", new List<string> { "1", "2", "3", "4", "5" });

            // CoreType dummy for prototype (replace later with your enum values)
            SetDropdownChoicesFromEnum<CoreType>("dd_module_core", CoreType.Kinetic);

            var gm = GameManager.Instance;
            var profile = gm != null ? gm.Profile : null;
            var roster = profile != null ? profile.GetUnlockedHeroes() : Array.Empty<string>();
            SetDropdownChoices("dd_hero_select", new List<string>(roster));

            DebugManager.Log(
                $"CheatMenu: Dropdowns filled. parts={CountChoices("dd_parts_item")} remains={CountChoices("dd_remains_item")} gear={CountChoices("dd_gear_item")} modules={CountChoices("dd_module_item")}",
                DebugManager.EDebugLevel.Dev,
                "UI",
                LogType.Log
            );
        }

        private bool TryAddToInventoryDomain(string itemId, int count, string contextLabel)
        {
            var gm = GameManager.Instance;
            if (gm == null)
            {
                DebugManager.Log("CheatMenu: GameManager.Instance is null", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return false;
            }

            var profile = gm.Profile;
            var domain = gm.Inventory;

            if (profile == null || domain == null)
            {
                DebugManager.Log($"CheatMenu: missing Profile or Inventory domain (Profile={profile != null}, Domain={domain != null})",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return false;
            }

            itemId = itemId?.Trim();
            if (string.IsNullOrEmpty(itemId))
            {
                DebugManager.Log($"CheatMenu: itemId empty ({contextLabel})", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return false;
            }

            count = Mathf.Max(0, count);
            if (count <= 0)
            {
                DebugManager.Log($"CheatMenu: count <= 0 for '{itemId}' ({contextLabel})", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return false;
            }

            if (!gm.TryResolveByItemId(itemId, out var invType, out var instanceId))
            {
                DebugManager.Log($"CheatMenu: Unknown inventory prefix for itemId='{itemId}' ({contextLabel})",
                    DebugManager.EDebugLevel.Test, "Inventory", LogType.Warning);
                return false;
            }

            gm.EnsureInstance(instanceId, invType);

            var ok = domain.TryAdd(instanceId, new ItemStackRef(itemId, count), out var tx);
            if (!ok)
            {
                DebugManager.Log($"CheatMenu: TryAdd failed for {itemId} x{count} -> {tx.reason} ({contextLabel})",
                    DebugManager.EDebugLevel.Dev, "Inventory", LogType.Warning);
                return false;
            }

            DebugManager.Log($"CheatMenu: Added {itemId} x{count} ({contextLabel})",
                DebugManager.EDebugLevel.Dev, "Inventory", LogType.Log);
            return true;
        }

        private static List<string> GetIdsByPrefix(ItemRegistry reg, string prefix)
        {
            var list = new List<string>();
            if (string.IsNullOrWhiteSpace(prefix))
                return list;

            foreach (var def in reg.GetAllItemsByType(prefix))
            {
                if (def == null)
                    continue;

                // Ich gehe davon aus, dass ItemDef eine Id / itemId / id Property hat.
                // Wenn sie anders heißt: hier anpassen, das ist die einzige Stelle.
                var id = def.itemId; // <-- ggf. umbenennen: def.itemId / def.ItemId
                if (!string.IsNullOrEmpty(id))
                    list.Add(id);
            }

            list.Sort(StringComparer.OrdinalIgnoreCase);
            return list;
        }

        private int CountChoices(string dropdownName)
        {
            var dd = _root.Q<DropdownField>(dropdownName);
            return dd?.choices?.Count ?? 0;
        }

        private void AddGearRolled(string itemId, string tierStr)
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Inventory == null)
            {
                DebugManager.Log("CheatMenu: GameManager/Inventory missing", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            if (string.IsNullOrEmpty(itemId))
            {
                DebugManager.Log("CheatMenu: Gear itemId empty", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            if (!int.TryParse(tierStr, out var tierInt))
                tierInt = 1;

            tierInt = Mathf.Clamp(tierInt, 1, 3);

            var baseTier = tierInt == 1 ? GearBaseTier.T1 : (tierInt == 2 ? GearBaseTier.T2 : GearBaseTier.T3);

            // NEW: required selectors for rolling
            var gearType = GetEnumDropdownValueOrDefault("dd_gear_type", GearType.Chest);
            var armorClass = GetEnumDropdownValueOrDefault("dd_gear_armorClass", ArmorClass.Heavy);

            if (gm.gearRoller == null)
            {
                DebugManager.Log("CheatMenu: gm.gearRoller missing", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            // Create concrete gear instance
            var gear = GearInstance.CreateNew(itemId, baseTier); // creates GUID instanceId :contentReference[oaicite:3]{index=3}

            // Roll implicits (same call signature as your roller) :contentReference[oaicite:4]{index=4}
            var rng = new System.Random(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
            var rolls = gm.gearRoller.RollImplicits(gearType, armorClass, baseTier, rng);

            // Apply caps (like your crafting snippet does)
            var caps = gm.BalanceConfig.gear.slotCapsByTier.GetCaps(baseTier);
            for (int i = 0; i < rolls.Count; i++)
                gear.TryAddImplicit(rolls[i], caps.maxImplicits); // capacity enforced by GearInstance :contentReference[oaicite:5]{index=5}

            // Register runtime instance before placing ref into inventory :contentReference[oaicite:6]{index=6}
            gm.RegisterGearInstance(gear);

            // Resolve which player inventory instance to place this into (prefix routing) :contentReference[oaicite:7]{index=7}
            if (!gm.TryResolveByItemId(itemId, out var invType, out var outputInventoryId))
            {
                gm.RemoveGearInstance(gear.instanceId);
                DebugManager.Log($"CheatMenu: Unknown inventory prefix for '{itemId}'", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            gm.EnsureInstance(outputInventoryId, invType);

            // Gear is unstackable: count=1 + instanceId 
            var outStack = new ItemStackRef(itemId, 1, gear.instanceId);

            if (!gm.Inventory.TryAdd(outputInventoryId, outStack, out var tx) || !tx.success)
            {
                // Cleanup: don't keep dangling instances if inventory add fails :contentReference[oaicite:9]{index=9}
                gm.RemoveGearInstance(gear.instanceId);

                DebugManager.Log($"CheatMenu: TryAdd failed for rolled gear {itemId} -> {tx.reason}",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            DebugManager.Log($"CheatMenu: Added rolled gear {gear}", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);
        }

        // Generic helper for Enum dropdowns (UI Toolkit stores string values)
        private TEnum GetEnumDropdownValueOrDefault<TEnum>(string elementName, TEnum fallback) where TEnum : struct
        {
            var s = GetDropdownValue(elementName);
            if (string.IsNullOrWhiteSpace(s)) return fallback;
            if (Enum.TryParse<TEnum>(s, ignoreCase: true, out var v)) return v;
            return fallback;
        }

        private void AddImplicitRow()
        {
            if (_implicitsContainer == null)
            {
                LogAction("UI", "Implicits container missing.");
                return;
            }

            if (_implicitRows >= MaxImplicitsPrototype)
            {
                DebugManager.Log($"Implicits: reached max ({MaxImplicitsPrototype}).", DebugManager.EDebugLevel.Dev, "UI", LogType.Warning);
                return;
            }

            _implicitRows++;

            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            //row.style.gap = 6;

            var dd = new DropdownField();
            dd.choices = BuildImplicitIdChoicesForCurrentGearType();
            dd.value = dd.choices[0];
            dd.style.minWidth = 240;

            var btnRemove = new Button(() =>
            {
                _implicitsContainer.Remove(row);
                _implicitRows = Mathf.Max(0, _implicitRows - 1);
                LogAction("Inventory", "Remove Implicit Row");
            })
            { text = "Remove" };
            btnRemove.style.minWidth = 120;

            row.Add(dd);
            row.Add(btnRemove);

            _implicitsContainer.Add(row);
            LogAction("Inventory", "Add Implicit Row");
        }

        private void ClearImplicitRows()
        {
            _implicitsContainer?.Clear();
            _implicitRows = 0;
            LogAction("Inventory", "Clear Implicits");
        }

        private void AddAffixRow()
        {
            if (_affixesContainer == null)
            {
                LogAction("UI", "Affixes container missing.");
                return;
            }

            if (_affixRows >= MaxAffixesPrototype)
            {
                DebugManager.Log($"Affixes: reached max ({MaxAffixesPrototype}).", DebugManager.EDebugLevel.Dev, "UI", LogType.Warning);
                return;
            }

            _affixRows++;

            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            //row.style.gap = 6;

            var dd = new DropdownField();
            dd.choices = BuildAffixIdChoicesForCurrentGearType();
            dd.value = dd.choices[0];
            dd.style.minWidth = 240;

            var btnRemove = new Button(() =>
            {
                _affixesContainer.Remove(row);
                _affixRows = Mathf.Max(0, _affixRows - 1);
                LogAction("Inventory", "Remove Affix Row");
            })
            { text = "Remove" };
            btnRemove.style.minWidth = 120;

            row.Add(dd);
            row.Add(btnRemove);

            _affixesContainer.Add(row);
            LogAction("Inventory", "Add Affix Row");
        }

        private void ClearAffixRows()
        {
            _affixesContainer?.Clear();
            _affixRows = 0;
            LogAction("Inventory", "Clear Affixes");
        }

        private void BindClick(string name, Action action)
        {
            var btn = _root.Q<Button>(name);
            if (btn == null)
            {
                DebugManager.Log($"CheatMenuController: Button not found: {name}", DebugManager.EDebugLevel.Dev, "UI", LogType.Warning);
                return;
            }

            btn.clicked += action;
        }

        private void SetDropdownChoices(string name, List<string> choices)
        {
            var dd = _root.Q<DropdownField>(name);
            if (dd == null)
            {
                DebugManager.Log($"CheatMenuController: Dropdown not found: {name}", DebugManager.EDebugLevel.Dev, "UI", LogType.Warning);
                return;
            }

            dd.choices = choices ?? new List<string>();
            if (dd.choices.Count > 0)
            {
                dd.value = dd.choices[0];
            }
        }

        private void AddGearCustom(string itemId, string tierStr)
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Inventory == null)
            {
                DebugManager.Log("CheatMenu: GameManager/Inventory missing", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            if (gm.BalanceConfig == null)
            {
                DebugManager.Log("CheatMenu: BalanceConfig missing", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            if (gm.gearModRegistry == null)
            {
                DebugManager.Log("CheatMenu: gm.gearModRegistry missing", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            itemId = itemId?.Trim();
            if (string.IsNullOrEmpty(itemId))
            {
                DebugManager.Log("CheatMenu: Custom gear itemId empty", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            if (!int.TryParse(tierStr, out var tierInt))
                tierInt = 1;
            tierInt = Mathf.Clamp(tierInt, 1, 3);

            var baseTier = tierInt == 1 ? GearBaseTier.T1 : (tierInt == 2 ? GearBaseTier.T2 : GearBaseTier.T3);

            // Reuse existing selectors (same as rolled)
            var gearType = GetEnumDropdownValueOrDefault("dd_gear_type", GearType.Chest);
            var armorClass = GetEnumDropdownValueOrDefault("dd_gear_armorClass", ArmorClass.Heavy);

            // Read selected implicit/affix IDs from the dynamic UI rows
            var selectedImplicits = ReadSelectedIdsFromRows(_implicitsContainer);
            var selectedAffixes = ReadSelectedIdsFromRows(_affixesContainer);

            var caps = gm.BalanceConfig.gear.slotCapsByTier.GetCaps(baseTier);

            // Create instance
            var gear = GearInstance.CreateNew(itemId, baseTier);

            // Deterministic-ish seed per click (fine for cheats)
            var rng = new System.Random(UnityEngine.Random.Range(int.MinValue, int.MaxValue));

            // Apply implicits
            int implicitSlot = 0;
            var implicitIdSet = new HashSet<string>(StringComparer.Ordinal);

            for (int i = 0; i < selectedImplicits.Count && implicitSlot < caps.maxImplicits; i++)
            {
                var id = (selectedImplicits[i] ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(id) || id == "None") continue;

                // avoid duplicates
                if (!implicitIdSet.Add(id))
                {
                    DebugManager.Log($"CheatMenu: duplicate implicit '{id}' skipped.", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                    continue;
                }

                if (!gm.gearModRegistry.TryGetImplicit(id, out var def) || def == null)
                {
                    DebugManager.Log($"CheatMenu: unknown implicit '{id}' skipped.", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                    continue;
                }

                var value = RollValue(def, baseTier, rng);
                gear.TryAddImplicit(new ImplicitRoll(id, value, implicitSlot, baseTier), caps.maxImplicits);
                implicitSlot++;
            }

            // Apply affixes
            int affixSlot = 0;
            var affixIdSet = new HashSet<string>(StringComparer.Ordinal);

            for (int i = 0; i < selectedAffixes.Count && affixSlot < caps.maxAffixes; i++)
            {
                var id = (selectedAffixes[i] ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(id) || id == "None") continue;

                // avoid duplicates (matches roller default behavior unless allowDuplicateAffixIdPerItem is true)
                if (!affixIdSet.Add(id))
                {
                    DebugManager.Log($"CheatMenu: duplicate affix '{id}' skipped.", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                    continue;
                }

                if (!gm.gearModRegistry.TryGetAffix(id, out var def) || def == null)
                {
                    DebugManager.Log($"CheatMenu: unknown affix '{id}' skipped.", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                    continue;
                }

                var value = RollValue(def, baseTier, rng);
                gear.TryAddAffix(new AffixRoll(id, value, affixSlot, baseTier), caps.maxAffixes);
                affixSlot++;
            }

            // Register runtime instance before placing ref into inventory
            gm.RegisterGearInstance(gear);

            // Resolve inventory by prefix convention (same as crafting / TryAddToInventoryDomain)
            if (!gm.TryResolveByItemId(itemId, out var invType, out var outputInventoryId))
            {
                gm.RemoveGearInstance(gear.instanceId);
                DebugManager.Log($"CheatMenu: Unknown inventory prefix for '{itemId}'", DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            gm.EnsureInstance(outputInventoryId, invType);

            // Add ref (unstackable gear => count=1 + instanceId)
            var outStack = new ItemStackRef(itemId, 1, gear.instanceId);

            if (!gm.Inventory.TryAdd(outputInventoryId, outStack, out var tx) || !tx.success)
            {
                gm.RemoveGearInstance(gear.instanceId);
                DebugManager.Log($"CheatMenu: TryAdd failed for custom gear {itemId} -> {tx.reason}",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            DebugManager.Log($"CheatMenu: Added custom gear {gear} (GearType={gearType}, ArmorClass={armorClass})",
                DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);
        }

        private List<string> ReadSelectedIdsFromRows(VisualElement container)
        {
            var result = new List<string>();
            if (container == null) return result;

            // each row: [DropdownField] [Remove Button]
            for (int i = 0; i < container.childCount; i++)
            {
                var row = container[i];
                if (row == null) continue;

                var dd = row.Q<DropdownField>();
                if (dd == null) continue;

                var v = dd.value;
                if (!string.IsNullOrWhiteSpace(v))
                    result.Add(v);
            }

            return result;
        }

        private List<string> BuildImplicitIdChoicesForCurrentGearType()
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.gearModRegistry == null)
                return new List<string>();

            var gearType = GetEnumDropdownValueOrDefault("dd_gear_type", GearType.Chest);

            var ids = new HashSet<string>(StringComparer.Ordinal);

            // Mirror the way GearRoller pulls candidates: by (gearType, pool, role)
            var pools = (ImplicitPool[])Enum.GetValues(typeof(ImplicitPool));
            var roles = (ImplicitRole[])Enum.GetValues(typeof(ImplicitRole));

            for (int p = 0; p < pools.Length; p++)
            {
                for (int r = 0; r < roles.Length; r++)
                {
                    _tmpImplicitDefs.Clear();
                    gm.gearModRegistry.GetImplicitCandidates(
                        gearType: gearType,
                        poolMaskAsInt: (int)pools[p],
                        roleAsInt: (int)roles[r],
                        buffer: _tmpImplicitDefs
                    );

                    for (int i = 0; i < _tmpImplicitDefs.Count; i++)
                    {
                        var d = _tmpImplicitDefs[i];
                        if (d == null) continue;

                        var id = d.ImplicitId;
                        if (!string.IsNullOrWhiteSpace(id))
                            ids.Add(id.Trim());
                    }
                }
            }

            var list = new List<string>(ids);
            list.Sort(StringComparer.OrdinalIgnoreCase);
            return list;
        }

        private List<string> BuildAffixIdChoicesForCurrentGearType()
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.gearModRegistry == null)
                return new List<string>();

            var gearType = GetEnumDropdownValueOrDefault("dd_gear_type", GearType.Chest);

            var ids = new HashSet<string>(StringComparer.Ordinal);

            var fams = (AffixFamily[])Enum.GetValues(typeof(AffixFamily));
            for (int f = 0; f < fams.Length; f++)
            {
                _tmpAffixDefs.Clear();
                gm.gearModRegistry.GetAffixCandidates(fams[f], gearType, _tmpAffixDefs);

                for (int i = 0; i < _tmpAffixDefs.Count; i++)
                {
                    var d = _tmpAffixDefs[i];
                    if (d == null) continue;

                    var id = d.AffixId;
                    if (!string.IsNullOrWhiteSpace(id))
                        ids.Add(id.Trim());
                }
            }

            var list = new List<string>(ids);
            list.Sort(StringComparer.OrdinalIgnoreCase);
            return list;
        }

        private static float RollValue(ImplicitDef def, GearBaseTier baseTier, System.Random rng)
        {
            var range = baseTier switch
            {
                GearBaseTier.T1 => def.Ranges.Tier1,
                GearBaseTier.T2 => def.Ranges.Tier2,
                GearBaseTier.T3 => def.Ranges.Tier3,
                _ => def.Ranges.Tier1
            };

            var u = (float)rng.NextDouble();
            return range.Min + (range.Max - range.Min) * u;
        }

        private static float RollValue(AffixDef def, GearBaseTier baseTier, System.Random rng)
        {
            var range = baseTier switch
            {
                GearBaseTier.T1 => def.Ranges.Tier1,
                GearBaseTier.T2 => def.Ranges.Tier2,
                GearBaseTier.T3 => def.Ranges.Tier3,
                _ => def.Ranges.Tier1
            };

            var u = (float)rng.NextDouble();
            return range.Min + (range.Max - range.Min) * u;
        }

        private string GetDropdownValue(string name)
        {
            var dd = _root.Q<DropdownField>(name);
            return dd != null ? dd.value : "<null>";
        }

        private int GetIntValue(string name, int fallback)
        {
            var field = _root.Q<IntegerField>(name);
            return field != null ? field.value : fallback;
        }

        private static void LogAction(string tag, string msg)
        {
            DebugManager.Log(msg, DebugManager.EDebugLevel.Debug, "UI", LogType.Log);
        }

        private void FillGearEnumDropdowns()
        {
            SetDropdownChoicesFromEnum<GearType>("dd_gear_type", GearType.Chest);
            SetDropdownChoicesFromEnum<ArmorClass>("dd_gear_armorClass", ArmorClass.Heavy);
        }

        private void SetDropdownChoicesFromEnum<TEnum>(string dropdownName, TEnum defaultValue) where TEnum : struct, Enum
        {
            var dd = _root.Q<DropdownField>(dropdownName);
            if (dd == null)
            {
                DebugManager.Log($"CheatMenu: Dropdown not found: {dropdownName}",
                    DebugManager.EDebugLevel.Dev, "UI", LogType.Warning);
                return;
            }

            // Enum names -> choices
            var names = Enum.GetNames(typeof(TEnum));
            var choices = new List<string>(names.Length);
            for (int i = 0; i < names.Length; i++)
                choices.Add(names[i]);

            dd.choices = choices;

            // Default selection (ensure it exists)
            var def = defaultValue.ToString();
            dd.value = choices.Contains(def) ? def : (choices.Count > 0 ? choices[0] : string.Empty);
        }

        private void WireGearTypeRefresh()
        {
            var dd = _root.Q<DropdownField>("dd_gear_type");
            if (dd == null)
            {
                DebugManager.Log("CheatMenu: dd_gear_type not found for refresh hook.",
                    DebugManager.EDebugLevel.Dev, "UI", LogType.Warning);
                return;
            }

            dd.RegisterValueChangedCallback(_ =>
            {
                RefreshModDropdownsInContainer(_implicitsContainer, BuildImplicitIdChoicesForCurrentGearTypeWithNone);
                RefreshModDropdownsInContainer(_affixesContainer, BuildAffixIdChoicesForCurrentGearTypeWithNone);
            });
        }

        private List<string> BuildImplicitIdChoicesForCurrentGearTypeWithNone()
        {
            var list = BuildImplicitIdChoicesForCurrentGearType();
            list.Insert(0, "None");
            return list;
        }

        private List<string> BuildAffixIdChoicesForCurrentGearTypeWithNone()
        {
            var list = BuildAffixIdChoicesForCurrentGearType();
            list.Insert(0, "None");
            return list;
        }

        private void RefreshModDropdownsInContainer(
            VisualElement container,
            Func<List<string>> buildChoices)
        {
            if (container == null) return;

            var newChoices = buildChoices();
            for (int i = 0; i < container.childCount; i++)
            {
                var row = container[i];
                if (row == null) continue;

                var dd = row.Q<DropdownField>();
                if (dd == null) continue;

                var old = dd.value;
                dd.choices = newChoices;

                // keep selection if still valid
                if (!string.IsNullOrEmpty(old) && newChoices.Contains(old))
                    dd.value = old;
                else
                    dd.value = newChoices.Count > 0 ? newChoices[0] : string.Empty;
            }
        }

        private void LevelUpHeroProgress(string heroId, int addLevels)
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Profile == null)
            {
                DebugManager.Log("CheatMenu: missing GameManager/Profile for LevelUp.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            heroId = heroId?.Trim();
            if (string.IsNullOrEmpty(heroId))
            {
                DebugManager.Log("CheatMenu: heroId empty for LevelUp.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            addLevels = Mathf.Clamp(addLevels, 0, 100);
            if (addLevels <= 0)
                return;

            var hpd = gm.Profile.GetOrCreateHeroProgress(heroId);
            if (hpd == null)
            {
                DebugManager.Log($"CheatMenu: could not get HeroProgressData for '{heroId}'.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            // Mirror HeroInstance behavior:
            // - Level cap 100
            // - Orbit points: MVP +2 per level (see HeroInstance)
            // - XP we keep as-is (cheat menu), but you can optionally zero CurrentXP
            const int orbitPerLevel = 2;

            int beforeLevel = hpd.Level;
            int beforeUnspent = hpd.UnspentOrbitPoints;
            int beforeTotalOrbit = hpd.TotalOrbitPoints;

            int applied = 0;
            for (int i = 0; i < addLevels; i++)
            {
                if (hpd.Level >= 100)
                    break;

                hpd.Level += 1;
                hpd.TotalOrbitPoints += orbitPerLevel;
                hpd.UnspentOrbitPoints += orbitPerLevel;

                applied++;
            }

            DebugManager.Log(
                $"CheatMenu: LevelUp hero='{heroId}' +{applied} (lvl {beforeLevel}->{hpd.Level}) | Orbit unspent {beforeUnspent}->{hpd.UnspentOrbitPoints} | total {beforeTotalOrbit}->{hpd.TotalOrbitPoints}",
                DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);
        }

        private void ResetOrbitPoints(string heroId)
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Profile == null)
            {
                DebugManager.Log("CheatMenu: missing GameManager/Profile for ResetOrbitPoints.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            heroId = heroId?.Trim();
            if (string.IsNullOrEmpty(heroId))
            {
                DebugManager.Log("CheatMenu: heroId empty for ResetOrbitPoints.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            var hpd = gm.Profile.GetOrCreateHeroProgress(heroId);
            if (hpd == null)
            {
                DebugManager.Log($"CheatMenu: could not get HeroProgressData for '{heroId}' (ResetOrbitPoints).",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            var before = hpd.UnspentOrbitPoints;
            hpd.UnspentOrbitPoints = hpd.TotalOrbitPoints;

            DebugManager.Log($"CheatMenu: ResetOrbitPoints hero='{heroId}' unspent {before}->{hpd.UnspentOrbitPoints} (total={hpd.TotalOrbitPoints})",
                DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);
        }

        private void ResetHeroToLevel1(string heroId)
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Profile == null)
            {
                DebugManager.Log("CheatMenu: missing GameManager/Profile for ResetHeroToLevel1.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            heroId = heroId?.Trim();
            if (string.IsNullOrEmpty(heroId))
            {
                DebugManager.Log("CheatMenu: heroId empty for ResetHeroToLevel1.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            var hpd = gm.Profile.GetOrCreateHeroProgress(heroId);
            if (hpd == null)
            {
                DebugManager.Log($"CheatMenu: could not get HeroProgressData for '{heroId}' (ResetHeroToLevel1).",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            ApplyResetToLevel1(hpd);

            DebugManager.Log($"CheatMenu: ResetHeroToLevel1 hero='{heroId}' -> lvl={hpd.Level}, XP={hpd.CurrentXP}/{hpd.TotalXP}, Orbit total={hpd.TotalOrbitPoints}, unspent={hpd.UnspentOrbitPoints}",
                DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);
        }

        private void ResetAllHeroesProgress()
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Profile == null)
            {
                DebugManager.Log("CheatMenu: missing GameManager/Profile for ResetAllHeroesProgress.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Error);
                return;
            }

            // Wichtig: wir resetten alle EXISTIERENDEN HeroProgressDatas im Save.
            // Wenn du auch alle "unlockable heroes" forcieren willst, ist das ein anderer Button.
            var list = gm.Profile.HeroesData;
            if (list == null || list.Count == 0)
            {
                DebugManager.Log("CheatMenu: ResetAllHeroesProgress: Profile.HeroesData empty.",
                    DebugManager.EDebugLevel.Dev, "Cheat", LogType.Warning);
                return;
            }

            int changed = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var hpd = list[i];
                if (hpd == null) continue;

                ApplyResetToLevel1(hpd);
                changed++;
            }

            DebugManager.Log($"CheatMenu: ResetAllHeroesProgress -> reset {changed} heroes to level 1 and orbit=0.",
                DebugManager.EDebugLevel.Dev, "Cheat", LogType.Log);

            // Optional: refresh hero dropdown if it shows stale selection
            // SetHeroDropdownFromProfile();  // only if you have that method already
        }

        private static void ApplyResetToLevel1(HeroProgressData hpd)
        {

            hpd.Level = 1;

            hpd.TotalOrbitPoints = 0;
            hpd.UnspentOrbitPoints = 0;

            // XP reset ist logisch bei "Level 1".
            hpd.CurrentXP = 0;
            hpd.TotalXP = 0;

            hpd.UnlockedSockets = 0;
        }

        private void ResetAllResearch()
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Profile == null)
            {
                DebugManager.Log("CheatMenu: ResetAllResearch failed (GameManager/Profile missing).",
                    DebugManager.EDebugLevel.Dev, "Research", LogType.Error);
                return;
            }

            // Runtime-Container sicherstellen
            if (gm.Profile.ResearchRuntime == null)
                gm.Profile.ResearchRuntime = new CHAL.Systems.Research.CodexState();

            var rt = gm.Profile.ResearchRuntime;

            // State leeren
            rt.activeNodeId = null;
            rt.completedNodeIds.Clear();
            rt.perNodeProgress.Clear();

            // Save löschen + leeren Snapshot speichern (wie GameManager.InitResearch(false), aber ohne Event-Rebind)
            SaveSystem.DeleteResearch(gm.Profile.profileId);
            SaveSystem.SaveResearch(gm.Profile.profileId, gm.Profile.BuildResearchSnapshotFrom(rt));

            // UnlockRegistry neu aufsetzen (leer, plus AlwaysUnlocked)
            if (gm.ResearchUnlocks != null)
            {
                var nodes = LoadResearchNodes();
                gm.ResearchUnlocks.RebuildFrom(nodes, rt.completedNodeIds);

                var tree = LoadResearchTree();
                if (tree != null && tree.alwaysUnlockedIds != null)
                    gm.ResearchUnlocks.ApplyAlwaysUnlocked(tree.alwaysUnlockedIds);
            }

            DebugManager.Log("CheatMenu: Reset all Research (state cleared + snapshot saved + unlock registry rebuilt).",
                DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
        }

        private void UnlockAllResearch()
        {
            var gm = GameManager.Instance;
            if (gm == null || gm.Profile == null)
            {
                DebugManager.Log("CheatMenu: UnlockAllResearch failed (GameManager/Profile missing).",
                    DebugManager.EDebugLevel.Dev, "Research", LogType.Error);
                return;
            }

            if (gm.Profile.ResearchRuntime == null)
                gm.Profile.ResearchRuntime = new CHAL.Systems.Research.CodexState();

            var rt = gm.Profile.ResearchRuntime;

            rt.activeNodeId = null;
            rt.perNodeProgress.Clear();
            rt.completedNodeIds.Clear();

            // Alle Nodes als completed markieren
            var nodes = LoadResearchNodes();
            for (int i = 0; i < nodes.Count; i++)
            {
                var n = nodes[i];
                if (n == null) continue;

                var id = string.IsNullOrWhiteSpace(n.id) ? null : n.id.Trim();
                if (string.IsNullOrEmpty(id)) continue;

                rt.completedNodeIds.Add(id);
            }

            // Snapshot speichern
            SaveSystem.SaveResearch(gm.Profile.profileId, gm.Profile.BuildResearchSnapshotFrom(rt));

            // UnlockRegistry rebuild + AlwaysUnlocked
            if (gm.ResearchUnlocks != null)
            {
                gm.ResearchUnlocks.RebuildFrom(nodes, rt.completedNodeIds);

                var tree = LoadResearchTree();
                if (tree != null && tree.alwaysUnlockedIds != null)
                    gm.ResearchUnlocks.ApplyAlwaysUnlocked(tree.alwaysUnlockedIds);
            }

            DebugManager.Log($"CheatMenu: Unlock all Research (completedNodes={rt.completedNodeIds.Count}).",
                DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
        }

        // --- local loaders (keine zusätzliche Registry-Logik; identisch zum GM-Pfad) ---
        private static CodexTreeDef LoadResearchTree()
        {
            return Resources.Load<CodexTreeDef>("data/Research/Tree");
        }

        private static List<CodexNodeDef> LoadResearchNodes()
        {
            var arr = Resources.LoadAll<CodexNodeDef>("data/Research/Nodes");
            var list = new List<CodexNodeDef>(arr != null ? arr.Length : 0);
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                    list.Add(arr[i]);
            }
            return list;
        }


    }
}
