# Assets/src/Data/Enums/DamageType.cs

_Automatically generated/updated from `Assets/src/Data/Enums/DamageType.cs`._

# Purpose
- Defines enumerations for different types of damage and attunements.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public enum DamageType`
    - Values:
      - `Physical`
      - `Fire` // 1 elem resistance for all elements
      - `Cold` // 1 elem resistance for all elements
      - `Lightning` // 1 elem resistance for all elements
      - `Earth` // not used atm, 1 elem resistance for all elements
      - `Poison`
      - `Arcane`
      - `Daemonic` // only in endgame, resist with Attunement to "Diabolic"
      - `Holy` // only in endgame, resist with Attunement to "Seraphic"
      - `Void`
      - `Abyssal`
  - `public enum Attunement`
    - Values:
      - `Diabolic` = -4 // ~ 70% Resist against Daemonic Dmg; -70% Vulnerable against Holy Dmg
      - `Infernal` = -3
      - `Fallen` = -2
      - `Tainted` = -1
      - `Neutral` = 0
      - `Blessed` = 1
      - `Sanctified` = 2
      - `Celestial` = 3
      - `Seraphic` = 4 // ~ 70% Resist against Holy Dmg; -70% Vulnerable against Daemonic Dmg 

# Key Behavior & Side Effects
- None specified.

# Constraints & Failure Modes
- None specified.

# Example
```csharp
DamageType damage = DamageType.Fire;
Attunement attunement = Attunement.Blessed;
```

# Unknowns
- None.
