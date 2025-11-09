# Assets/src/Core/BalanceManager.cs

_Automatically generated/updated from `Assets/src/Core/BalanceManager.cs`._

# Purpose
- Manages the game balance configuration and provides access to it.

# Public API
- Namespace: CHAL.Core
- Types
  - public class BalanceManager : MonoBehaviour
    - Public fields/properties
      - static BalanceManager Instance { get; private set; }
      - GameBalanceConfig Config: Accesses the game balance configuration, loading it if not set.
    - Public methods
      - static int GetXpForLevel(int level): Returns the experience points needed to reach the specified level.
      - void DebugXpProgression(): Calculates and displays total experience points up to specified checkpoint levels.
      - float GetRangeValue(SkillRange range): Returns the corresponding range value based on the specified skill range.

# Key Behavior & Side Effects
- Singleton pattern ensures only one instance of BalanceManager exists; destroys any additional instances.
- Loads GameBalanceConfig from Resources if not assigned in the Inspector.
- Validates configuration on Awake; logs errors if configuration is null.
- DebugXpProgression logs cumulative experience points for specified levels.

# Constraints & Failure Modes
- Requires GameBalanceConfig to be assigned or present in Resources/Config/GameBalanceConfig.
- Logs warnings/errors if configuration is missing or if multiple instances are created.

# Example
```csharp
// Example usage of BalanceManager
int xpForLevel5 = BalanceManager.GetXpForLevel(5);
BalanceManager.Instance.DebugXpProgression();
```

# Unknowns
- None.

