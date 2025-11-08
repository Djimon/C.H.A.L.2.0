# CHAL.Data.SpecialRule

_Automatically generated/updated from `Assets/src/Data/DTO/LootDTO.cs`._

# Purpose
- Defines data transfer objects (DTOs) for loot-related data in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **RarityGuaranteeKV**
    - Public fields/properties:
      - `Rarity rarity`: Represents the rarity type (e.g., "Common", "Rare", "Epic", "Legendary").
      - `int min`: Minimum value associated with the rarity.
  
  - **LootDropDto**
    - Public fields/properties:
      - `string itemId`: Identifier for the loot item.
      - `float chance`: Probability of the item dropping (optional if "chances" is set).
      - `float[] chances`: Array of probabilities for multiple drop chances (optional).
      - `int quantity`: Number of items to drop (default is 1).
      - `string sourceTag`: Tag indicating the source of the loot.
  
  - **LootRuleDto**
    - Public fields/properties:
      - `string tag`: Tag associated with the loot rule.
      - `LootDropDto[] drops`: Array of loot drop definitions.
      - `int minDrops`: Minimum number of drops (default is 0).
      - `int maxDrops`: Maximum number of drops (default is 0).
      - `RarityGuaranteeKV[] rarityGuarantees`: Array of rarity guarantees (optional).
  
  - **SpecialRule**
    - Public fields/properties:
      - `string[] tags`: Array of tags associated with the special rule.
      - `LootDropDto[] drops`: Array of loot drop definitions.
  
  - **SpecialRulesWrapper**
    - Public fields/properties:
      - `SpecialRule[] rules`: Array of special rules.

# Key Behavior & Side Effects
- No explicit behavior or side effects defined in the code.

# Constraints & Failure Modes
- No specific guards, null/empty handling, threading/async notes, or performance hints evident in the code.

# Example
```csharp
var lootDrop = new LootDropDto
{
    itemId = "sword_01",
    chance = 0.1f,
    quantity = 1,
    sourceTag = "enemy"
};
```

# Unknowns
- No unknowns identified from the provided file.
