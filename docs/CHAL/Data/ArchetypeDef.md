# CHAL.Data.ArchetypeDef

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeDef.cs`._

# Purpose
- Defines an archetype for characters in the game, including attributes and settings related to their roles and abilities.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class ArchetypeDef : ScriptableObject**
    - Public fields/properties:
      - `string ArchetypeId` - Identifier for the archetype (e.g., "Vanguard").
      - `string DisplayName` - Localizable name for the archetype.
      - `string RoleDescription` - Description of the archetype's role (e.g., "Tank, Frontline, Schadensglättung").
      - `PrimaryAttackArchetype primAttackType` - Type of primary attack (Melee or Ranged).
      - `List<HeroSlot> PreferredSlots` - List of preferred slots (e.g., ["Torso", "Head"]).
      - `List<HeroAIPrio> DefaultAIPrio` - List of default AI priorities (e.g., ["AttackHighestHP", "BuffAllies", "AA"]).
      - `HeroAttribs Core` - Core attributes for the archetype.
      - `HeroAttribs Secondary1` - First secondary attributes for the archetype.
      - `HeroAttribs Secondary2` - Second secondary attributes for the archetype.
      - `HeroAttribs Tertiary` - Tertiary attributes for the archetype.
      - `HeroAttribs Edge` - Edge attributes for the archetype.
      - `ArchetypeGrowthConfig GrowthConfig` - Configuration for growth patterns.
      - `ModifierDef SignaturePassive` - Signature passive modifier data.
    - Public methods:
      - `void OnValidate()` - Validates the growth pattern on changes; logs errors if the growth pattern does not have exactly 5 entries.

  - **public class ArchetypeGrowthConfig**
    - Public fields/properties:
      - `int CoreTarget` - Target value for core attributes.
      - `int SecondaryTarget` - Target value for secondary attributes.
      - `int TertiaryTarget` - Target value for tertiary attributes.
      - `int EdgeTarget` - Target value for edge attributes.
      - `LevelGrowthPattern GrowthPattern` - Defines the growth pattern.

  - **public class LevelGrowthPattern**
    - Public fields/properties:
      - `LevelGrowthRole[] growthPriority` - Array defining the priority of roles for growth (exactly 5 entries).

  - **public enum LevelGrowthRole**
    - Values: `Core`, `Secondary`, `Tertiary`, `Edge`.

  - **public enum PrimaryAttackArchetype**
    - Values: `Melee`, `Ranged`.

# Key Behavior & Side Effects
- The `OnValidate` method checks the `GrowthPattern` for exactly 5 entries and logs an error if the condition is not met.

# Constraints & Failure Modes
- The `GrowthConfig` must have a `GrowthPattern` with exactly 5 entries; otherwise, an error is logged.
- The `OnValidate` method is called when the object is modified in the Unity Editor.

# Example
```csharp
ArchetypeDef myArchetype = ScriptableObject.CreateInstance<ArchetypeDef>();
myArchetype.ArchetypeId = "Vanguard";
myArchetype.DisplayName = "Vanguard";
myArchetype.RoleDescription = "Tank, Frontline, Schadensglättung";
myArchetype.primAttackType = PrimaryAttackArchetype.Melee;
myArchetype.PreferredSlots = new List<HeroSlot> { HeroSlot.Torso, HeroSlot.Head };
myArchetype.DefaultAIPrio = new List<HeroAIPrio> { HeroAIPrio.AttackHighestHP, HeroAIPrio.BuffAllies, HeroAIPrio.AA };
```

# Unknowns
- The definitions and structures of `HeroSlot`, `HeroAIPrio`, and `HeroAttribs` are not provided in this file.
- The implementation details of `ModifierDef` and `DebugManager` are not included in this file.

