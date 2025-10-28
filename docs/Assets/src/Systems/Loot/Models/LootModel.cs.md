# Assets/src/Systems/Loot/Models/LootModel.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines data models for loot drops, rules, and results in a game system.

# Public API
## Namespace
- `CHAL.Systems.Loot.Models`

## Types
- **sealed class LootDrop**
  - Public fields:
    - `string itemId` - Identifier for the item.
    - `int quantity` - Quantity of the item to drop.
    - `float? chance` - Chance of dropping (null if using `chancesArray`).
    - `float[] chancesArray` - Array of chances for multiple outcomes (null if using `chance`).
    - `Rarity rarity` - Rarity of the loot.
    - `int lootValue` - Value of the loot.
    - `string sourceTag` - Tag indicating the source of the loot.

- **sealed class LootRule**
  - Public fields:
    - `string tag` - Tag associated with the loot rule.
    - `List<LootDrop> drops` - List of possible loot drops.
    - `int minDrops` - Minimum number of drops (0 means ignored).
    - `int maxDrops` - Maximum number of drops (0 means ignored).
    - `Dictionary<Rarity, int> rarityGuarantees` - Guarantees for specific rarities.

- **sealed class MergedLoot**
  - Public fields:
    - `List<LootDrop> drops` - List of merged loot drops.
    - `int minDrops` - Minimum number of drops.
    - `int maxDrops` - Maximum number of drops.
    - `Dictionary<Rarity, int> rarityGuarantees` - Guarantees for specific rarities.

- **sealed class LootResultEntry**
  - Public fields:
    - `string EnemyId` - Optional reference to the enemy that generated the drop.
    - `string PickedTag` - Tag relevant for this drop, needed for DNA resolver.
    - `string ItemId` - Identifier for the actual item.
    - `int quantity` - Quantity of the item (default is 1).

# Key Behavior & Side Effects
- No explicit behavior or side effects are defined in this file.

# Constraints & Failure Modes
- No specific guards, null/empty handling, threading/async notes, or performance hints are evident in this file.

# Example
```csharp
var lootDrop = new LootDrop
{
    itemId = "sword_01",
    quantity = 1,
    chance = 0.5f,
    rarity = Rarity.Common,
    lootValue = 100,
    sourceTag = "enemy_01"
};

var lootRule = new LootRule
{
    tag = "enemy_loot",
    drops = new List<LootDrop> { lootDrop },
    minDrops = 1,
    maxDrops = 3,
    rarityGuarantees = new Dictionary<Rarity, int> { { Rarity.Rare, 1 } }
};
```

# Unknowns
- No unknowns are explicitly stated in this file.
```
