# CHAL.Data.RuneForgeConfig

_Automatically generated/updated from `Assets/src/Data/Config/RuneForgeConfig.cs`._

```text
1) Purpose
- Defines data structures for Rune Forge configuration (used as data-only assets).
- RuneForgeEntry stores an input item (remain) and a list of possible runes with weights.
- RuneForgeConfig is a ScriptableObject asset that holds a list of RuneForgeEntry entries; includes editor-friendly asset creation.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public class RuneForgeEntry
    - public ItemDef remain; // Remain-Item, das als Input dient
    - public List<RuneChance> runes; // Mgliche Runen + Gewichtungen
  - public class RuneChance
    - public ItemDef rune;
    - public float weight; // [Range(0f, 1f)]
  - public class RuneForgeConfig : ScriptableObject
    - public List<RuneForgeEntry> entries;

Notes:
- RuneForgeEntry and RuneChance are marked [Serializable].
- RuneForgeEntry fields have [Tooltip] attributes describing their purpose.
- RuneChance.weight uses [Range(0f, 1f)] to constrain values in the editor.
- RuneForgeConfig is decorated with [CreateAssetMenu(fileName = "RuneForgeConfig", menuName = "Config/RuneForgeConfig")].

3) Key Behavior & Side Effects
- No runtime behavior or methods are defined here; these are data containers.
- Asset creation is editor-time via the CreateAssetMenu attribute on RuneForgeConfig.

4) Constraints & Failure Modes
- Weight is editor-constrained to the range [0, 1] via [Range(0f, 1f)].
- No explicit runtime validation or guards are defined in this file.
- Public fields imply potential nulls (e.g., remain, runes, entries) if not initialized externally.

5) Example
```csharp
// Minimal example: construct in code (ItemDef is defined elsewhere)
ItemDef inputItem = /* ... obtain or create ItemDef ... */;
ItemDef runeItem  = /* ... obtain or create ItemDef ... */;

var entry = new RuneForgeEntry
{
    remain = inputItem,
    runes = new List<RuneChance>
    {
        new RuneChance { rune = runeItem, weight = 0.75f }
    }
};

var config = ScriptableObject.CreateInstance<RuneForgeConfig>();
config.entries = new List<RuneForgeEntry> { entry };
```

6) Unknowns
- How this data is consumed at runtime (exact usage of remain and runes) is not defined here.
- Validation rules beyond weight range (e.g., non-null remain, non-empty runes list) are not specified.
- Details of ItemDef type and how it interacts with RuneForgeConfig are not provided in this file.
- Any serialization or asset-loading behavior beyond standard Unity ScriptableObject serialization is not specified here.
```
