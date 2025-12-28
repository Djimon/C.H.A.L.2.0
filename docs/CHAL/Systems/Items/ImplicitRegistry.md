# Assets/src/Systems/Items/Gear/ImplicitRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Items/Gear/ImplicitRegistry.cs`._

# Purpose
- Defines the `ImplicitRegistry` class for managing implicit definitions associated with gear items.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - `public sealed class ImplicitRegistry`
    - **Public fields/properties**: None
    - **Public methods**:
      - `public ImplicitRegistry(ImplicitRegistryDef def)` - Initializes the registry with the provided definition.
      - `public bool TryGet(string implicitId, out ImplicitDef def)` - Attempts to retrieve an implicit definition by its ID.
      - `public void GetCandidates(ImplicitPool pool, ImplicitRole role, GearType gearType, List<ImplicitDef> buffer)` - Fills the provided buffer with candidates filtered by pool, role, and allowed gear types.

# Key Behavior & Side Effects
- The constructor validates the provided `ImplicitRegistryDef` and populates internal dictionaries.
- `TryGet` returns false if the `implicitId` is null or empty, or if the ID is not found.
- `GetCandidates` clears the provided buffer and populates it with valid implicit definitions based on the specified pool, role, and gear type.

# Constraints & Failure Modes
- The constructor logs an error if the `ImplicitRegistryDef` is null or empty.
- Duplicate IDs are ignored with a warning.
- The `GetCandidates` method does not add any definitions to the buffer if no valid candidates are found.

# Example
```csharp
var registry = new ImplicitRegistry(implicitRegistryDef);
if (registry.TryGet("someImplicitId", out var implicitDef))
{
    // Use implicitDef
}

var candidates = new List<ImplicitDef>();
registry.GetCandidates(implicitPool, implicitRole, gearType, candidates);
// candidates now contains filtered implicit definitions
```

# Unknowns
- The structure and properties of `ImplicitRegistryDef`, `ImplicitDef`, `ImplicitPool`, `ImplicitRole`, and `GearType` cannot be determined from this file.

