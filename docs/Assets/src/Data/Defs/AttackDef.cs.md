# Assets/src/Data/Defs/AttackDef.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `AttackDef` class for configuring attack properties in a game.
- Provides a `DamageEntry` struct for specifying damage types and multipliers.

## Public API
- Namespace: `CHAL.Data`
- Types
  - **public class** `AttackDef` [extends `ScriptableObject`]
    - **Public fields/properties:**
      - `string attackId` - Unique identifier for the attack.
      - `string displayNameKey` - Key for localized display name.
      - `List<DamageEntry> damages` - List of damage types and their multipliers.
      - `float cooldown` - Cooldown time for the attack.
      - `string[] tags` - Array of tags for categorizing the attack.
      - `string animationType` - Type of animation associated with the attack.
      - `GameObject vfxPrefab` - Visual effect prefab for the attack.
  - **public struct** `DamageEntry`
    - **Public fields/properties:**
      - `DamageType DmgType` - Type of damage.
      - `float DmgMultiplier` - Multiplier for the damage type.
    - **Public methods:**
      - `DamageEntry(DamageType type, float multiplier)` - Constructor to initialize a `DamageEntry`.

## Key Behavior & Side Effects
- The `AttackDef` class is marked as obsolete, indicating it should not be used in new implementations.
- The `DamageEntry` struct allows for the creation of damage configurations with specified types and multipliers.

## Constraints & Failure Modes
- The `damages` list can be empty, but it is expected to contain valid `DamageEntry` instances.
- The `cooldown` field defaults to 0, which may not be valid for all attacks.
- No threading or async handling is evident in this file.

## Example
```csharp
AttackDef fireballAttack = ScriptableObject.CreateInstance<AttackDef>();
fireballAttack.attackId = "fireball_poison";
fireballAttack.damages.Add(new DamageEntry(DamageType.Fire, 2.0f));
fireballAttack.cooldown = 3.0f;
fireballAttack.tags = new string[] { "projectile", "aoe" };
fireballAttack.animationType = "Cast";
fireballAttack.vfxPrefab = someVfxPrefab; // Assign a GameObject for visual effects
```

## Unknowns
- Specific details about the `DamageType` enum are not provided in this file.
- The implications of using the `attackId` and `displayNameKey` in the broader context of the application are not defined.
```
