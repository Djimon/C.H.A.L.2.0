# CHAL.Core.BalanceManager

_Automatically generated/updated from `Assets/src/Core/BalanceManager.cs`._

```text
1) Purpose
- Defines BalanceManager as a central, singleton MonoBehaviour for game balancing data.
- Exposes a Config accessor (GameBalanceConfig) with lazy loading from Resources if not assigned in the Inspector.
- Provides XP calculation (GetXpForLevel), an editor-debug helper (DebugXpProgression), and range lookup (GetRangeValue) based on the balance config.

2) Public API
- Namespace/module
  - CHAL.Core

- Types
  - public class BalanceManager : MonoBehaviour
    - Public properties
      - public static BalanceManager Instance { get; private set; }
        - Singleton instance reference for global access.
      - public GameBalanceConfig Config { get; }
        - Returns the balance config; loads from Resources if not set in Inspector.
    - Public methods
      - public static int GetXpForLevel(int level)
        - Calculates and returns the XP required for the given level using the config data.
      - public void DebugXpProgression()
        - Context-menu-annotated editor helper; logs XP progression for checkpoints.
      - public float GetRangeValue(SkillRange range)
        - Returns the configured range value for the given SkillRange.

Notes:
- GetXpForLevel uses Instance.config.economy.xp internally (bypassing the Config getter).
- DebugXpProgression is decorated with ContextMenu for quick in-editor invocation.

3) Key Behavior & Side Effects
- Execution order
  - BalanceManager uses [DefaultExecutionOrder(-1000)] to run very early in startup.
- Awake lifecycle
  - Enforces singleton: if another Instance exists, logs a warning and destroys the duplicate.
  - Persists across scene loads via DontDestroyOnLoad.
  - Validates config presence by touching Config during Awake; logs error if null.
- Config retrieval
  - Config property: if private config is null, attempts Resources.Load<GameBalanceConfig>("Config/GameBalanceConfig") and caches it; logs error if loading fails.
- XP calculation
  - GetXpForLevel(level): reads xp from Instance.config.economy.xp, applies a scale from levelCurveFactor, and computes baseXp * (1 + scale*(level-1))^2, rounded to int.
- Debugging XP
  - DebugXpProgression(): iterates through checkpoint levels, sums XP up to each level using GetXpForLevel, and logs per checkpoint via DebugManager.Log.
- Range lookup
  - GetRangeValue(range): reads Instance.Config.skillRanges and maps SkillRange values to the corresponding range fields:
    - Self, Melee, Reach, MidDistance, FarDistance
    - Defaults to meleeRange for unknown values.

4) Constraints & Failure Modes
- Null/Inspector risks
  - If Config is not set in Inspector and Resources.Load fails, Config remains null and usage may cause null references.
  - GetXpForLevel accesses Instance.config directly; if BalanceManager.Instance is not initialized or config is null, this can throw a NullReferenceException.
- Runtime lifecycle
  - Requires BalanceManager to exist in the scene (or be instantiated) before calling static methods that rely on Instance.
- Resource path
  - Lazy load uses "Resources.Load<GameBalanceConfig>("Config/GameBalanceConfig")" – must exist at that path in the project.
- Editor-only features
  - DebugXpProgression is available via ContextMenu; relies on UnityEditor-backed tooling at edit time.

5) Example
- (Not included: no clear minimal usage example deriveable beyond surface API.)

6) Unknowns
- Exact structure and fields of GameBalanceConfig (beyond those accessed here: economy.xp, economy.xp.baseLevelUpXp, economy.xp.levelCurveFactor, skillRanges.selfRange, meleeRange, etc.).
- Definition of SkillRange enum (values used: Self, Melee, Reach, MidDistance, FarDistance).
- Details of DebugManager.Log and Debug levels.
- Existence and contents of the Resources asset at Config/GameBalanceConfig.
- Any additional side effects from BalanceManager interactions not visible in this file.
```
