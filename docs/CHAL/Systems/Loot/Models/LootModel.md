# CHAL.Systems.Loot.Models.LootModel

_Automatically generated/updated from `Assets/src/Systems/Loot/Models/LootModel.cs`._

1) Purpose
- Define data models used for loot generation: LootDrop, LootRule, MergedLoot, and LootResultEntry.
- Grouped under CHAL.Systems.Loot.Models namespace for loot-related systems.
- Provide public, serializable-like field containers (no methods) used by other systems to compose and resolve loot.

2) Public API
- Namespace/module
  - CHAL.Systems.Loot.Models
- Types
  - public sealed class LootDrop
    - public string itemId
    - public int quantity
    - public float? chance          // null if chancesArray is used
    - public float[] chancesArray   // null if chance is used
    - public Rarity rarity
    - public int lootValue
    - public string sourceTag
  - public sealed class LootRule
    - public string tag
    - public List<LootDrop> drops = new()
    - public int minDrops            // 0 = ignore
    - public int maxDrops            // 0 = ignore
    - public Dictionary<Rarity, int> rarityGuarantees = new()
  - public sealed class MergedLoot
    - public List<LootDrop> drops = new()
    - public int minDrops
    - public int maxDrops
    - public Dictionary<Rarity, int> rarityGuarantees = new()
  - public sealed class LootResultEntry
    - public string EnemyId           // optional: reference to monster that generated the drop
    - public string PickedTag         // tag relevant for this drop; used by DNA resolver
    - public string ItemId            // the actual item
    - public int quantity = 1

3) Key Behavior & Side Effects
- No methods or runtime behavior defined; all types are plain data containers.
- Collections are initialized inline where used (e.g., drops, rarityGuarantees) to empty instances.
- LootDrop supports two mutually exclusive representations for drop chance:
  - chance (float?) or
  - chancesArray (float[])
  - Commentary indicates one should be used; the other set to null accordingly.

4) Constraints & Failure Modes
- minDrops/maxDrops semantics:
  - 0 means ignore (no minimum/maximum enforced).
- Collections:
  - drops and rarityGuarantees initialized to empty containers; safe to enumerate before population.
- Mutually exclusive chance representations in LootDrop:
  - If using chance, chancesArray is null; if using chancesArray, chance is null (per code comments).
- Dependencies:
  - Rarity type comes from CHAL.Data; not defined in this file.

5) Example
- Not derivable from this file alone; no behavior or methods to demonstrate usage.

6) Unknowns
- What Rarity exactly represents and how it is serialized/used elsewhere.
- How LootRule, MergedLoot, and LootResultEntry are consumed by the loot engine (other systems).
- Any serialization attributes, performance considerations, or Unity-specific integration (not present here).

