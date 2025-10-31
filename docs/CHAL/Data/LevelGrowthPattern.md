# CHAL.Data.LevelGrowthPattern

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeDef.cs`._

# Purpose
- Defines the `ArchetypeDef` class as a ScriptableObject for hero archetypes in a game.
- Provides configuration for hero attributes, roles, and growth patterns.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class ArchetypeDef : ScriptableObject**
    - Public fields/properties:
      - `string ArchetypeId` - Identifier for the archetype.
      - `string DisplayName` - Localizable name for the archetype.
      - `string RoleDescription` - Description of the archetype's role.
      - `PrimaryAttackArchetype primAttackType` - Type of primary attack (Melee/Ranged).
      - `List<HeroSlot> PreferredSlots` - Preferred slots for the hero.
      - `List<HeroAIPrio> DefaultAIPrio` - Default AI priorities for the hero.
      - `HeroAttribs Core` - Core attributes of the hero.
      - `HeroAttribs Secondary1` - First secondary attributes of the hero.
      - `HeroAttribs Secondary2` - Second secondary attributes of the hero.
      - `HeroAttribs Tertiary` - Tertiary attributes of the hero.
      - `HeroAttribs Edge` - Edge attributes of the hero.
      - `ArchetypeGrowthConfig GrowthConfig` - Configuration for growth patterns.
      - `ModifierDef SignaturePassive` - Signature passive modifier data.
    - Public methods:
      - `private void OnValidate()` - Validates the growth pattern on changes.

  - **public class ArchetypeGrowthConfig**
    - Public fields/properties:
      - `int CoreTarget` - Target value for core attributes.
      - `int SecondaryTarget` - Target value for secondary attributes.
      - `int TertiaryTarget` - Target value for tertiary attributes.
      - `int EdgeTarget` - Target value for edge attributes.
      - `LevelGrowthPattern GrowthPattern` - Growth pattern configuration.

  - **public class LevelGrowthPattern**
    - Public fields/properties:
      - `LevelGrowthRole[] growthPriority` - Array defining the growth priority roles.

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
- The `OnValidate` method checks if the `GrowthPattern` has exactly 5 entries and logs an error if not.

# Constraints & Failure Modes
- The `OnValidate` method guards against invalid growth patterns by checking the length of `growthPriority`.
- If `GrowthConfig` is null, accessing `GrowthPattern` will not throw an exception due to the null-conditional operator.

# Example
```csharp
ArchetypeDef heroArchetype = ScriptableObject.CreateInstance<ArchetypeDef>();
heroArchetype.ArchetypeId = "Vanguard";
heroArchetype.DisplayName = "Vanguard";
heroArchetype.RoleDescription = "Tank, Frontline, Schadensglttung";
heroArchetype.primAttackType = PrimaryAttackArchetype.Melee;
```

# Unknowns
- The definitions and structures of `HeroSlot`, `HeroAIPrio`, and `HeroAttribs` are not provided in this file.
- The implementation details of `ModifierDef` and `DebugManager` are not included.

