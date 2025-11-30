# Assets/src/Systems/Skills/DamagePacket.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/DamagePacket.cs`._

# Purpose
- Defines the `DamagePacket` class for managing damage information in a skill system.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public class DamagePacket`
    - Public fields/properties:
      - `Dictionary<DamageType, float> DamagePerType`: Stores damage amounts categorized by damage type.
      - `float TotalDamageBeforeDef`: Total damage calculated before any defenses are applied.
      - `bool IsHitBased`: Indicates if the damage is based on a hit (default is true).
      - `bool IsDot`: Indicates if the damage is a damage-over-time effect (default is false).
    - Public methods:
      - `void AddDamage(DamageType type, float amount)`: Adds damage of a specified type; does nothing if amount is less than or equal to zero.

# Key Behavior & Side Effects
- The `AddDamage` method updates the `DamagePerType` dictionary and increments `TotalDamageBeforeDef` if the provided amount is greater than zero.

# Constraints & Failure Modes
- The `AddDamage` method ignores any damage amounts that are less than or equal to zero.
- No threading or async behavior is present.

# Example
```csharp
var damagePacket = new DamagePacket();
damagePacket.AddDamage(DamageType.Fire, 10f);
damagePacket.AddDamage(DamageType.Physical, 5f);
```

# Unknowns
- None.
