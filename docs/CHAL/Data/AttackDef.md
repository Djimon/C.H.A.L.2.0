# Assets/src/Data/Defs/AttackDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/AttackDef.cs`._

# Purpose
- Defines the `AttackDef` class representing an attack definition used in the game, including properties for identity, damage, cooldown, and metadata.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class AttackDef : ScriptableObject**
    - **Public fields/properties:**
      - `string attackId`: Identifier for the attack.
      - `string displayNameKey`: Key for the display name.
      - `List<DamageEntry> damages`: List of damage entries associated with the attack.
      - `float cooldown`: Cooldown duration for the attack.
      - `string[] tags`: Array of tags for categorizing the attack.
      - `string animationType`: Type of animation to play when using the attack.
      - `GameObject vfxPrefab`: Visual effect prefab associated with the attack.
  - **public struct DamageEntry**
    - **Public fields/properties:**
      - `DamageType DmgType`: Type of damage.
      - `float DmgMultiplier`: Multiplier for the damage.

# Key Behavior & Side Effects
- The `AttackDef` class is marked as obsolete, indicating it should not be used in new implementations. 

# Constraints & Failure Modes
- No specific guards or null/empty handling are evident in the code.
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
