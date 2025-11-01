# CHAL.Data.LevelGrowthRole

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeDef.cs`._

```text
1) Purpose
- Defines the Hero archetype data model as a Unity ScriptableObject (ArchetypeDef) and related growth/config structures.
- Encapsulates metadata for a hero archetype (IDs, display names, role description), slot preferences, AI priorities, attribute allocations, growth targets, and a signature passive modifier.
- Provides supporting serializable types: ArchetypeGrowthConfig, LevelGrowthPattern, LevelGrowthRole, PrimaryAttackArchetype; enables editor creation via CreateAssetMenu.

2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class ArchetypeDef : ScriptableObject
    - Public fields
      - public string ArchetypeId;             // "Vanguard"
      - public string DisplayName;             // Localizable display name
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
      - public ModifierDef SignaturePassive;   // ScriptableObject with ModifierData
    - Private methods
      - private void OnValidate()
        - If GrowthConfig?.GrowthPattern?.growthPriority == null
          - Debugs error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrge haben." with category "Edtior"
          - return
        - int len = GrowthConfig.GrowthPattern.growthPriority.Length;
        - If len != 5
          - Debugs error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrge haben. Aktuelle: {len}", "Edtior"

  - [Serializable] public class ArchetypeGrowthConfig
    - Public fields
      - public int CoreTarget = 120;
      - public int SecondaryTarget = 100;
      - public int TertiaryTarget = 80;
      - public int EdgeTarget = 65;
      - public LevelGrowthPattern GrowthPattern = new LevelGrowthPattern();

  - [Serializable] public class LevelGrowthPattern
    - Public fields
      - [Tooltip("Pattern aus genau 5 Rollen, z.B. Core, Sec, Sec, Ter, Edge")]
      - public LevelGrowthRole[] growthPriority = new LevelGrowthRole[5]
        - {
          LevelGrowthRole.Core,
          LevelGrowthRole.Secondary,
          LevelGrowthRole.Secondary,
          LevelGrowthRole.Tertiary,
          LevelGrowthRole.Edge
        }

  - public enum LevelGrowthRole
    - Core
    - Secondary
    - Tertiary
    - Edge

  - public enum PrimaryAttackArchetype
    - Melee
    - Ranged

3) Key Behavior & Side Effects
- Editor-time validation (OnValidate in ArchetypeDef)
  - If GrowthConfig or GrowthPattern’s growthPriority is null, logs an error and exits.
  - If GrowthPattern.growthPriority.Length != 5, logs an error with current length.
- Asset creation
  - ArchetypeDef is registered for Unity CreateAssetMenu, enabling creation via Data/Hero Archetype menu and default file name "HeroArcheType".
- Serialization
  - ArchetypeGrowthConfig and LevelGrowthPattern are serializable types used by ArchetypeDef to store growth targets and pattern.

4) Constraints & Failure Modes
- Growth validation guarded: GrowthConfig?.GrowthPattern?.growthPriority may be null; in that case an error is logged and further validation halts.
- GrowthPattern must define exactly five roles; otherwise an error is logged.
- OnValidate runs in the editor; runtime behavior not specified here.
- Public fields imply Unity serialization; runtime nullability is not enforced beyond editor checks.

5) Example
- Not derivable from this file beyond the provided defaults; no minimal runnable example is included.

6) Unknowns
- Definitions and behavior of HeroSlot, HeroAIPrio, HeroAttribs, ModifierDef, DebugManager, and the exact runtime usage of ArchetypeDef fields (e.g., how GrowthConfig interacts with gameplay) are not defined in this file.
- Exact behavior of DebugManager.Error (logging mechanics, localization, etc.) is not specified.
- Interaction with other systems (e.g., how PreferredSlots, DefaultAIPrio, or SignaturePassive are consumed) is not defined here.
```
