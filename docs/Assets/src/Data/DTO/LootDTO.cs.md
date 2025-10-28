# Assets/src/Data/DTO/LootDTO.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines data transfer objects (DTOs) for loot-related data in a game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `RarityGuaranteeKV`
    - Public fields/properties:
      - `Rarity rarity`: Represents the rarity type ("Common", "Rare", "Epic", "Legendary").
      - `int min`: Minimum value associated with the rarity.
  - `LootDropDto`
    - Public fields/properties:
      - `string itemId`: Identifier for the loot item.
      - `float chance`: Probability of the item being dropped (optional if "chances" is set).
      - `float[] chances`: Array of probabilities for multiple drop chances (optional).
      - `int quantity`: Number of items to drop (default is 1).
      - `string sourceTag`: Tag indicating the source of the loot.
  - `LootRuleDto`
    - Public fields/properties:
      - `string tag`: Tag associated with the loot rule.
      - `LootDropDto[] drops`: Array of loot drop definitions.
      - `int minDrops`: Minimum number of drops (default is 0).
      - `int maxDrops`: Maximum number of drops (default is 0).
      - `RarityGuaranteeKV[] rarityGuarantees`: Array of rarity guarantees (optional).
  - `SpecialRule`
    - Public fields/properties:
      - `string[] tags`: Array of tags associated with the special rule.
      - `LootDropDto[] drops`: Array of loot drop definitions.
  - `SpecialRulesWrapper`
    - Public fields/properties:
      - `SpecialRule[] rules`: Array of special rules.

# Key Behavior & Side Effects
- None specified in the file.

# Constraints & Failure Modes
- No explicit guards or null/empty handling noted.
- No threading/async notes or performance hints evident.

# Example
```csharp
var lootRule = new LootRuleDto
{
    tag = "LootTag",
    drops = new LootDropDto[]
    {
        new LootDropDto { itemId = "Item1", chance = 0.5f, quantity = 2 },
        new LootDropDto { itemId = "Item2", chances = new float[] { 0.3f, 0.7f } }
    },
    minDrops = 1,
    maxDrops = 3
};
```

# Unknowns
- No information on how these DTOs are utilized or integrated within the broader system.
```
