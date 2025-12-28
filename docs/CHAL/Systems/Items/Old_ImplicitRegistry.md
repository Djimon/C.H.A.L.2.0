# Assets/src/Systems/Items/Gear/Old_ImplicitRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Items/Gear/Old_ImplicitRegistry.cs`._

# Purpose
- Defines the `ImplicitRegistry` class for managing implicit definitions based on IDs and pool-role combinations.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - `public sealed class ImplicitRegistry`
    - **Public fields/properties**: None
    - **Public methods**:
      - `public bool TryGet(string implicitId, out ImplicitDef def)`
        - Returns true if the `ImplicitDef` is found by its ID; otherwise, false.
      - `public void GetCandidates(ImplicitPool pool, ImplicitRole role, GearType gearType, List<ImplicitDef> buffer)`
        - Fills `buffer` with candidates filtered by `AllowedGearTypes`.

  - `private readonly struct PoolRoleKey : IEquatable<PoolRoleKey>`
    - **Public fields/properties**: 
      - `public readonly ImplicitPool Pool`
      - `public readonly ImplicitRole Role`
    - **Public methods**:
      - `public bool Equals(PoolRoleKey other)`
        - Determines equality with another `PoolRoleKey`.
      - `public override bool Equals(object obj)`
        - Determines equality with an object.
      - `public override int GetHashCode()`
        - Returns the hash code for the instance.

# Key Behavior & Side Effects
- The constructor initializes the registry from an `ImplicitRegistryDef`, logging errors for missing or empty definitions.
- Duplicate implicit IDs are logged as warnings and ignored.
- The `TryGet` method returns false for null or empty IDs.
- The `GetCandidates` method clears the provided buffer before populating it with valid candidates.

# Constraints & Failure Modes
- The constructor checks for null or empty `ImplicitRegistryDef` and its `Implicits` list.
- The `TryGet` method handles null or empty `implicitId`.
- The `GetCandidates` method requires a pre-initialized `List<ImplicitDef>` for output.

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
- The structure and properties of `ImplicitDef`, `ImplicitRegistryDef`, `ImplicitPool`, `ImplicitRole`, and `GearType` cannot be determined from this file.

