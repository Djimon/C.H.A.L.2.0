# Assets/src/Data/Defs/ImplicitGearTypeConfig.cs

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitGearTypeConfig.cs`._

# Purpose
- Defines the configuration for implicit gear types in the game.
- Inherits from `ScriptableObject` for easy asset management.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `ImplicitGearTypeConfig` [extends `ScriptableObject`]
    - Public fields/properties:
      - `List<GearTypePool> Pools`: Collection of gear type pools.
    - Public methods:
      - `void OnValidate()`: Validates the gear type pools and entries, ensuring no duplicates and clamping weights.
      - `static bool IsValidId(string id)`: Checks if the given ID is valid based on specific character rules.

  - [Serializable] public struct `GearTypePool`
    - Public fields/properties:
      - `GearType GearType`: The type of gear.
      - `List<ImplicitWeight> Entries`: List of implicit weights associated with the gear type.

  - [Serializable] public struct `ImplicitWeight`
    - Public fields/properties:
      - `string ImplicitId`: Identifier for the implicit weight.
      - `int Weight`: Weight associated with the implicit ID.

# Key Behavior & Side Effects
- `OnValidate` method performs the following:
  - Avoids duplicates for each gear type.
  - Clamps negative weights to zero.
  - Trims IDs and checks for validity, issuing warnings for unusual IDs.
  - Adds missing default IDs with a weight of zero if they are not present.

# Constraints & Failure Modes
- Handles null or empty lists for `Pools` and `Entries`.
- Ensures that `ImplicitId` follows the format of lower snake case (a-z, 0-9, _).
- Issues warnings for duplicate IDs and unusual ID formats.

# Example
```csharp
var config = ScriptableObject.CreateInstance<ImplicitGearTypeConfig>();
config.Pools[0].Entries.Add(new ImplicitWeight { ImplicitId = "new_id", Weight = 10 });
```

# Unknowns
- None.

