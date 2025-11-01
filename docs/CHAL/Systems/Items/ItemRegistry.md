# CHAL.Systems.Items.ItemRegistry

_Automatically generated/updated from `Assets/src/Systems/Items/ItemRegistry.cs`._

```csharp
Purpose
- Defines a singleton-like ItemRegistry as a ScriptableObject that loads ItemDef assets from Resources.
- Maintains an in-memory dictionary mapping itemId to ItemDef (_byId).
- Loads a ModulePartMap JSON from Resources and validates that mapping against loaded items, creating placeholders and a CSV report on issues.

Public API
- Namespace/module
  - CHAL.Systems.Items

- Types
  - public sealed class ItemRegistry : ScriptableObject
    - Public fields/properties
      - public static ItemRegistry Instance { get; }
        - Lazy singleton accessor; creates an instance and calls Reload on first access.
    - Public methods
      - public void Reload()
        - Clears _byId, loads all ItemDef assets from Resources/data/Items, validates IDs, and populates _byId.
        - Loads and validates ModulePartMap after item load.
      - public bool TryGet(string itemId, out ItemDef def)
        - Attempts to retrieve an ItemDef by id; returns true if found.
      - public Rarity GetRarity(string itemId)
        - Returns rarity for given id; defaults to Rarity.Common if not found.
      - public int GetLootValue(string itemId)
        - Returns lootValue for given id; defaults to 0 if not found.
      - public bool Exists(string itemId)
        - Returns true if itemId exists in _byId.
      - public IEnumerable<ItemDef> GetAllItemsByType(string typePrefix)
        - Yields ItemDef instances whose IDs start with "{typePrefix}:" (case-insensitive).
      - public void CreatePlaceholderitem(string itemId)
        - Creates a placeholder ItemDef asset under Assets/Resources/data/Items/{prefix}/missing with basic fields.
      - public ItemType GetTypeOf(string itemId)
        - Determines item type via ItemTypeUtils.FromId(itemId).
      - public bool IsType(string itemId, ItemType t)
        - Returns true if the item’s type matches t.
      - public void TriggerInstance()
        - Logs a diagnostic message (debug helper).

Key Behavior & Side Effects
- Lazy singleton and reload
  - Accessing Instance for the first time creates a new ItemRegistry via CreateInstance<ItemRegistry>() and calls Reload().
- Loading items
  - Reload clears the registry, loads all ItemDef assets from Resources/data/Items, and builds _byId.
  - For each def:
    - Skips if def.itemId is null/empty or not parseable by ItemKey.TryParse; logs a warning.
    - Skips if a duplicate itemId exists; logs a warning.
    - Adds valid items to _byId.
  - Logs the loaded item count.
- ModulePartMap loading and validation
  - Reload calls LoadModulePartMap, which loads Resources/data/Items/ModulePartMap as TextAsset.
  - If missing, logs a warning and returns null.
  - If present, parses via JsonUtility.FromJson<ModulePartMapWrapper> and converts to a dictionary; logs count.
  - ValidateModulePartMap(mod_part_map) checks:
    - Every module key exists in _byId; otherwise creates a placeholder and records an error.
    - Every part listed in values exists in _byId; otherwise creates a placeholder and records an error.
    - Each module has at least one part; records an error if empty.
    - Parts that appear in no module are flagged.
  - If any errors exist:
    - Logs warnings for each error.
    - Writes a ModulePartValidation.csv next to the project Assets folder.
    - Logs the path to the saved report.
  - If no errors, logs that ModulePartMap is fully valid.
- Lookups and helpers
  - TryGet/GetRarity/GetLootValue/Exists rely on the current _byId contents.
  - GetAllItemsByType filters IDs by prefix.
  - CreatePlaceholderitem creates an ItemDef asset with sane defaults and writes via AssetDatabase (Editor tooling).
  - GetTypeOf/IsType route through ItemTypeUtils.FromId.
  - TriggerInstance logs a diagnostic message (no state mutation).

Constraints & Failure Modes
- Editor tooling
  - Uses UnityEditor.AssetDatabase and file I/O; intended for editor tooling, not runtime.
- Null/empty handling gaps
  - LoadModulePartMap may return null; ValidateModulePartMap assumes a non-null dictionary and will throw if null.
- Thread safety
  - Instance initialization is not thread-safe; concurrent access could race between checks.
- Performance
  - Reload loads all ItemDef assets and processes ModulePartMap on each call; may be expensive on large catalogs.
- Placeholder creation
  - CreatePlaceholderitem writes new assets under Assets/Resources/... and refreshes the AssetDatabase; requires editor context.
- Logging and reports
  - Validation errors generate a CSV report at a fixed path, which may be overwritten on subsequent runs.

Example
- Minimal usage snippet:
```csharp
var registry = CHAL.Systems.Items.ItemRegistry.Instance;
if (registry.TryGet("part:wood", out var woodDef))
{
    // Use woodDef (e.g., access woodDef.itemId, woodDef.rarity, etc.)
}
```

Unknowns
- Definitions of ItemDef, Rarity, ItemType, ItemKey, and ItemTypeUtils are external to this file.
- Structure and content of ModulePartMapWrapper and the exact JSON schema are not shown here.
- Exact runtime behavior outside the Unity Editor (e.g., Play mode) is not defined; AssetDatabase usage indicates editor-only tooling.
- The precise contents and organization of Resources/data/Items and the expected module/part naming conventions are not specified beyond usage.
- Concurrency guarantees or explicit thread-safety notes are not provided in code.
