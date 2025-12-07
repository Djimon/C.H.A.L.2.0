# Assets/src/Data/Defs/ArchetypeDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeDef.cs`._

# Purpose
- Defines archetype definitions for characters in the game, including attributes and settings related to their roles and abilities.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `ArchetypeDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string ArchetypeId` - Identifier for the archetype.
      - `string DisplayName` - Localizable name for the archetype.
      - `string RoleDescription` - Description of the archetype's role.
      - `PrimaryAttackArchetype primAttackType` - Type of primary attack (Melee or Ranged).
      - `List<HeroSlot> PreferredSlots` - Preferred slots for the archetype (e.g., "Torso", "Head").
      - `List<HeroAIPrio> DefaultAIPrio` - Default AI priorities for the archetype (e.g., "AttackHighestHP").
      - `HeroAttribs Core` - Core attributes for the archetype.
      - `HeroAttribs Secondary1` - First secondary attributes for the archetype.
      - `HeroAttribs Secondary2` - Second secondary attributes for the archetype.
      - `HeroAttribs Tertiary` - Tertiary attributes for the archetype.
      - `HeroAttribs Edge` - Edge attributes for the archetype.
      - `ArchetypeGrowthConfig GrowthConfig` - Configuration for growth patterns.
      - `ModifierDef SignaturePassive` - Signature passive modifier for the archetype.
    - Public methods:
      - `void OnValidate()` - Validates the growth pattern on changes; logs errors if conditions are not met.

  - public class `ArchetypeGrowthConfig`
    - Public fields/properties:
      - `int CoreTarget` - Target value for core attributes.
      - `int SecondaryTarget` - Target value for secondary attributes.
      - `int TertiaryTarget` - Target value for tertiary attributes.
      - `int EdgeTarget` - Target value for edge attributes.
      - `LevelGrowthPattern GrowthPattern` - Pattern for level growth.

  - public class `LevelGrowthPattern`
    - Public fields/properties:
      - `LevelGrowthRole[] growthPriority` - Array defining the priority of roles for growth.

  - public enum `LevelGrowthRole`
    - Values:
      - `Core`
      - `Secondary`
      - `Tertiary`
      - `Edge`

  - public enum `PrimaryAttackArchetype`
    - Values:
      - `Melee`
      - `Ranged`

# Key Behavior & Side Effects
- The `OnValidate` method checks if the `GrowthPattern` is null or if it does not contain exactly 5 entries, logging an error if either condition is met.

# Constraints & Failure Modes
- The `OnValidate` method ensures that `GrowthPattern.growthPriority` is not null and contains exactly 5 entries.
- If the conditions are not met, an error message is logged via `DebugManager`.

# Example
```csharp
ArchetypeDef myArchetype = ScriptableObject.CreateInstance<ArchetypeDef>();
myArchetype.ArchetypeId = "Vanguard";
myArchetype.DisplayName = "Vanguard";
myArchetype.RoleDescription = "Tank, Frontline, Schadensglättung";
myArchetype.primAttackType = PrimaryAttackArchetype.Melee;
```

# Unknowns
- None.
