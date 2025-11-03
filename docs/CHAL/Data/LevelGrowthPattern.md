# CHAL.Data.LevelGrowthPattern

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeDef.cs`._

1) Purpose
- Defines a Unity ScriptableObject ArchetypeDef representing a hero archetype with identity, display, roles, growth, and signature passive.
- Provides supporting data structures for growth configuration (ArchetypeGrowthConfig, LevelGrowthPattern) and related enums (LevelGrowthRole, PrimaryAttackArchetype).
- Exposes a CreateAssetMenu entry for creating Hero Archetype assets in the Unity editor.

```

```text
2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class ArchetypeDef : ScriptableObject
    - Fields
      - public string ArchetypeId; // e.g., "Vanguard"
      - public string DisplayName; // Localizable display name
      - public string RoleDescription; // e.g., "Tank, Frontline, Schadensglttung"
      - public PrimaryAttackArchetype primAttackType;
      - public List<HeroSlot> PreferredSlots; // e.g., ["Torso", "Head"]
      - public List<HeroAIPrio> DefaultAIPrio; // e.g., ["AttackHighestHP", "BuffAllies", "AA"]
      - public HeroAttribs Core;
      - public HeroAttribs Secondary1;
      - public HeroAttribs Secondary2;
      - public HeroAttribs Tertiary;
      - public HeroAttribs Edge;
      - public ArchetypeGrowthConfig GrowthConfig; // Reference to growth config
      - [Header("Signature Passive")]
      - public ModifierDef SignaturePassive; // ScriptableObject with ModifierData
    - Methods
      - private void OnValidate() // Performs sanity checks on GrowthConfig and GrowthPattern at edit-time
    - Notes
      - Inherits ScriptableObject
      - Asset creation via Unity menu:
        - CreateAssetMenu(fileName = "HeroArcheType", menuName = "Data/Hero Archetype")

  - public class ArchetypeGrowthConfig : Serializable
    - Fields
      - public int CoreTarget = 120;
      - public int SecondaryTarget = 100;
      - public int TertiaryTarget = 80;
      - public int EdgeTarget = 65;
      - public LevelGrowthPattern GrowthPattern = new LevelGrowthPattern();

  - public class LevelGrowthPattern : Serializable
    - Fields
      - [Tooltip("Pattern aus genau 5 Rollen, z.B. Core, Sec, Sec, Ter, Edge")]
      - public LevelGrowthRole[] growthPriority = new LevelGrowthRole[5]
        - Initialization:
          - LevelGrowthRole.Core
          - LevelGrowthRole.Secondary
          - LevelGrowthRole.Secondary
          - LevelGrowthRole.Tertiary
          - LevelGrowthRole.Edge

  - public enum LevelGrowthRole
    - Core
    - Secondary
    - Tertiary
    - Edge

  - public enum PrimaryAttackArchetype
    - Melee
    - Ranged

```

```text
3) Key Behavior & Side Effects
- OnValidate (ArchetypeDef)
  - If GrowthConfig?.GrowthPattern?.growthPriority == null
    - Logs error via DebugManager.Error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrge haben." and returns.
  - Otherwise, len = GrowthConfig.GrowthPattern.growthPriority.Length
    - If len != 5
      - Logs error via DebugManager.Error: "[ArchetypeDef] {name}: GrowthPattern muss genau 5 Eintrge haben. Aktuelle: {len}"
- Editor-only validation enforces that GrowthPattern.growthPriority has exactly 5 entries, aligning with LevelGrowthPattern default.
- Signature Passive is grouped in inspector under "Signature Passive".

```

```text
4) Constraints & Failure Modes
- GrowthConfig must be non-null with GrowthPattern containing a non-null growthPriority array with 5 entries to avoid editor errors.
- If GrowthPattern.growthPriority is null or length != 5, OnValidate logs errors but does not throw; asset validation is editor-only.
- Public API references to types not defined in this file (e.g., HeroSlot, HeroAIPrio, HeroAttribs, ModifierDef, DebugManager) are assumed to be defined elsewhere in the project.
- OnValidate is not a runtime behavior; it runs in the editor when the asset is modified or loaded.

```

```text
5) Example
```csharp
using CHAL.Data;

// Minimal example: create an instance (in editor via CreateAssetMenu) and assign basics
var archetype = ScriptableObject.CreateInstance<ArchetypeDef>();
archetype.ArchetypeId = "Vanguard";
archetype.DisplayName = "Tank";
archetype.RoleDescription = "Frontline defender";

archetype.GrowthConfig = new ArchetypeGrowthConfig();
// GrowthPattern defaults provided by LevelGrowthPattern; customize as needed
```

```

```text
6) Unknowns
- Definitions of HeroSlot, HeroAIPrio, HeroAttribs, ModifierDef, and DebugManager are not present in this file.
- Exact runtime effects of SignaturePassive/ModifierDef beyond being a ScriptableObject reference are not specified here.
- Behavior of growth values or their application to gameplay is not implemented in this file.
- Any Unity-specific behavior beyond OnValidate and CreateAssetMenu is not described here.

