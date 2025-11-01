# global.ItemDefEditor

_Automatically generated/updated from `Assets/src/Editor/ItemDefEdtitor.cs`._

```text
1) Purpose
- Defines a Unity Editor custom inspector for ItemDef using [CustomEditor(typeof(ItemDef))].
- Renders and edits common ItemDef fields (Item ID, Icon, Rarity, Loot Value) and shows type-specific fields based on itemId prefixes.
- Encodes editor-side helpers (Ensure<T> and DrawStringArray) as local functions within OnInspectorGUI to initialize nested data and render string arrays.

2) Public API
- Namespace/module: global (no namespace)
- Types
  - public class ItemDefEditor : Editor
    - [CustomEditor(typeof(ItemDef))] binding to ItemDef
    - public override void OnInspectorGUI()
      - Renders and edits ItemDef fields; performs type-specific UI based on itemId prefixes
      - Saves changes via EditorUtility.SetDirty(target) when GUI.changed is true

3) Key Behavior & Side Effects
- Inspects target as ItemDef: var item = (ItemDef)target;
- Always displays common fields:
  - item.itemId via TextField
  - item.icon via ObjectField (Sprite)
  - item.rarity via EnumPopup
  - item.lootValue via IntField
- Type-specific sections (based on item.itemId prefixes):
  - "remains:": Ensure(ref item.remainData); item.remainData.remainType = TextField
  - "rune:": Ensure(ref item.runeData); item.runeData.effectType = TextField; item.runeData.runeColortType = EnumPopup
  - "part:": Ensure(ref item.partData); item.partData.dnaType = TextField
  - "module:": Ensure(ref item.moduleData); item.moduleData.modulePower = FloatField; item.moduleData.effect = TextField
  - "gear:": Ensure(ref item.gearData); item.gearData.slotType = EnumPopup; DrawStringArray(ref item.gearData.tags, "Tag"); item.gearData.runeSocketType = EnumPopup
  - Unknown prefix: shows HelpBox with known prefixes
- Helpers (local functions within OnInspectorGUI):
  - Ensure<T>(ref T field) where T : class, new(): initializes null fields with new T()
  - DrawStringArray(ref string[] arr, string label): renders and edits a dynamic string array with a Count field and per-element TextField
- State persistence:
  - If GUI.changed, marks target dirty via EditorUtility.SetDirty(target)

4) Constraints & Failure Modes
- UNITY_EDITOR guard: code only included in the editor; not included in runtime builds.
- Ensure<T> requires T to be a class with a parameterless constructor; otherwise compilation/runtime could fail.
- item.itemId prefixes are string-based; incorrect/missing prefixes fall back to the Unknown-item HelpBox.
- DrawStringArray assumes arr may be null; handles via Count field and array reallocation.
- Updates to nested data (remainData, runeData, etc.) are created on-demand; may impact serialization if those fields are not serialized in ItemDef.

5) Example
- Not clearly derivable from this file (no usage example provided).

6) Unknowns
- Definition and structure of ItemDef and nested data types (RemainData, RuneData, PartData, ModuleData, GearData) are not present in this file.
- Definitions for Rarity, RuneColorType, GearType, and related enums/classes are not shown.
- Exact serialization behavior of ItemDef fields and how changes interact with the rest of the system are not specified here.
```
