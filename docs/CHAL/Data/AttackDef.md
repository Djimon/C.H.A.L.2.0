# CHAL.Data.AttackDef

_Automatically generated/updated from `Assets/src/Data/Defs/AttackDef.cs`._

# Purpose
- Defines the `AttackDef` class as a ScriptableObject for attack definitions in the game.
- Contains properties for attack identity, damage, cooldown, tags, animation type, and visual effects.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class AttackDef : ScriptableObject**
    - Public fields/properties:
      - `string attackId` - Unique identifier for the attack.
      - `string displayNameKey` - Key for the display name.
      - `List<DamageEntry> damages` - List of damage types and multipliers.
      - `float cooldown` - Cooldown duration for the attack.
      - `string[] tags` - Array of tags associated with the attack.
      - `string animationType` - Type of animation to play for the attack.
      - `GameObject vfxPrefab` - Prefab for visual effects associated with the attack.
  - **public struct DamageEntry**
    - Public fields/properties:
      - `DamageType DmgType` - Type of damage.
      - `float DmgMultiplier` - Multiplier for the damage type.
    - Public methods:
      - `DamageEntry(DamageType type, float multiplier)` - Constructor to initialize a `DamageEntry`.

# Key Behavior & Side Effects
- The `AttackDef` class is marked as obsolete, indicating it should not be used in new implementations.
- The `DamageEntry` struct allows for the definition of multiple damage types and their respective multipliers.

# Constraints & Failure Modes
- The `damages` list is initialized to an empty list by default.
- The `cooldown` is initialized to 0 by default.
- No explicit null or empty handling is defined for the fields.

# Example
```csharp
AttackDef fireballAttack = ScriptableObject.CreateInstance<AttackDef>();
fireballAttack.attackId = "fireball_poison";
fireballAttack.damages.Add(new DamageEntry(DamageType.Fire, 2.0f));
fireballAttack.damages.Add(new DamageEntry(DamageType.Poison, 1.0f));
fireballAttack.cooldown = 3.0f;
fireballAttack.tags = new string[] { "projectile", "aoe" };
fireballAttack.animationType = "Cast";
fireballAttack.vfxPrefab = someVfxPrefab; // Assign a GameObject prefab
```

# Unknowns
- Specific details about the `DamageType` enumeration are not provided in this file.
- The context or usage of the `displayNameKey` is not defined.

