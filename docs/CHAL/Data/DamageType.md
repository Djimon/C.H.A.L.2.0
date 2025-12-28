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
      - `Fire`
      - `Cold`
      - `Lightning`
      - `Earth`
      - `Poison`
      - `Arcane`
      - `Daemonic` // cap with Attunement to "Diabolic"
      - `Holy` // cap with Attunement to "Seraphic"
      - `Void`
      - `Abyssal`
  - `public enum Attunement`
    - Values:
      - `Diabolic` = -4
      - `Infernal` = -3
      - `Fallen` = -2
      - `Tainted` = -1
      - `Neutral` = 0
      - `Blessed` = 1
      - `Sanctified` = 2
      - `Celestial` = 3
      - `Seraphic` = 4

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
