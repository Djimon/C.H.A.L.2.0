# Assets/src/Data/Defs/ImplicitRegistryDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitRegistryDef.cs`._

1) Purpose
- Defines the `ImplicitRegistryDef` class as a ScriptableObject for managing a list of `ImplicitDef` instances.

2) Public API
- Namespace/module: `CHAL.Data`
- Types
  - public sealed class `ImplicitRegistryDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `List<ImplicitDef> Implicits`: List of implicit definitions.
    - Public methods:
      - `void OnValidate()`: Validates the `Implicits` list by removing null entries and duplicates based on `ImplicitId`.

3) Key Behavior & Side Effects
- On validation, the method removes null entries from the `Implicits` list.
- It trims duplicates by `ImplicitId`, keeping the first occurrence and logging warnings for empty or duplicate IDs.

4) Constraints & Failure Modes
- If `Implicits` is null, the method exits early without processing.
- The method handles empty or null `ImplicitId` by removing the corresponding entry and logging a warning.
- Uses a `HashSet` to track seen IDs for duplicate detection.

5) Example
```csharp
// Example of creating an ImplicitRegistryDef asset
var implicitRegistry = ScriptableObject.CreateInstance<ImplicitRegistryDef>();
implicitRegistry.Implicits.Add(new ImplicitDef { ImplicitId = "unique_id_1" });
```

6) Unknowns
- The structure and properties of `ImplicitDef` are not defined in this file.
