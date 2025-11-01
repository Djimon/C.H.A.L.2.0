# CHAL.Data.ArchetypeGrowthConfig

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeDef.cs`._

```text
1) Purpose
- Defines Unity ScriptableObject-based data structures for hero archetypes (ArchetypeDef) and their growth/configuration.
- Provides serializable support for growth patterns and per-archetype attributes (ArchetypeGrowthConfig, LevelGrowthPattern, LevelGrowthRole).
- Exposes enums for attack type and growth roles to drive gameplay configuration.

2) Public API
- Namespace/module
  - CHAL.Data

- Types

  - public class ArchetypeDef : ScriptableObject
    - Public fields
      - public string ArchetypeId;             // "Vanguard"
      - public string DisplayName;             // Lokalisierbarer Name
      - public string RoleDescription;         // "Tank, Frontline, Schadensglttung"
      - public PrimaryAttackArchetype primAttackType;
      - public List<HeroSlot> PreferredSlots;  // ["Torso", "Head"]
      - public List<HeroAIPrio> DefaultAIPrio; // ["AttackHighestHP", "BuffAllies", "AA"]
      - public HeroAttribs Core;
      - public HeroAttribs Secondary1;
      - public HeroAttribs Secondary2;
      - public HeroAttribs Tertiary;
      - public HeroAttribs Edge;
      - public ArchetypeGrowthConfig GrowthConfig;
      - [Header("Signature Passive")]
      - public ModifierDef SignaturePassive;   // ScriptableObject mit ModifierData
    - Private/Unity lifecycle
      - private void OnValidate()
        - Behavior: Validates GrowthConfig.GrowthPattern.growthPriority is non-null and has exactly 5 entries; logs errors otherwise.
        - Uses DebugManager.Error to report issues.
        - Returns early if GrowthConfig or GrowthPattern growthPriority is null.

  - public class ArchetypeGrowthConfig
    - Public fields
      - public int CoreTarget = 120;
      - public int SecondaryTarget = 100;
      - public int TertiaryTarget = 80;
      - public int EdgeTarget = 65;
      - public LevelGrowthPattern GrowthPattern;

  - public class LevelGrowthPattern
    - Public fields
      - [Tooltip("Pattern aus genau 5 Rollen, z.B. Core, Sec, Sec, Ter, Edge")]
      - public LevelGrowthRole[] growthPriority = new LevelGrowthRole[5]
        {
          LevelGrowthRole.Core,
          LevelGrowthRole.Secondary,
          LevelGrowthRole.Secondary,
          LevelGrowthRole.Tertiary,
          LevelGrowthRole.Edge
        };

  - public enum LevelGrowthRole
    - Members
      - Core
      - Secondary
      - Tertiary
      - Edge

  - public enum PrimaryAttackArchetype
    - Members
      - Melee
      - Ranged

```

3) Key Behavior & Side Effects
- ArchetypeDef.OnValidate
  - If GrowthConfig?.GrowthPattern?.growthPriority == null
    - Logs error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrage haben." with tag "Edtior"
    - Returns immediately.
  - Otherwise, checks GrowthConfig.GrowthPattern.growthPriority.Length
    - If length != 5, logs error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrge haben. Aktuelle: {len}"
- Growth configuration is serialized data, used to drive growth targets and pattern per archetype.
- SignaturePassive is a ScriptableObject-based modifier; its presence configures a per-archetype passive.

4) Constraints & Failure Modes
- OnValidate guards:
  - GrowthConfig and GrowthPattern must be non-null with a non-null growthPriority array.
  - growthPriority must contain exactly 5 entries; otherwise a validation error is logged.
- Logging is performed via DebugManager.Error (no exception is thrown in OnValidate).
- Default GrowthPattern is defined to provide a 5-element pattern (Core, Secondary, Secondary, Tertiary, Edge).
- Serialized fields imply editor-time configuration; runtime behavior depends on Unity serialization.

5) Example
- Not derivable from this file alone (no usage example provided).

6) Unknowns
- Definitions and behavior of external types: HeroSlot, HeroAIPrio, HeroAttribs, ModifierDef, DebugManager.
- How ArchetypeDef instances are created/consumed at runtime beyond OnValidate checks.
- Any additional editor tooling or handling beyond the provided OnValidate method.
