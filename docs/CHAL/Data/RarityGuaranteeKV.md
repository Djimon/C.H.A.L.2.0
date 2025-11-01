# CHAL.Data.RarityGuaranteeKV

_Automatically generated/updated from `Assets/src/Data/DTO/LootDTO.cs`._

```text
1) Purpose
- Define serializable data transfer objects (DTOs) for loot configuration in the CHAL.Data namespace.
- Provide public fields to configure loot drops, rules, and special handling for serialization (Unity-compatible).
- No behavior or methods; used as plain data containers.

2) Public API
- Namespace: CHAL.Data
- Types
  - public class RarityGuaranteeKV
    - public Rarity rarity
      - "Common", "Rare", "Epic", "Legendary" (per comment)
    - public int min
  - public class LootDropDto
    - public string itemId
    - public float chance
      - optional when chances is set (per comment)
    - public float[] chances
      - optional
    - public int quantity = 1
    - public string sourceTag
  - public class LootRuleDto
    - public string tag
    - public LootDropDto[] drops
    - public int minDrops = 0
    - public int maxDrops = 0
    - public RarityGuaranteeKV[] rarityGuarantees
      - optional
  - public class SpecialRule
    - public string[] tags
    - public LootDropDto[] drops
  - public class SpecialRulesWrapper
    - public SpecialRule[] rules

3) Key Behavior & Side Effects
- No methods or runtime logic present.
- Data is intended to be serialized (Unity/Serializable) and consumed by loot generation logic elsewhere.

4) Constraints & Failure Modes
- All fields are public; no validation implemented here.
- Optional fields are indicated in comments; absence of arrays/objects implies null.
- Defaults:
  - LootDropDto.quantity defaults to 1
  - LootRuleDto.minDrops defaults to 0
  - LootRuleDto.maxDrops defaults to 0
- Rarity type is referenced but not defined in this file; assumed to be defined elsewhere.

5) Example
```csharp
// Minimal example: a LootRuleDto with a single drop
var rule = new CHAL.Data.LootRuleDto
{
  tag = "StarterLoot",
  drops = new CHAL.Data.LootDropDto[]
  {
    new CHAL.Data.LootDropDto { itemId = "gold_coin", chance = 0.5f, quantity = 3 }
  },
  minDrops = 1,
  maxDrops = 2
};
```

6) Unknowns
- The definition and values of Rarity (enum/class) are not shown here.
- How exactly chance and chances interact at runtime (resolution logic) is not defined in this file.
- Any usage details (where these DTOs are loaded, validated, or applied) are outside this file.
