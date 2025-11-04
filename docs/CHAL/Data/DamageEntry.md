# CHAL.Data.DamageEntry

_Automatically generated/updated from `Assets/src/Data/Defs/AttackDef.cs`._

1) Purpose
- Defines a data container for an attack/skill: AttackDef as a ScriptableObject with identity, damage, cooldown, meta, and presentation metadata.
- Defines a serializable data structure for a single damage entry: DamageEntry with a DamageType and multiplier.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public class AttackDef : ScriptableObject
    - Public fields (Unity inspector exposed):
      - [Header("Identity")] string attackId
        - Identity of the attack
      - string displayNameKey
        - Display name key for localization
      - [Header("Damage")] List<DamageEntry> damages = new()
        - List of damage entries applied by this attack
      - [Header("Cooldown")] float cooldown = 0f
        - Autoattack cooldown in seconds
      - [Header("Meta")] string[] tags
        - Tags describing the attack (e.g., "aoe", "projectile", "buff")
      - [Header("Presentation")] string animationType
        - Animation type to play (e.g., "MeleeSwing", "Cast", "Shoot")
      - GameObject vfxPrefab
        - Visual effect prefab for the attack
  - public struct DamageEntry : System.Serializable
    - Public fields:
      - public DamageType DmgType
        - Type of damage (e.g., Fire, Poison, Phys)
      - public float DmgMultiplier
        - Multiplier to apply to base damage (e.g., 1.5f)
    - Public constructor:
      - public DamageEntry(DamageType type, float multiplier)
        - Initializes DmgType and DmgMultiplier

3) Key Behavior & Side Effects
- Data-only surface: AttackDef holds configuration for an attack; no runtime logic defined here.
- Damage list is initialized to an empty list by default.
- AttackDef is marked obsolete:
  - [System.Obsolete("Deprecated.Please use the central skillData", false)]
  - Indicates deprecation with a non-fatal warning.
- DamageEntry provides a small constructor for convenient initialization.
- Inspector organization via [Header] attributes to group fields in Unity Editor.

4) Constraints & Failure Modes
- AttackDef is deprecated; usage should migrate to central skillData (per Obsolete attribute).
- Public fields are serialized by Unity; ensure they are assigned to avoid null references at runtime (e.g., attackId, damages, vfxPrefab).
- DamageEntry fields rely on DamageType being defined elsewhere; this file does not define DamageType.
- No threading/async guarantees or performance hints are explicit in this file.

5) Example
```csharp
// Example usage (Unity runtime)
var atk = UnityEngine.ScriptableObject.CreateInstance<CHAL.Data.AttackDef>();
atk.attackId = "fireball_poison";
atk.displayNameKey = "attack.fireball_poison";
atk.damages = new List<CHAL.Data.DamageEntry>
{
    new CHAL.Data.DamageEntry(DamageType.Fire, 2.0f),
    new CHAL.Data.DamageEntry(DamageType.Poison, 1.0f)
};
atk.cooldown = 3.0f;
atk.tags = new[] { "projectile", "aoe" };
atk.animationType = "Cast";
atk.vfxPrefab = null; // assign a prefab as needed
```

6) Unknowns
- Definition and values of DamageType (enum) are not present in this file.
- How AttackDef instances are created/loaded, and how they are consumed by gameplay systems (central skillData is referenced but not shown here).
- Any runtime logic that applies damages, cooldowns, or visuals is outside this file.
- Whether AttackDef assets are created via Unity Editor, runtime, or via external tooling is not specified here.
