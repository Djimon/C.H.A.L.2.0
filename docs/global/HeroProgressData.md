# Assets/src/Systems/Heroes/HeroProgressData.cs

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroProgressData.cs`._

# Purpose
- Defines the `HeroProgressData` class to store progress information for a hero.

# Public API
- Namespace/module: None specified.
- Types
  - public class HeroProgressData
    - Public fields/properties:
      - `string HeroId`: References the hero definition.
      - `int Level`: Current level of the hero.
      - `int CurrentXP`: Current experience points of the hero.
      - `int TotalXP`: Total experience points (optional).
      - `int TotalOrbitPoints`: Total orbit points available.
      - `int UnspentOrbitPoints`: Orbit points that have not been spent.
      - `int UnlockedSockets`: Number of unlocked sockets.

# Key Behavior & Side Effects
- No explicit behavior or side effects defined in the class.

# Constraints & Failure Modes
- No guards, null/empty handling, threading/async notes, or performance hints evident in the code.

# Example
```csharp
HeroProgressData heroProgress = new HeroProgressData
{
    HeroId = "hero_001",
    Level = 5,
    CurrentXP = 1500,
    TotalXP = 3000,
    TotalOrbitPoints = 10,
    UnspentOrbitPoints = 3,
    UnlockedSockets = 2
};
```

# Unknowns
- Future fields such as `GearLoadoutDTO`, `IsUnlocked`, and `Nickname` are commented out and not defined in this file.
