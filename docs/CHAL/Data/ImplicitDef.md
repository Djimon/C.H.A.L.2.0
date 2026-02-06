# Assets/src/Data/Defs/ImplicitDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitDef.cs`._

# Purpose
- Defines a data structure for a single implicit modifier that can roll on gear.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class** `ImplicitDef` [extends `ScriptableObject`]
    - **public string** `Id` - Stable identifier for the implicit modifier.
    - **public ImplicitRole** `Role` - Role categorization of the implicit.
    - **public ImplicitPoolBitMask** `PoolMembership` - Membership in implicit pools.
    - **public GearType[]** `AllowedGearTypes` - Gear types this implicit can roll on.
    - **public GearStatTarget** `Target` - Stat modified by this implicit.
    - **public GearValueKind** `ValueKind` - How the value applies (flat or percent).
    - **public TieredRollRange** `Ranges` - Roll ranges by base tier.
    - **public float** `customWeight` - Weight affecting the chance of rolling this implicit.
    - **private void** `OnValidate()` - Validates and normalizes fields when the asset is modified.
    - **public IEnumerable<ImplicitPool>** `EnumeratePools()` - Enumerates the implicit pools based on membership.
    - **public bool** `Allows(GearType gearType)` - Checks if the implicit is allowed on the specified gear type.

  - **[Serializable] public enum** `ImplicitPool`
    - Values: `Melee`, `Ranged`, `Caster`, `Neutral`

  - **[Flags] public enum** `ImplicitPoolBitMask`
    - Values: `None`, `Melee`, `Ranged`, `Caster`, `Neutral`

  - **[Serializable] public enum** `ImplicitRole`
    - Values: `Defense`, `Offense`, `Utility`

  - **[Serializable] public enum** `GearValueKind`
    - Values: `Flat`, `Percent`

  - **[Serializable] public enum** `GearStatTarget`
    - Values: Various stats including `Armor`, `MaxLife`, `Damage`, etc.

  - **[Serializable] public struct** `RollRange`
    - **public float** `Min` - Minimum value of the roll range.
    - **public float** `Max` - Maximum value of the roll range.
    - **public RollRange(float min, float max)** - Constructor for roll range.
    - **public RollRange Normalize()** - Normalizes the roll range.

  - **[Serializable] public struct** `TieredRollRange`
    - **public RollRange** `Tier1` - Roll range for tier 1.
    - **public RollRange** `Tier2` - Roll range for tier 2.
    - **public RollRange** `Tier3` - Roll range for tier 3.
    - **public TieredRollRange Normalize()** - Normalizes all tiered roll ranges.

# Key Behavior & Side Effects
- `OnValidate()` ensures `Id` is trimmed and validated, and `customWeight` is non-negative.
- `EnumeratePools()` yields implicit pools based on the `PoolMembership` bitmask.
- `Allows(GearType gearType)` checks if the implicit is allowed on the specified gear type, returning true if no restrictions are set.

# Constraints & Failure Modes
- `AllowedGearTypes` can be null or empty, defaulting to allow all gear types.
- `customWeight` is clamped to a minimum of 0.
- `Id` must follow the lower_snake_case format; otherwise, a warning is logged.

# Example
```csharp
var implicitDef = ScriptableObject.CreateInstance<ImplicitDef>();
implicitDef.Id = "armor_pct";
implicitDef.Role = ImplicitRole.Defense;
implicitDef.AllowedGearTypes = new GearType[] { GearType.Armor, GearType.Helmet };
```

# Unknowns
- No information on the `GearType` type or its values.
- No details on the `DebugManager` class or its `Warning` method.

