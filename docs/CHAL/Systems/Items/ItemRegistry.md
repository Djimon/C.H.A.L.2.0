# CHAL.Systems.Items.ItemRegistry

_Automatically generated/updated from `Assets/src/Systems/Items/ItemRegistry.cs`._

```csharp
// Documentation for: Assets/src/Systems/Items/ItemRegistry.cs
```

1) Purpose
- Defines a singleton ScriptableObject-based registry for items (ItemDef) loaded from Unity Resources.
- Builds an in-memory lookup by itemId and exposes basic accessors (rarity, loot value, existence, type).
- Performs module/gear/recipe validations, creates placeholder items for missing references, and writes validation reports to CSV files (ModulePartValidation.csv, etc.). 

2) Public API
- Namespace: CHAL.Systems.Items
- Type: public sealed class ItemRegistry : ScriptableObject
  - Public static ItemRegistry Instance
    - ItemRegistry Instance { get; }
    - Lazy-initializes a registry instance and calls Reload().
  - Public void Reload()
    - Clears _byId; loads all ItemDef assets from Resources/data/Items; validates IDs; logs warnings for invalid IDs or duplicates; populates _byId.
    - Loads and validates ModulePartMap; validates gear and recipes; may generate reports and placeholders.
  - Public bool TryGet(string itemId, out ItemDef def)
    - Tries to fetch the ItemDef by itemId from the internal dictionary.
  - Public Rarity GetRarity(string itemId)
    - Returns the item’s rarity or Rarity.Common if not found.
  - Public int GetLootValue(string itemId)
    - Returns the item’s lootValue or 0 if not found.
  - Public bool Exists(string itemId)
    - True if the itemId exists in the registry.
  - Public IEnumerable<ItemDef> GetAllItemsByType(string typePrefix)
    - Yields all ItemDef where the key starts with typePrefix + ":" (case-insensitive).
  - Public void CreatePlaceholderitem(string itemId)
    - Creates a placeholder ItemDef asset under Assets/Resources/data/Items/{prefix}/missing with basic fields and saves via AssetDatabase.
  - Public ItemType GetTypeOf(string itemId)
    - Returns the item type derived from Id using ItemTypeUtils.FromId(itemId).
  - Public bool IsType(string itemId, ItemType t)
    - True if the item’s type matches t.
  - Public void TriggerInstance()
    - Debug hook/diagnostic to indicate instance was triggered.
- Private state
  - private static ItemRegistry _instance;
  - private readonly Dictionary<string, ItemDef> _byId = new();

Notes
- The class relies on other types (ItemDef, RecipeDef, ModulePartMapWrapper, Rarity, ItemType, ItemTypeUtils, DebugManager, ItemKey) defined elsewhere in the project.

3) Key Behavior & Side Effects
- Instance getter:
  - Creates a new ItemRegistry via CreateInstance<ItemRegistry>() on first access, then calls Reload().
- Reload flow:
  - _byId.Clear()
  - Load all ItemDef assets from Resources/data/Items and validate:
    - Skip if def.itemId is null/empty or not parseable by ItemKey.TryParse
    - Warn on duplicates
    - Add valid defs to _byId (keyed by itemId)
  - Logs the loaded item count
  - LoadModulePartMap() and ValidateModulePartMap(_modulePartMap)
  - ValidateGearAndRecipes(reportPath) appends issues to a CSV file (ModulePartValidation.csv) under the project when issues exist
- Validation specifics:
  - Gear validation:
    - Checks gear assets under Resources/data/Items/gear
    - Validates non-empty itemId, correct prefix (gear:)
    - Warns if gear itemId not in registry
  - Recipes validation:
    - Checks outputItemId in RecipeDef assets under Resources/data/Recipes
    - Warns if output is empty
    - If missing, creates a placeholder item for the output
  - Placeholder creation:
    - Creates placeholder ItemDefs via CreatePlaceholderitem
  - Reports:
    - Appends rows to ModulePartValidation.csv; creates header if file does not exist
    - Logs success or warnings during report writing
- ModulePartMap validation:
  - Loads module/part mapping from data/Items/ModulePartMap JSON
  - Validates that each module exists in registry; each part exists in registry; each module has at least one part
  - Checks that all mapped parts appear in at least one module
  - Creates placeholders for missing modules/parts as needed
  - Writes per-run validation report to ModulePartValidation.csv and logs results
- Placeholder item creation:
  - Derives folder = Assets/Resources/data/Items/{prefix}/missing
  - Creates folder if needed, asset path, and a new ItemDef with itemId, description, rarity, lootValue
  - Uses UnityEditor.AssetDatabase to create/save/refresh the asset
- Editor tooling:
  - Uses UnityEditor APIs (AssetDatabase) and writes to project paths; intended for editor-time validation and tooling

4) Constraints & Failure Modes
- Guarded IO/validation:
  - ModulePartMap load may return null; ValidateModulePartMap handles it gracefully
  - Gear/Recipe validation accounts for empty arrays; reports warnings when missing
  - Report writing is enclosed in try/catch to avoid crashing during IO errors
- Side effects:
  - Placeholder item creation writes new assets to the project (AssetDatabase)
  - Validation reports are stored in CSV files; may be overwritten or appended each time
- Runtime vs editor:
  - Heavy reliance on UnityEditor APIs means this code is editor tooling; may not run in a runtime build

5) Example
- Minimal usage snippet (conceptual):
```csharp
using CHAL.Systems.Items;

var registry = ItemRegistry.Instance;
if (registry.TryGet("gear:sword_iron", out var def))
{
    Debug.Log($"Item: {def.name}, Description: {def.description}");
}
```

6) Unknowns
- Definitions/shape of ItemDef, RecipeDef, ModulePartMapWrapper, and their exact serialized fields beyond those used here
- Details of DebugManager, ItemKey.TryParse, Rarity, ItemType, and ItemTypeUtils
- Whether there are additional side effects at runtime beyond editor tooling behavior
- Any runtime-specific initialization order beyond the lazy Instance construction and Reload call
