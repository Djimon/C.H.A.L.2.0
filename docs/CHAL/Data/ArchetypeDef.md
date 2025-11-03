# CHAL.Data.ArchetypeDef

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeDef.cs`._

1) Purpose
- Defines ArchetypeDef as a Unity ScriptableObject that holds hero archetype data (identity, display/description, attack type, preferred slots, AI priorities, core/secondary/tertiary/edge attributes, growth config, and a signature passive).
- Defines ArchetypeGrowthConfig and LevelGrowthPattern (with growthPriority) as serializable supporting data for growth targets and order.
- Defines LevelGrowthRole and PrimaryAttackArchetype enums; LevelGrowthPattern defaults to a 5-entry order Core, Secondary, Secondary, Tertiary, Edge.
- Adds [CreateAssetMenu] attribute to ArchetypeDef for asset creation in Unity.

2) Public API
- Namespace/Module: CHAL.Data
- Types
  - public class ArchetypeDef : ScriptableObject
    - Public fields
      - string ArchetypeId
      - string DisplayName
      - string RoleDescription
      - PrimaryAttackArchetype primAttackType
      - List<HeroSlot> PreferredSlots
      - List<HeroAIPrio> DefaultAIPrio
      - HeroAttribs Core
      - HeroAttribs Secondary1
      - HeroAttribs Secondary2
      - HeroAttribs Tertiary
      - HeroAttribs Edge
      - ArchetypeGrowthConfig GrowthConfig
      - ModifierDef SignaturePassive
    - Public methods: None
  - public class ArchetypeGrowthConfig
    - Public fields
      - int CoreTarget
      - int SecondaryTarget
      - int TertiaryTarget
      - int EdgeTarget
      - LevelGrowthPattern GrowthPattern
  - public class LevelGrowthPattern
    - Public fields
      - LevelGrowthRole[] growthPriority
  - public enum LevelGrowthRole
    - Core
    - Secondary
    - Tertiary
    - Edge
  - public enum PrimaryAttackArchetype
    - Melee
    - Ranged

3) Key Behavior & Side Effects
- ArchetypeDef.OnValidate()
  - If GrowthConfig?.GrowthPattern?.growthPriority == null:
    - Logs error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrge haben." with source "Edtior" and returns early.
  - Else, checks length:
    - If GrowthConfig.GrowthPattern.growthPriority.Length != 5:
      - Logs error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrge haben. Aktuelle: {len}" with source "Edtior".

4) Constraints & Failure Modes
- OnValidate guards GrowthConfig and GrowthPattern for non-null GrowthPriority; otherwise errors.
- GrowthPattern.growthPriority must have exactly 5 entries; otherwise an error is logged.
- OnValidate uses DebugManager.Error to report issues; exact messages include a misspelled "Eintrge" and "Edtior".

5) Example
- Not derivable from file (no usage example provided).

6) Unknowns
- Definitions and details of HeroSlot, HeroAIPrio, HeroAttribs, ModifierDef, DebugManager are external to this file.
- How ArchetypeDef is consumed at runtime beyond its surface fields.
- Behavior of GrowthConfig.GrowthPattern in other code paths.

