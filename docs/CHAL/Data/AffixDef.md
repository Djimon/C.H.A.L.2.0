# Assets/src/Data/Defs/AffixDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/AffixDef.cs`._

# Purpose
- Defines static designer data for a single affix modifier that can roll on gear.
- Affixes have no prefix/suffix distinction in this project.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class AffixDef** [extends ScriptableObject]
    - **public string AffixId**: Stable identifier for the affix.
    - **public AffixCategory Category**: Categorization of the affix.
    - **public AffixFamilyBitMask FamilyMembership**: Membership in affix families.
    - **public GearType[] AllowedGearTypes**: Gear types this affix can roll on.
    - **public GearStatTarget Target**: Stat modified by this affix.
    - **public GearValueKind ValueKind**: How the value applies (flat or percent).
    - **public TieredRollRange Ranges**: Roll ranges by base tier.
    - **public float customWeight**: Weight affecting the chance of rolling this affix.
    - **public bool Allows(GearType gearType)**: Checks if the affix can roll on the specified gear type; returns true if allowed.
    - **private void OnValidate()**: Validates fields and logs warnings for invalid states.
    - **public IEnumerable<AffixFamily> EnumerateFamilies()**: Enumerates the families this affix belongs to.
  
  - **[Serializable] public enum AffixCategory**
    - None = 0
    - Attribute
    - Crit
    - Damage
    - Defense
    - Life
    - Skill
    - Special

  - **[Flags] public enum AffixFamilyBitMask**
    - None = 0
    - Core = 1 << 0
    - Defensive = 1 << 1
    - Synergy = 1 << 2
    - Utility = 1 << 3

  - **[Serializable] public enum AffixFamily**
    - Core = 0
    - Defensive = 1
    - Synergy = 2
    - Utility = 3

# Key Behavior & Side Effects
- `Allows(GearType gearType)`: Returns true if the affix can roll on the specified gear type, allowing for flexibility in gear compatibility.
- `OnValidate()`: Automatically trims `AffixId`, ensures `customWeight` is non-negative, and logs warnings for invalid `AffixId`, missing `FamilyMembership`, or `Category` being None.

# Constraints & Failure Modes
- `AllowedGearTypes` can be null or empty, defaulting to allow all gear types.
- `customWeight` is clamped to a minimum of 0.
- Warnings are logged for invalid `AffixId` formats and missing family memberships or categories.

# Example
```csharp
AffixDef affix = ScriptableObject.CreateInstance<AffixDef>();
affix.AffixId = "life_flat";
affix.Category = AffixCategory.Life;
affix.AllowedGearTypes = new GearType[] { GearType.Armor, GearType.Weapon };
```

# Unknowns
- The definitions and behaviors of `GearType`, `GearStatTarget`, `GearValueKind`, and `TieredRollRange` are not provided in this file.

