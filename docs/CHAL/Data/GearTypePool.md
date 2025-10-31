# CHAL.Data.GearTypePool

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitGearTypeConfig.cs`._

# Purpose
- Defines the `ImplicitGearTypeConfig` class as a ScriptableObject for managing gear type pools and their implicit weights.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class** `ImplicitGearTypeConfig` [extends `ScriptableObject`]
    - **public List<GearTypePool>** `Pools` - List of gear type pools.
    - **private void** `OnValidate()` - Validates and adjusts the gear type pools and their entries.
    - **private static bool** `IsValidId(string id)` - Checks if the given ID is valid based on specific character rules.

  - **[Serializable] public struct** `GearTypePool`
    - **public GearType** `GearType` - The type of gear.
    - **public List<ImplicitWeight>** `Entries` - List of implicit weights associated with the gear type.

  - **[Serializable] public struct** `ImplicitWeight`
    - **public string** `ImplicitId` - Identifier for the implicit weight.
    - **public int** `Weight` - Weight associated with the implicit ID.

# Key Behavior & Side Effects
- `OnValidate()`:
  - Ensures no duplicate IDs exist within the same gear type.
  - Clamps negative weights to zero.
  - Trims implicit IDs and logs warnings for unusual IDs.
  - Adds default implicit IDs with a weight of zero if they are missing.

# Constraints & Failure Modes
- Handles null/empty lists for `Pools` and `Entries`.
- Logs warnings for invalid IDs and duplicates but does not throw exceptions.
- Assumes `GearType` and `ImplicitWeight` structures are properly defined elsewhere.

# Example
```csharp
var config = ScriptableObject.CreateInstance<ImplicitGearTypeConfig>();
config.Pools.Add(new GearTypePool { GearType = GearType.Head, Entries = new List<ImplicitWeight>() });
```

# Unknowns
- The definition and values of `GearType` are not provided in this file.

