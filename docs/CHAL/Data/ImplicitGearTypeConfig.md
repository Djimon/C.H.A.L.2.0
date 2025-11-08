# CHAL.Data.ImplicitGearTypeConfig

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitGearTypeConfig.cs`._

# Purpose
- Defines the configuration for implicit gear types in the game using a ScriptableObject for asset management.

# Public API
- Namespace: CHAL.Data
- Types
  - public class ImplicitGearTypeConfig : ScriptableObject
    - Public fields/properties:
      - List<GearTypePool> Pools: Collection of gear type pools with implicit weights.
    - Public methods:
      - private void OnValidate(): Validates and adjusts the gear type pools and their entries.
      - private static bool IsValidId(string id): Checks if the given ID is valid based on specific character rules.

  - [Serializable] public struct GearTypePool
    - Public fields/properties:
      - GearType GearType: The type of gear associated with this pool.
      - List<ImplicitWeight> Entries: List of implicit weights for this gear type.

  - [Serializable] public struct ImplicitWeight
    - Public fields/properties:
      - string ImplicitId: The identifier for the implicit weight.
      - int Weight: The weight associated with the implicit ID.

# Key Behavior & Side Effects
- OnValidate method ensures:
  - No duplicate implicit IDs within the same gear type.
  - Negative weights are clamped to zero.
  - Implicit IDs are trimmed of whitespace.
  - Warnings are issued for unusual IDs and duplicates, adjusting weights accordingly.
  - Default implicit IDs are added if missing, with a weight of zero.

# Constraints & Failure Modes
- Handles null/empty lists for Pools and Entries gracefully.
- Validates IDs to ensure they only contain lowercase letters, numbers, and underscores.
- Issues warnings for invalid IDs and duplicates, modifying the state of Entries as necessary.

# Example
```csharp
// Example of creating an instance of ImplicitGearTypeConfig
ImplicitGearTypeConfig config = ScriptableObject.CreateInstance<ImplicitGearTypeConfig>();
```

# Unknowns
- None.

