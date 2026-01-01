# Assets/src/Systems/Enemy/MonsterTagRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Enemy/MonsterTagRegistry.cs`._

# Purpose
- Defines the `MonsterTagRegistry` class for managing monster tags in the game.

# Public API
- Namespace: `CHAL.Systems.Enemy`
- Types
  - **public sealed class MonsterTagRegistry**
    - **Public fields/properties**
      - `static MonsterTagRegistry Instance`: Singleton instance of the registry.
      - `IReadOnlyCollection<MonsterTagDef> All`: Gets all loaded monster tag definitions.
    - **Public methods**
      - `void LoadAll(bool force = false)`: Loads all monster tag definitions from resources; clears existing tags if `force` is true.
      - `bool TryGet(string tagId, out MonsterTagDef def)`: Attempts to retrieve a monster tag definition by its ID.
      - `bool IsKnown(string tagId)`: Checks if a monster tag ID is known.
      - `void ExportCsv(string exportPath)`: Exports the current registry snapshot to a CSV file.

# Key Behavior & Side Effects
- Loads monster tag definitions from the specified resource path when `LoadAll` is called.
- Logs warnings for empty or duplicate tag IDs during loading.
- Exports the loaded tags to a CSV file when `ExportCsv` is called.

# Constraints & Failure Modes
- `LoadAll` will not reload tags if already loaded unless `force` is true.
- `TryGet` and `All` will automatically call `LoadAll` if the tags are not yet loaded.
- The directory for CSV export is created if it does not exist.

# Example
```csharp
var registry = MonsterTagRegistry.Instance;
registry.LoadAll(); // Load tags
if (registry.TryGet("exampleTagId", out var tagDef))
{
    // Use tagDef
}
```

# Unknowns
- None.

