# CHAL.Data.LootDropDto

_Automatically generated/updated from `Assets/src/Data/DTO/LootDTO.cs`._

Purpose
- Defines serializable data structures for loot rules, drops, and related rules.
- All types are under namespace CHAL.Data and marked [System.Serializable].
- Establishes relationships between loot rules, drops, and rarity constraints (via references like LootRuleDto -> LootDropDto[], RarityGuaranteeKV[], SpecialRule -> LootDropDto[], SpecialRulesWrapper -> SpecialRule[]).

Public API
- Namespace/module: CHAL.Data

Types
- public class RarityGuaranteeKV
  - public Rarity rarity; // "Common", "Rare", "Epic", "Legendary"
  - public int min;

- public class LootDropDto
  - public string itemId;
  - public float chance;       // optional wenn "chances" gesetzt
  - public float[] chances;    // optional
  - public int quantity = 1;
  - public string sourceTag;

- public class LootRuleDto
  - public string tag;
  - public LootDropDto[] drops;
  - public int minDrops = 0;
  - public int maxDrops = 0;
  - public RarityGuaranteeKV[] rarityGuarantees; // optional

- public class SpecialRule
  - public string[] tags;
  - public LootDropDto[] drops;

- public class SpecialRulesWrapper
  - public SpecialRule[] rules;

Key Behavior & Side Effects
- No methods or executable logic; purely data structures.
- Serializable attribute present on all types, indicating they are intended for serialization.
- Default values present:
  - LootDropDto.quantity defaults to 1.
  - LootRuleDto.minDrops defaults to 0.
  - LootRuleDto.maxDrops defaults to 0.
- Array fields (drops, chances, rarityGuarantees, rules) are reference types and default to null unless initialized.

Constraints & Failure Modes
- Nullability:
  - Array fields (drops, chances, rarityGuarantees, rules) default to null if not assigned.
- Optional fields:
  - chance is described as optional when chances is set (per comments); behavior not defined in this file.
  - rarityGuarantees is marked as optional (no default initialization).
- No validation, parsing, or runtime logic provided here.
- No threading/async considerations within this file.

Example
```csharp
using CHAL.Data;

var exampleLootRule = new LootRuleDto
{
  tag = "example_loot",
  drops = new LootDropDto[]
  {
    new LootDropDto { itemId = "item_001", chance = 0.5f, quantity = 1, sourceTag = "spawn" }
  },
  minDrops = 1,
  maxDrops = 3,
  rarityGuarantees = new RarityGuaranteeKV[]
  {
    new RarityGuaranteeKV { rarity = Rarity.Common, min = 1 }
  }
};
```

Unknowns
- Where Rarity is defined (likely elsewhere) and its exact enum values.
- How itemId maps to actual game items or inventories.
- Exact semantics of how chance and chances are used together (not defined in this file).
- How these DTOs are consumed (JSON/Unity serialization specifics beyond [System.Serializable]).
- How SpecialRulesWrapper is consumed or applied in the loot system.
