# CHAL.Data.DamageEntry

_Automatically generated/updated from `Assets/src/Data/Defs/AttackDef.cs`._

# Purpose
- Defines the `AttackDef` class representing an attack definition used in the game, including properties for identity, damage, cooldown, and metadata.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class AttackDef : ScriptableObject**
    - `string attackId` - Unique identifier for the attack.
    - `string displayNameKey` - Key for the display name.
    - `List<DamageEntry> damages` - List of damage types and their multipliers.
    - `float cooldown` - Cooldown time for the attack.
    - `string[] tags` - Array of tags for categorization (e.g., "aoe", "projectile", "buff").
    - `string animationType` - Type of animation to play (e.g., "MeleeSwing", "Cast", "Shoot").
    - `GameObject vfxPrefab` - Visual effect prefab associated with the attack.

  - **public struct DamageEntry**
    - `DamageType DmgType` - Type of damage.
    - `float DmgMultiplier` - Multiplier for the damage (e.g., 1.5f = 150% of enemy base damage).
    - `DamageEntry(DamageType type, float multiplier)` - Constructor to initialize `DamageEntry`.

# Key Behavior & Side Effects
- The `AttackDef` class is marked as obsolete, indicating it should not be used in new implementations. It suggests using a central `skillData` instead.

# Constraints & Failure Modes
- No explicit guards or null/empty handling noted.
- The `damages` list is initialized to an empty list by default.

# Example
```csharp
AttackDef fireballAttack = new AttackDef
{
    attackId = "fireball_poison",
    displayNameKey = "Fireball",
    damages = new List<DamageEntry>
    {
        new DamageEntry(DamageType.Fire, 2.0f),
        new DamageEntry(DamageType.Poison, 1.0f)
    },
    cooldown = 3.0f,
    tags = new string[] { "projectile", "aoe" },
    animationType = "Cast",
    vfxPrefab = someVfxPrefab
};
```

# Unknowns
- The specific implementation details of `DamageType` are not provided in this file.
