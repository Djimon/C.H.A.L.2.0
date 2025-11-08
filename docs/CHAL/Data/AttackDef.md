# CHAL.Data.AttackDef

_Automatically generated/updated from `Assets/src/Data/Defs/AttackDef.cs`._

# Purpose
- Defines the `AttackDef` class representing an attack definition used in the game, including properties for identity, damage, cooldown, and metadata.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class** `AttackDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string attackId` - Unique identifier for the attack.
      - `string displayNameKey` - Key for the display name.
      - `List<DamageEntry> damages` - List of damage entries associated with the attack.
      - `float cooldown` - Cooldown duration for the attack.
      - `string[] tags` - Array of tags for categorizing the attack.
      - `string animationType` - Type of animation to play when using the attack.
      - `GameObject vfxPrefab` - Visual effect prefab associated with the attack.
  - **[System.Serializable] public struct** `DamageEntry`
    - Public fields/properties:
      - `DamageType DmgType` - Type of damage.
      - `float DmgMultiplier` - Multiplier for the damage type.
    - Public methods:
      - `DamageEntry(DamageType type, float multiplier)` - Constructor to initialize a `DamageEntry`.

# Key Behavior & Side Effects
- The `AttackDef` class is marked as obsolete, indicating it should not be used in new implementations.

# Constraints & Failure Modes
- No explicit guards or null/empty handling noted in the code.
- No threading or async considerations present.

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
