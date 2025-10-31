# CHAL.Data.ArchetypeGrowthConfig

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeDef.cs`._

# Purpose
- Defines the `ArchetypeDef` class as a ScriptableObject for game archetypes.
- Provides configuration for hero attributes, preferred slots, and AI priorities.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class ArchetypeDef : ScriptableObject**
    - Public fields/properties:
      - `string ArchetypeId` - Identifier for the archetype.
      - `string DisplayName` - Localizable name for the archetype.
      - `string RoleDescription` - Description of the archetype's role.
      - `PrimaryAttackArchetype primAttackType` - Type of primary attack.
      - `List<HeroSlot> PreferredSlots` - Preferred slots for the hero.
      - `List<HeroAIPrio> DefaultAIPrio` - Default AI priorities for the hero.
      - `HeroAttribs Core` - Core attributes for the hero.
      - `HeroAttribs Secondary1` - First secondary attributes for the hero.
      - `HeroAttribs Secondary2` - Second secondary attributes for the hero.
      - `HeroAttribs Tertiary` - Tertiary attributes for the hero.
      - `HeroAttribs Edge` - Edge attributes for the hero.
      - `ArchetypeGrowthConfig GrowthConfig` - Configuration for growth patterns.
      - `ModifierDef SignaturePassive` - Signature passive modifier.
    - Public methods:
      - `void OnValidate()` - Validates the growth pattern on changes.

  - **public class ArchetypeGrowthConfig**
    - Public fields/properties:
      - `int CoreTarget` - Target value for core attributes.
      - `int SecondaryTarget` - Target value for secondary attributes.
      - `int TertiaryTarget` - Target value for tertiary attributes.
      - `int EdgeTarget` - Target value for edge attributes.
      - `LevelGrowthPattern GrowthPattern` - Growth pattern configuration.

  - **public class LevelGrowthPattern**
    - Public fields/properties:
      - `LevelGrowthRole[] growthPriority` - Array defining growth priority roles.

  - **public enum LevelGrowthRole**
    - Enum values:
      - `Core`
      - `Secondary`
      - `Tertiary`
      - `Edge`

  - **public enum PrimaryAttackArchetype**
    - Enum values:
      - `Melee`
      - `Ranged`

# Key Behavior & Side Effects
- `OnValidate()` checks if `GrowthPattern.growthPriority` has exactly 5 entries and logs an error if not.

# Constraints & Failure Modes
- `GrowthConfig` must not be null and must have a `GrowthPattern` with exactly 5 entries.
- Errors are logged using `DebugManager.Error` if validation fails.

# Example
```csharp
ArchetypeDef heroArchetype = ScriptableObject.CreateInstance<ArchetypeDef>();
heroArchetype.ArchetypeId = "Vanguard";
heroArchetype.DisplayName = "Vanguard";
heroArchetype.RoleDescription = "Tank, Frontline, Schadensglttung";
heroArchetype.primAttackType = PrimaryAttackArchetype.Melee;
```

# Unknowns
- The definitions of `HeroSlot`, `HeroAIPrio`, and `HeroAttribs` are not provided in this file.
- The implementation details of `ModifierDef` and `DebugManager` are not included.

