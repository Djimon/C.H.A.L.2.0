# CHAL.Systems.Loot.UnluckyProtection

_Automatically generated/updated from `Assets/src/Systems/Loot/UnluckyProtection.cs`._

# Purpose
- Manages dry streaks per rarity and calculates chance multipliers.

# Public API
- Namespace: CHAL.Systems.Loot
- Types
  - public class UnluckyProtection
    - Public methods:
      - void OnDrop(Rarity rarity) 
        - Resets the streak for the given rarity when an item is dropped.
      - void OnFail(Rarity rarity) 
        - Increases the streak for the given rarity when an item is not dropped.
      - float GetMultiplier(Rarity rarity) 
        - Returns the multiplier for the current rarity based on the streak.
      - string DebugInfo() 
        - Provides debug information for logs.

# Key Behavior & Side Effects
- Streaks are initialized for each rarity upon instantiation.
- Streaks are reset to 0 when an item of the tracked rarity is dropped.
- Streaks are incremented when an item of the tracked rarity is not dropped.
- The multiplier is calculated based on the current streak and rarity.

# Constraints & Failure Modes
- Only tracks specific rarities: Rare, Epic, Legendary, Daemonic, Holy, Mythic.
- If a rarity is not tracked, the multiplier defaults to 1.

# Unknowns
- None.

