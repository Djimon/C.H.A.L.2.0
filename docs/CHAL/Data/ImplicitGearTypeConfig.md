# CHAL.Data.ImplicitGearTypeConfig

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitGearTypeConfig.cs`._

# Purpose
- Defines the `ImplicitGearTypeConfig` class as a ScriptableObject for managing implicit gear types and their weights.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class** `ImplicitGearTypeConfig` [extends `ScriptableObject`]
    - **public List<GearTypePool>** `Pools` - List of gear type pools with implicit weights.
    - **private void** `OnValidate()` - Validates and adjusts entries in `Pools` on changes.
    - **private static bool** `IsValidId(string id)` - Checks if the given ID is valid.

  - **[Serializable] public struct** `GearTypePool`
    - **public GearType** `GearType` - Type of gear (e.g., Head, Chest).
    - **public List<ImplicitWeight>** `Entries` - List of implicit weights associated with the gear type.

  - **[Serializable] public struct** `ImplicitWeight`
    - **public string** `ImplicitId` - Identifier for the implicit weight.
    - **public int** `Weight` - Weight associated with the implicit ID.

# Key Behavior & Side Effects
- `OnValidate()` performs the following:
  - Prevents duplicate IDs per gear type.
  - Clamps negative weights to zero.
  - Trims IDs and logs warnings for unusual IDs.
  - Adds missing default IDs with a weight of zero.

# Constraints & Failure Modes
- Handles null or empty `Pools` gracefully.
- Validates IDs to ensure they contain only lowercase letters, numbers, and underscores.
- Logs warnings for duplicate IDs and unusual ID formats.

# Example
```csharp
var config = ScriptableObject.CreateInstance<ImplicitGearTypeConfig>();
config.Pools.Add(new GearTypePool { GearType = GearType.Head, Entries = new List<ImplicitWeight>() });
```

# Unknowns
- Specifics of the `GearType` enumeration are not defined in this file.

