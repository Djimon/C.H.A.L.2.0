# CHAL.Data.ImplicitWeight

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitGearTypeConfig.cs`._

# Purpose
- Defines the `ImplicitGearTypeConfig` class as a ScriptableObject for managing gear type pools and their implicit weights.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class** `ImplicitGearTypeConfig` [extends `ScriptableObject`]
    - **public List<GearTypePool>** `Pools` - List of gear type pools with implicit weights.
    - **private void** `OnValidate()` - Validates and adjusts the gear type pools and their entries.
    - **private static bool** `IsValidId(string id)` - Checks if the given ID is valid based on specific character rules.

  - **[Serializable] public struct** `GearTypePool`
    - **public GearType** `GearType` - The type of gear (e.g., Head, Chest).
    - **public List<ImplicitWeight>** `Entries` - List of implicit weights associated with the gear type.

  - **[Serializable] public struct** `ImplicitWeight`
    - **public string** `ImplicitId` - Identifier for the implicit weight.
    - **public int** `Weight` - Weight associated with the implicit ID.

# Key Behavior & Side Effects
- `OnValidate()` performs the following:
  - Checks for duplicates within each gear type and clamps negative weights to zero.
  - Trims implicit IDs and logs warnings for unusual IDs.
  - Adds default implicit IDs with a weight of zero if they are missing.

# Constraints & Failure Modes
- Handles null or empty lists for `Pools` and `Entries`.
- Ensures that implicit IDs conform to the format: lower_snake_case (a-z, 0-9, _).
- Uses `Debug.LogWarning` to report issues with IDs and duplicates.

# Example
```csharp
var config = ScriptableObject.CreateInstance<ImplicitGearTypeConfig>();
config.Pools.Add(new GearTypePool { GearType = GearType.Head, Entries = new List<ImplicitWeight>() });
```

# Unknowns
- The definition of `GearType` is not provided in this file.

