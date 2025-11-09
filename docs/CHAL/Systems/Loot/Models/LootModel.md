# Assets/src/Systems/Loot/Models/LootModel.cs

_Automatically generated/updated from `Assets/src/Systems/Loot/Models/LootModel.cs`._

# Purpose
- Defines data models for loot drops, loot rules, merged loot, and loot result entries in the loot system.

# Public API
- Namespace: `CHAL.Systems.Loot.Models`
- Types
  - `sealed class LootDrop`
    - Public fields/properties:
      - `string itemId` - Identifier for the item.
      - `int quantity` - Quantity of the item.
      - `float? chance` - Chance of dropping; null if using `chancesArray`.
      - `float[] chancesArray` - Array of chances; null if using `chance`.
      - `Rarity rarity` - Rarity of the loot.
      - `int lootValue` - Value of the loot.
      - `string sourceTag` - Tag indicating the source of the loot.
  
  - `sealed class LootRule`
    - Public fields/properties:
      - `string tag` - Tag associated with the loot rule.
      - `List<LootDrop> drops` - List of possible loot drops.
      - `int minDrops` - Minimum number of drops; 0 to ignore.
      - `int maxDrops` - Maximum number of drops; 0 to ignore.
      - `Dictionary<Rarity, int> rarityGuarantees` - Guarantees for specific rarities.

  - `sealed class MergedLoot`
    - Public fields/properties:
      - `List<LootDrop> drops` - List of loot drops.
      - `int minDrops` - Minimum number of drops.
      - `int maxDrops` - Maximum number of drops.
      - `Dictionary<Rarity, int> rarityGuarantees` - Guarantees for specific rarities.

  - `sealed class LootResultEntry`
    - Public fields/properties:
      - `string EnemyId` - Optional reference to the enemy that generated the drop.
      - `string PickedTag` - Tag relevant for this drop, needed for DNA resolver.
      - `string ItemId` - Identifier for the item.
      - `int quantity` - Quantity of the item, default is 1.

# Key Behavior & Side Effects
- None explicitly defined in the code.

# Constraints & Failure Modes
- `minDrops` and `maxDrops` can be set to 0 to ignore drop limits.
- `chance` and `chancesArray` are mutually exclusive; one must be null for the other to be valid.

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
    tag = "enemy_01",
    drops = new List<LootDrop> { lootDrop },
    minDrops = 1,
    maxDrops = 3,
    rarityGuarantees = new Dictionary<Rarity, int> { { Rarity.Common, 1 } }
};
```

# Unknowns
- No unknowns identified from the provided code.

