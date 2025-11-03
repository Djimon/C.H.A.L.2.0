# CHAL.Systems.Loot.LootRulesService

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRulesService.cs`._

1) Purpose
- Load, parse, and manage loot rules from data/LootRules, expose lookup by tag, and provide merged loot configurations.
- Load and expose additional "secret"/special rules from data/LootComboRules, and compute secret drops for monsters.
- Support merging rules for multiple tags or for a wave composition, including drops, counts, and rarity guarantees.

```

```text
2) Public API
- Namespace/module
  - CHAL.Systems.Loot

- Types
  - public sealed class LootRulesService
    - Public methods
      - void LoadAll()
        - Loads all normal loot rules from Resources data/LootRules, populates internal index, logs counts, and loads secret rules.
      - bool TryGetRule(string tag, out LootRule rule)
        - Tries to get a rule by tag from the internal index.
      - MergedLoot GetMergedForTags(IEnumerable<string> tags)
        - Returns a merged loot configuration by concatenating drops from all matching tag rules; combines min/max drops and rarity guarantees.
      - MergedLoot GetMergedForWave(WaveComposition wave)
        - Returns a merged loot configuration for all monsters and their bonus tags in the wave; sums min/max drops and rarities.
      - List<LootDropDto> GetSecretDrops(IEnumerable<string> monsterTags)
        - Returns extra drops from secret rules whose tag requirements are satisfied by the provided monster tags.

```

```text
3) Key Behavior & Side Effects
- LoadAll
  - Clears internal _byTag.
  - Loads TextAsset data/LootRules via Resources.LoadAll<TextAsset>.
  - For each asset:
    - Deserializes LootRuleDto via JsonUtility.FromJson.
    - Converts to LootRule with ToRule(dto, sourceName).
    - If a duplicate tag is found, logs a warning; overwrites existing entry.
    - On success, stores rule in _byTag[tag].
  - Logs loaded count.
  - Calls LoadSecretRules().
- ToRule (private)
  - Validates dto.tag is non-empty; otherwise throws.
  - Creates LootRule with tag, minDrops, maxDrops.
  - Applies rarityGuarantees (if present) to rule.rarityGuarantees, clamping min to >=0.
  - Validates dto.drops (must be non-null/non-empty); otherwise throws.
  - For each drop entry:
    - Validates itemId using ItemKey.TryParse; invalid => throw.
    - Ensures item exists in ItemRegistry; if not, creates a placeholder item and throws.
    - Creates LootDrop with itemId, quantity (min 1), chance vs chances[] selection, rarity, lootValue, sourceTag.
    - Validates: chance in [0,100] if specified; each value in chances[] in [0,100].
    - Requires either chance or chances[] to be set; otherwise throws.
    - Adds drop to rule.drops.
- GetMergedForTags
  - Creates empty MergedLoot.
  - For each requested tag:
    - If no rule found, logs a warning and continues.
    - Appends rule.drops to merged.drops.
    - If rule.minDrops > 0, merged.minDrops = max(merged.minDrops, rule.minDrops).
    - If rule.maxDrops > 0, merged.maxDrops = max(merged.maxDrops, rule.maxDrops).
    - Merges rarity guarantees by taking max per rarity key.
- GetMergedForWave
  - Creates empty MergedLoot.
  - For each monster in wave.Monsters, iterates counts and bonusTags.
  - For each tag with a matching rule:
    - Appends rule.drops to merged.drops.
    - Sums minDrops (+= rule.minDrops) and maxDrops (+= rule.maxDrops) when >0.
    - Sums rarity guarantees per rarity (adds min per rarity).
- LoadSecretRules
  - Clears _secretRules.
  - Loads data/LootComboRules assets as TextAsset.
  - Deserializes SpecialRulesWrapper via JsonUtility.FromJson.
  - If wrapper and wrapper.rules present, appends to _secretRules.
  - Logs loaded secret rule count.
- GetSecretDrops
  - Iterates _secretRules.
  - If monsterTags satisfy rule.tags via MatchesAll, adds rule.drops to extras.
  - Returns extras.
- MatchesAll (private)
  - Returns true if all requiredTags are contained in presentTags.

```

```text
4) Constraints & Failure Modes
- Input validation
  - tag must be non-empty; otherwise throws.
  - drops must be non-null/non-empty; otherwise throws.
  - itemId must parse as a valid ItemKey; otherwise throws.
  - item must exist in ItemRegistry; if not, creates a placeholder item and throws.
  - drops must have either a valid chance or a chances[] array; otherwise throws.
  - numeric values (chance and each entry in chances[]) must be in [0,100]; otherwise throws.
- Runtime behavior
  - Duplicate tags log a warning and are overwritten.
  - Missing rule for a tag in GetMergedForTags logs a warning and skips that tag.
  - Secret rules are loaded from data/LootComboRules; errors are logged but loading continues.
- Threading/async
  - All loading is synchronous; no explicit threading or async handling.
- Performance/allocation
  - Merges via list concatenation and per-tag loops; uses additive/maximum accumulation strategies.
- Logging/Debug
  - Uses DebugManager for warnings, errors, and dev logs with simple categories.

```

```text
5) Example
// Minimal usage example (assuming Unity context)
var lootService = new CHAL.Systems.Loot.LootRulesService();
lootService.LoadAll();

var tags = new[] { "tough-enemy", "rare-encounter" };
var merged = lootService.GetMergedForTags(tags);

```

```text
6) Unknowns
- Definitions and semantics of:
  - LootRule, LootRuleDto, LootDrop, LootDropDto
  - MergedLoot, WaveComposition
  - SpecialRule, SpecialRulesWrapper
- Exact structure and behavior of ItemRegistry, ItemKey, and item placeholder creation.
- Details of DebugManager methods and log formatting.
- Any runtime usage of UnityEditor (namespace included but not shown in usage here).
- Any side effects of loading assets at runtime beyond what is explicit in code.

