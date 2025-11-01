# CHAL.Data.PrimaryAttackArchetype

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeDef.cs`._

```text
1) Purpose
- Defines ArchetypeDef as a Unity ScriptableObject for hero archetypes (identifiers, display, role, stats, growth, and signature passive).
- Provides serializable support types: ArchetypeGrowthConfig, LevelGrowthPattern, LevelGrowthRole, PrimaryAttackArchetype.
- Exposes editor-time validation for growth configuration via OnValidate; uses DebugManager for errors.

```

2) Public API
- Namespace/module
  - CHAL.Data

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
    - Notes
      - [CreateAssetMenu(fileName = "HeroArcheType", menuName = "Data/Hero Archetype")] attribute present
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

Notes
- ArchetypeDef exposes several complex types (HeroSlot, HeroAIPrio, HeroAttribs, ModifierDef) assumed to be defined elsewhere.
- OnValidate is a Unity editor-time lifecycle method (not public); not part of the public API surface.

3) Key Behavior & Side Effects
- OnValidate behavior
  - If GrowthConfig?.GrowthPattern?.growthPriority is null:
    - Logs error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrge haben." with category "Edtior"
    - Returns early
  - Otherwise:
    - Computes len = GrowthConfig.GrowthPattern.growthPriority.Length
    - If len != 5:
      - Logs error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrge haben. Aktuelle: {len}" with category "Edtior"
- Asset lifecycle
  - ArchetypeDef is a ScriptableObject created via Unity's CreateAssetMenu; assets are expected to serialize the public fields above.
- Growth configuration
  - GrowthPattern defaults to a 5-entry array: Core, Secondary, Secondary, Tertiary, Edge

4) Constraints & Failure Modes
- GrowthConfig can be null; OnValidate handles null safely by emitting an error.
- GrowthPattern.growthPriority must be exactly 5 entries; otherwise, an error is logged.
- Editor-only validation path; runtime behavior depends on how and when OnValidate is invoked in the build.

5) Example
- Not provided (no clearly derivable minimal code example within this file).

6) Unknowns
- Definitions and runtime behavior of HeroSlot, HeroAIPrio, HeroAttribs, ModifierDef, and DebugManager are not present in this file.
- Exact usage semantics of GrowthConfig (beyond the target fields) and how GrowthPattern influences gameplay are not specified here.
- Behavior of external tooling or editor scripts interacting with these types is not defined in this file.
