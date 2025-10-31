# CHAL.Core.BalanceManager

_Automatically generated/updated from `Assets/src/Core/BalanceManager.cs`._

# BalanceManager.cs

## Purpose
- Defines the `BalanceManager` class for managing game balance configurations.
- Provides access to experience points calculation and skill range values.

## Public API
- Namespace: `CHAL.Core`
- Types
  - **public class BalanceManager : MonoBehaviour**
    - **Public fields/properties**
      - `public static BalanceManager Instance { get; private set; }` - Singleton instance of `BalanceManager`.
      - `public GameBalanceConfig Config` - Accesses the game balance configuration, loading it if not set.
    - **Public methods**
      - `public static int GetXpForLevel(int level) : int` - Calculates experience points required for a given level.
      - `public void DebugXpProgression()` - Logs cumulative XP for specified levels.
      - `public float GetRangeValue(SkillRange range) : float` - Retrieves range value based on the specified skill range.

## Key Behavior & Side Effects
- **Awake()**
  - Initializes the singleton instance; destroys duplicate instances.
  - Loads the game balance configuration if not assigned.
  - Logs errors if the configuration is null.
- **GetXpForLevel(int level)**
  - Calculates XP based on a curve defined in the configuration.
- **DebugXpProgression()**
  - Logs total XP required for levels 1, 10, 50, and 100.
- **GetRangeValue(SkillRange range)**
  - Returns the range value based on the provided skill range.

## Constraints & Failure Modes
- Requires a valid `GameBalanceConfig` to function correctly; logs errors if not found.
- Singleton pattern ensures only one instance exists; destroys additional instances.

## Example
```csharp
// Example usage of BalanceManager
int xpForLevel5 = BalanceManager.GetXpForLevel(5);
float meleeRange = BalanceManager.Instance.GetRangeValue(SkillRange.Melee);
```

## Unknowns
- The structure and contents of `GameBalanceConfig` and `SkillRange` cannot be determined from this file.

