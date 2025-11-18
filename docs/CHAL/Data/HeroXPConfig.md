# Assets/src/Data/Defs/HeroXpConfig.cs

_Automatically generated/updated from `Assets/src/Data/Defs/HeroXpConfig.cs`._

1) Purpose
- Defines a configuration asset for hero experience points (XP) in a game.

2) Public API
- Namespace: CHAL.Data
- Types
  - public class HeroXPConfig : ScriptableObject
    - Public fields/properties:
      - LevelCap: Maximum level a hero can achieve (default 100).
      - baseXpPerStandardWave: Base XP awarded per standard wave (default 100).
      - wavesRequiredPerLevel: Array defining the number of waves required for each level.
    - Public methods:
      - int GetRequiredXPForLevel(int currentLevel): Returns the required XP for a given level; returns 0 if level is invalid or exceeds LevelCap.

3) Key Behavior & Side Effects
- Returns 0 if the current level is less than 1 or greater than or equal to LevelCap.
- Returns 0 if wavesRequiredPerLevel is null or empty.
- Returns 0 if the index for the current level is out of bounds of the wavesRequiredPerLevel array.
- Calculates required XP based on the number of waves and base XP per wave.

4) Constraints & Failure Modes
- LevelCap and baseXpPerStandardWave must be at least 1 due to [Min(1)] attribute.
- Ensure wavesRequiredPerLevel is initialized and populated to avoid returning 0.

5) Example
```csharp
HeroXPConfig heroXPConfig = ScriptableObject.CreateInstance<HeroXPConfig>();
int requiredXP = heroXPConfig.GetRequiredXPForLevel(1);
```

6) Unknowns
- None.
