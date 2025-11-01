# CHAL.Data.SpecialRule

_Automatically generated/updated from `Assets/src/Data/DTO/LootDTO.cs`._

1) Purpose
- Define serializable data containers for loot rules, drops, and related metadata.
- Represent drop items, quantity, and optional rarity constraints for loot generation.
- Provide DTOs that can be serialized (Unity/.NET) without behavior.

2) Public API
- Namespace/module
  - CHAL.Data

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

3) Key Behavior & Side Effects
- No methods or logic defined in this file.
- Types are marked [System.Serializable], enabling serialization by Unity/.NET tooling.
- Fields are public, enabling direct serialization and access; no invariants enforced here.

4) Constraints & Failure Modes
- No constructors or validation beyond default field initializers.
- Arrays (e.g., drops, chances, rarityGuarantees) may be null unless initialized.
- Default values:
  - LootDropDto.quantity defaults to 1
  - LootRuleDto.minDrops and maxDrops default to 0
- Mutual-exclusivity/interaction between chance and chances is not enforced in code (documentation note: "chance" is optional when "chances" is set).

5) Example
```csharp
// Minimal instantiation example of a LootRuleDto
var rule = new CHAL.Data.LootRuleDto
{
    tag = "starter_loot",
    drops = new CHAL.Data.LootDropDto[]
    {
        new CHAL.Data.LootDropDto
        {
            itemId = "apple",
            chance = 0.25f,
            quantity = 2,
            sourceTag = "forest"
        }
    },
    minDrops = 1,
    maxDrops = 3,
    rarityGuarantees = new CHAL.Data.RarityGuaranteeKV[]
    {
        new CHAL.Data.RarityGuaranteeKV { rarity = Rarity.Common, min = 1 }
    }
};
```

6) Unknowns
- Definition of Rarity enum/type (not in this file).
- How these DTOs are consumed (who generates/uses them) and at what point in the flow.
- Exact serialization format (JSON, Unity serializer, etc.) beyond [System.Serializable].
