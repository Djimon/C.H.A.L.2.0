# Assets/src/Systems/Loot/UnluckyProtection.cs

_Automatically generated/updated from `Assets/src/Systems/Loot/UnluckyProtection.cs`._

# Purpose
- Manages dry streaks per rarity and calculates chance multipliers.

# Public API
- Namespace: CHAL.Systems.Loot
- Types
  - public class UnluckyProtection
    - Public methods:
      - void OnDrop(Rarity rarity) 
      - void OnFail(Rarity rarity) 
      - float GetMultiplier(Rarity rarity) 
      - string DebugInfo() 

# Key Behavior & Side Effects
- OnDrop resets the streak for the specified rarity to 0 and logs the action.
- OnFail increments the streak for the specified rarity and logs the action.
- GetMultiplier calculates a multiplier based on the current streak for the specified rarity.
- DebugInfo provides a string representation of the current streaks for Rare, Epic, and Legendary rarities.

# Constraints & Failure Modes
- Only tracks specific rarities: Rare, Epic, Legendary, Daemonic, Holy, and Mythic.
- If a rarity is not tracked, OnDrop and OnFail have no effect.

# Unknowns
- None.

