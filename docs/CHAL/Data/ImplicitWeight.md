# CHAL.Data.ImplicitWeight

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitGearTypeConfig.cs`._

# Purpose
- Defines the configuration for implicit gear types in the game, allowing for easy asset management through Unity's ScriptableObject.

# Public API
- Namespace: CHAL.Data
- Types
  - public class ImplicitGearTypeConfig : ScriptableObject
    - Public fields/properties:
      - List<GearTypePool> Pools: Collection of gear type pools, each containing implicit weights.
    - Public methods:
      - void OnValidate(): Validates the gear type pools, ensuring no duplicates, clamping negative weights, and trimming IDs.
      - static bool IsValidId(string id): Checks if the given ID is valid (only allows a-z, 0-9, and _).

  - [Serializable] public struct GearTypePool
    - Public fields/properties:
      - GearType GearType: The type of gear.
      - List<ImplicitWeight> Entries: List of implicit weights associated with the gear type.

  - [Serializable] public struct ImplicitWeight
    - Public fields/properties:
      - string ImplicitId: The identifier for the implicit weight.
      - int Weight: The weight associated with the implicit ID.

# Key Behavior & Side Effects
- OnValidate method performs the following:
  - Avoids duplicates for each gear type.
  - Clamps negative weights to zero.
  - Trims implicit IDs.
  - Issues warnings for unusual IDs and duplicates.
  - Adds missing default IDs with a weight of zero.

# Constraints & Failure Modes
- Handles null/empty lists for Pools and Entries gracefully.
- Validates IDs to ensure they conform to the specified format.
- Warnings are logged for unusual IDs and duplicate implicit IDs.

# Example
```csharp
// Example of creating an instance of ImplicitGearTypeConfig
ImplicitGearTypeConfig config = ScriptableObject.CreateInstance<ImplicitGearTypeConfig>();
```

# Unknowns
- None.

