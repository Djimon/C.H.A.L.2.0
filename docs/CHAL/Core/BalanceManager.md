# CHAL.Core.BalanceManager

_Automatically generated/updated from `Assets/src/Core/BalanceManager.cs`._

# Purpose
- Manages the game balance configuration and provides access to it.

# Public API
- Namespace: CHAL.Core
- Types
  - public class BalanceManager : MonoBehaviour
    - Public fields/properties
      - static BalanceManager Instance { get; private set; }
      - GameBalanceConfig Config: Accesses the game balance configuration, loading it if necessary.
    - Public methods
      - static int GetXpForLevel(int level): Calculates the experience points required for a given level.
      - void DebugXpProgression(): Debugs the experience progression for specified levels.
      - float GetRangeValue(SkillRange range): Gets the range value based on the specified skill range.

# Key Behavior & Side Effects
- Singleton pattern ensures only one instance of BalanceManager exists; destroys any additional instances.
- Loads GameBalanceConfig from Resources if not assigned in the Inspector.
- Validates configuration on Awake, logging errors if the config is null.
- DebugXpProgression logs total experience points for specified levels.

# Constraints & Failure Modes
- Requires GameBalanceConfig to be assigned or present in Resources/Config.
- Handles null configuration gracefully by logging errors.
- Uses DebugManager for logging, which may affect performance if called frequently.

# Example
```csharp
// Example usage of BalanceManager
int xpForLevel5 = BalanceManager.GetXpForLevel(5);
BalanceManager.Instance.DebugXpProgression();
```

# Unknowns
- None.

