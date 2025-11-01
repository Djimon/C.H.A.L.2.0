# global.ImplicitGearTypeConfigEditor

_Automatically generated/updated from `Assets/src/Editor/ImplicitGearConfigEditor.cs`._

```text
Purpose
- Editor extension: CustomInspector for ImplicitGearTypeConfig (Unity Editor only).
- Provides UI to paste/parse an implicit gear grid (TSV), copy templates from/to clipboard, and apply weights to the asset.
- Encapsulates template ordering and TSV parsing, weight extraction, and applying changes to GearTypePools in the asset.
```

```text
Public API
- Namespace/module: global (no explicit namespace)
- Type
  - public class ImplicitGearTypeConfigEditor : UnityEditor.Editor
    - Public methods
      - public override void OnInspectorGUI()
```

```text
Key Behavior & Side Effects
- Inspector UI flow
  - Draws default inspector.
  - Shows sections for pasting a grid, templates, and parsing/applying.
  - Displays status/info messages via status field and HelpBox.
- Templates and clipboard
  - CopyBlankTemplateToClipboard(asset):
    - Builds a TSV header: "ID" followed by GearType names in TemplateGears order.
    - If asset has no IDs (GatherIdsFromAsset), copies only header and sets status accordingly.
    - Otherwise adds a row per ID with all gear weights set to 0.
    - Writes TSV to system clipboard and updates status.
  - CopyFromAssetToClipboard(asset):
    - Validates asset non-null; ensures all GearTypes exist (EnsureAllGearTypesExist).
    - Builds a weight map per ID from asset (BuildWeightMap).
    - Collects all IDs from asset pools and weights, takes a distinct, ordered list.
    - Produces TSV with header and a row per ID using BuildLineForId.
    - Writes TSV to clipboard and updates status.
- Applying a grid
  - ApplyFromGrid(asset, text):
    - Parses TSV into headers and rows (ParseTSV).
    - Maps column headers to GearTypes (MapColumnsToGearTypes); requires at least one valid gear header.
    - Builds per-ID dictionary of GearType → Weight from the grid (skipping invalid columns; values default to 0).
    - Undoes changes via Undo.RecordObject.
    - Ensures all GearTypes exist in the asset (EnsureAllGearTypesExist).
    - For each (id, gearType, weight):
      - Finds or creates the corresponding GearTypePool in asset.Pools for that gearType.
      - Finds or adds an ImplicitWeight entry for the ID within the pool; updates weight if exists.
      - Writes back the pool to asset.Pools (struct allocation handled).
    - Marks asset dirty and saves assets; updates status with a summary.
    - Catches exceptions and stores error messages in status.
- Helpers and data flow
  - ParseTSV(text): splits into headers and rows by tabs/newlines.
  - Safe(row, i): helper for safe column access.
  - Norm(s): lowercase/trim helper for header matching.
  - MapColumnsToGearTypes(headers): maps supported headers (head, chest, gloves, legs, boots, amulet) to GearType values.
  - EnsureAllGearTypesExist(asset): ensures asset.Pools contains a pool for every GearType.
  - GatherIdsFromAsset(asset): collects all ImplicitId values from all pool entries.
  - BuildWeightMap(asset): builds per-id, per-gear-type weight map from asset pools.
  - BuildLineForId(id, weights): renders one TSV row for an id given a per-gear-type weight map.
```

```text
Constraints & Failure Modes
- Editor-only compilation guarded by #if UNITY_EDITOR.
- ApplyFromGrid wraps processing in try/catch; status shows error messages on exceptions.
- Requires at least one valid gear header in TSV; otherwise throws.
- MapColumnsToGearTypes only recognizes specific lowercase headers: gloves, head, legs, chest, boots, amulet.
- Null asset handling:
  - CopyFromAssetToClipboard handles asset == null with status.
  - ApplyFromGrid assumes a non-null asset; otherwise null reference may occur.
- Data integrity:
  - When updating, existing ImplicitWeight entries are updated; new entries are created if missing.
  - EnsureAllGearTypesExist may mutate asset to include missing pools for all GearTypes.
- Performance notes:
  - Iterates over asset pools/entries; uses per-ID dictionaries; may allocate several small collections.
```

```text
Example
- TSV to paste (Head, Chest, Gloves, Legs, Boots, Amulet)
- Minimal valid grid:
ID	Head	Chest	Gloves	Legs	Boots	Amulet
123	1	0	2	0	0	0
```

```csharp
// Example TSV (see Example above)
// Use in Paste Grid → Apply area in the editor UI.
ID	Head	Chest	Gloves	Legs	Boots	Amulet
123	1	0	2	0	0	0
```

```text
Unknowns
- Exact definitions of:
  - ImplicitGearTypeConfig
  - GearType
  - GearTypePool
  - ImplicitWeight
  - Related data structures (Pools, Entries) beyond usage here
- Behavior beyond editor UI (runtime usage of the config) is not defined in this file.
```
