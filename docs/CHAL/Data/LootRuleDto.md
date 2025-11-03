# CHAL.Data.LootRuleDto

_Automatically generated/updated from `Assets/src/Data/DTO/LootDTO.cs`._

1) Purpose
- Defines serializable data transfer objects for loot rules in the CHAL.Data namespace.
- Encapsulates loot drops, rarity guarantees, and special rule groupings (no behavior/logic).

2) Public API
- Namespace/module: CHAL.Data
- Types
  - [System.Serializable] public class RarityGuaranteeKV
    - public Rarity rarity; // "Common", "Rare", "Epic", "Legendary"
    - public int min;
  - [System.Serializable] public class LootDropDto
    - public string itemId;
    - public float chance;       // optional when "chances" is provided
    - public float[] chances;    // optional
    - public int quantity = 1;
    - public string sourceTag;
  - [System.Serializable] public class LootRuleDto
    - public string tag;
    - public LootDropDto[] drops;
    - public int minDrops = 0;
    - public int maxDrops = 0;
    - public RarityGuaranteeKV[] rarityGuarantees; // optional
  - [System.Serializable] public class SpecialRule
    - public string[] tags;
    - public LootDropDto[] drops;
  - [System.Serializable] public class SpecialRulesWrapper
    - public SpecialRule[] rules;

3) Key Behavior & Side Effects
- No methods or runtime logic; these are pure data containers.
- Default values present:
  - LootDropDto.quantity = 1
  - LootRuleDto(minDrops, maxDrops) = 0
- Optional fields indicated by comments (e.g., LootDropDto.chance when chances is provided; rarityGuarantees optional).

4) Constraints & Failure Modes
- No null/validation logic; arrays and complex fields may be null unless consumer initializes them.
- Public fields imply external code is responsible for correctness, serialization format, and consistency with other systems.

5) Example
- Minimal C# usage illustrating construction (types referenced from CHAL.Data):

```csharp
using CHAL.Data;

var drop = new LootDropDto
{
    itemId = "sword_basic",
    chance = 0.2f,
    quantity = 1,
    sourceTag = "loot_table_a"
};

var rule = new LootRuleDto
{
    tag = "EarlyLoot",
    drops = new LootDropDto[] { drop },
    minDrops = 0,
    maxDrops = 2
};

var wrapper = new SpecialRulesWrapper
{
    rules = new SpecialRule[]
    {
        new SpecialRule
        {
            tags = new string[] { "boss_defeat" },
            drops = new LootDropDto[] { drop }
        }
    }
};
```

6) Unknowns
- Definition of type Rarity (enum/class) is not in this file.
- How these DTOs are serialized/deserialized at runtime (Unity.JsonUtility, Newtonsoft, etc.) is not specified.
- Any higher-level validation, business rules, or integration logic are outside this file.
