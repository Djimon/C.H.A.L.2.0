# global.ItemDefEditor

_Automatically generated/updated from `Assets/src/Editor/ItemDefEdtitor.cs`._

1) Purpose
- Defines a Unity Editor custom inspector for ItemDef via [CustomEditor(typeof(ItemDef))].
- Renders and edits core item fields (Item ID, Icon, Rarity, Loot Value) and conditionally renders type-specific fields based on itemId prefixes.
- Automatically creates nested data blocks when needed and marks the target as dirty on changes.

2) Public API
- Namespace/module
  - Global namespace (Unity Editor integration)
- Types
  - public class ItemDefEditor : Editor
    - Public methods
      - public override void OnInspectorGUI()

3) Key Behavior & Side Effects
- Inspector rendering
  - item = (ItemDef)target
  - Always shows basis fields:
    - item.itemId = TextField("Item ID", item.itemId)
    - item.icon = ObjectField("Icon", item.icon, Sprite, false)
    - item.rarity = EnumPopup("Rarity", item.rarity)
    - item.lootValue = IntField("Loot Value", item.lootValue)
  - Type-specific section header: "Type Specific"
  - Type-specific fields based on itemId prefixes:
    - "remains:": Ensure(ref item.remainData); item.remainData.remainType = TextField("Remain Type", ...)
    - "rune:": Ensure(ref item.runeData); item.runeData.effectType = TextField("Effect Type", ...); item.runeData.runeColortType = EnumPopup("Rune Color", ...)
    - "part:": Ensure(ref item.partData); item.partData.dnaType = TextField("DNA Type", ...)
    - "module:": Ensure(ref item.moduleData); item.moduleData.modulePower = FloatField("Base Power", ...); item.moduleData.effect = TextField("Effect", ...)
    - "gear:": Ensure(ref item.gearData); item.gearData.slotType = EnumPopup("Slot Type", ...); DrawStringArray(ref item.gearData.tags, "Tag"); item.gearData.runeSocketType = EnumPopup("Rune Socket", ...)
    - default: Show info box about unknown prefix and supported prefixes
- Change persistence
  - if GUI.changed → EditorUtility.SetDirty(target)
- Helper mechanisms (local to OnInspectorGUI)
  - Ensure<T>(ref T field) where T : class, new(): creates new instance if null
  - DrawStringArray(ref string[] arr, string label): UI to edit a dynamic string array
- Data surface visible/edited
  - ItemDef core fields and nested data blocks (remainData, runeData, partData, moduleData, gearData)

4) Constraints & Failure Modes
- Editor-only code: wrapped in #if UNITY_EDITOR
- Potential null risk: item.itemId.StartsWith(...) is called without a null check; if itemId is null, this would throw a NullReferenceException
- Prefix-based logic: only handles remains:, rune:, part:, module:, gear:; unknown prefixes are reported but unhandled
- Serialization/asset behavior: relies on Unity editor workflow; exact runtime serialization details depend on ItemDef and nested types (not defined in this file)
- Performance: UI builds and object-initialization occur during Inspector redraws

5) Example
- Not derivable from this file alone; no standalone usage example is provided.

6) Unknowns
- Definitions and structure of ItemDef and all nested data types (RemainData, RuneData, PartData, ModuleData, GearData, GearType, RuneColorType, Rarity)
- Exact serialization behavior and whether these nested data objects are ScriptableObjects or plain classes
- Any additional itemId prefixes or editor behaviors outside the shown code
- Any runtime implications of editing these fields (e.g., validation rules, asset regeneration) not specified in this file
