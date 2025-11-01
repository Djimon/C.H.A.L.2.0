# CHAL.Data.AttackDef

_Automatically generated/updated from `Assets/src/Data/Defs/AttackDef.cs`._

```text
1) Purpose
- Data container for attack definitions (AttackDef) as a ScriptableObject in CHAL.Data.
- Defines a single damage entry structure (DamageEntry) used within AttackDef.
- Class is marked obsolete; intended to be replaced by central skillData.

2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class AttackDef : ScriptableObject
    - [System.Obsolete("Deprecated.Please use the central skillData", false)]
    - Public fields:
      - public string attackId;                   // identity key
      - public string displayNameKey;             // localization/display key
      - public List<DamageEntry> damages = new(); // damage entries (type + multiplier)
      - public float cooldown = 0f;               // autoattack cooldown
      - public string[] tags;                     // meta tags (e.g., "aoe", "projectile", "buff")
      - public string animationType;              // presentation: e.g., "MeleeSwing", "Cast", "Shoot"
      - public GameObject vfxPrefab;              // visual effect prefab for this attack
  - public struct DamageEntry
    - public DamageType DmgType;      // damage type (defined elsewhere)
    - public float DmgMultiplier;       // e.g., 1.5f means 150% of base damage
    - public DamageEntry(DamageType type, float multiplier) // constructor
      - sets DmgType = type
      - sets DmgMultiplier = multiplier

3) Key Behavior & Side Effects
- No runtime methods defined; this file only declares data structures.
- AttackDef is deprecated; usage triggers compiler warnings due to Obsolete attribute.
- Damages is a list initialized to an empty list; other fields rely on Unity serialization.
- DamageEntry instances pair a DamageType with a multiplier for each damage entry.

4) Constraints & Failure Modes
- AttackDef marked obsolete; new usage should migrate to central skillData.
- No validation logic present; fields are public and can be null or zero.
- DamageEntry references DamageType defined elsewhere; not declared in this file.
- Damages uses a default initialized list; ensure Unity serialization handles nulls if edited outside editor.

5) Example
- Not provided in this file as executable code; usage implied by public fields and Unity ScriptableObject asset creation.

6) Unknowns
- Definition and range of DamageType (defined elsewhere).
- How AttackDef assets are created, loaded, or consumed at runtime.
- Any runtime validation or behavior beyond data storage.
```
