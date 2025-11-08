# CHAL.Data.GearTypePool

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
      - GearType GearType: The type of gear associated with the pool.
      - List<ImplicitWeight> Entries: List of implicit weights for the gear type.

  - [Serializable] public struct ImplicitWeight
    - Public fields/properties:
      - string ImplicitId: Identifier for the implicit weight.
      - int Weight: Weight associated with the implicit ID.

# Key Behavior & Side Effects
- OnValidate checks for duplicate implicit IDs within each gear type, clamps negative weights to zero, and trims IDs.
- Issues warnings for unusual IDs and duplicates, setting their weights to zero if duplicates are found.
- Adds default implicit IDs with a weight of zero if they are missing from the entries.

# Constraints & Failure Modes
- Handles null or empty entries in Pools and Entries gracefully.
- Validates IDs to ensure they only contain lowercase letters, numbers, and underscores.
- Performance considerations are not explicitly mentioned.

# Example
```csharp
// Example of creating an instance of ImplicitGearTypeConfig
ImplicitGearTypeConfig config = ScriptableObject.CreateInstance<ImplicitGearTypeConfig>();
```

# Unknowns
- None.

