# Assets/src/Core/BalanceManager.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `BalanceManager` class for managing game balance configurations.
- Provides access to experience points calculations and skill range values.

# Public API
- Namespace: `CHAL.Core`
- Types
  - `public class BalanceManager : MonoBehaviour`
    - Public fields/properties:
      - `public static BalanceManager Instance { get; private set; }` - Singleton instance of `BalanceManager`.
      - `public GameBalanceConfig Config` - Accesses the game balance configuration, loading it if not assigned.
    - Public methods:
      - `public static int GetXpForLevel(int level) : int` - Calculates XP required for a given level.
      - `public void DebugXpProgression()` - Logs total XP required for specific levels.
      - `public float GetRangeValue(SkillRange range) : float` - Retrieves range value based on skill type.

# Key Behavior & Side Effects
- `Awake()` method initializes the singleton instance and ensures only one instance exists.
- Logs errors if the configuration is not set or loaded properly.
- `GetXpForLevel(int level)` computes XP based on a curve defined in the configuration.
- `DebugXpProgression()` logs cumulative XP for predefined levels.
- `GetRangeValue(SkillRange range)` returns range values based on the provided skill type.

# Constraints & Failure Modes
- If `Config` is null, it attempts to load from `Resources/Config/GameBalanceConfig`.
- Logs errors if configuration is not found or if `Config` is null during initialization.
- Uses `DontDestroyOnLoad` to persist the instance across scenes.

# Example
```csharp
// Example usage of BalanceManager
int xpForLevel5 = BalanceManager.GetXpForLevel(5);
Debug.Log($"XP required for level 5: {xpForLevel5}");
```

# Unknowns
- The structure and contents of `GameBalanceConfig` and `SkillRange` cannot be determined from this file.
```
