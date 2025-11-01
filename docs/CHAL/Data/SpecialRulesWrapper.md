# CHAL.Data.SpecialRulesWrapper

_Automatically generated/updated from `Assets/src/Data/DTO/LootDTO.cs`._

Purpose
- Define serializable DTOs for loot configuration in CHAL.Data.
- Represent loot drops, drop rules, and special rule groupings.
- Provide data shapes for Unity/serialization without behavior.

Public API
- Namespace: CHAL.Data
- Types
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
- No executable behavior or methods defined.
- All types are marked [System.Serializable], indicating serialization support (e.g., Unity).
- Data-only DTOs; usage implies external logic handles serialization and loot computation.

Constraints & Failure Modes
- All fields are public; no validation logic present in this file.
- Optional fields are indicated by comments; no enforced constraints here.
- Default values present for quantity (1) and min/max drops (0); nullability not specified.

Example
```csharp
using CHAL.Data;

var drop = new LootDropDto
{
    itemId = "loot_sword_01",
    chance = 0.25f,
    quantity = 2,
    sourceTag = "starter"
};

var rarity = new RarityGuaranteeKV
{
    rarity = Rarity.Common,
    min = 1
};

var rule = new LootRuleDto
{
    tag = "starter_loot",
    drops = new[] { drop },
    minDrops = 1,
    maxDrops = 3,
    rarityGuarantees = new[] { rarity }
};

// Optional: Special rule example
var special = new SpecialRule
{
    tags = new[] { "event", "holiday" },
    drops = new[] { drop }
};
```

Unknowns
- Rarity enum/type is not defined in this file.
- How exactly chance/chances are resolved or overridden is not specified here.
- Usage context (where these DTOs are consumed) is not present in this file.
