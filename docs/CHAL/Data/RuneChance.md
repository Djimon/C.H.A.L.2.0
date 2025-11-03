# CHAL.Data.RuneChance

_Automatically generated/updated from `Assets/src/Data/Config/RuneForgeConfig.cs`._

Purpose
- Define data structures for a Rune Forge configuration and a ScriptableObject to store them.
- Represent an input item (remain) and a set of possible runes with weights per entry.
- Provide a Unity Editor-friendly asset (via CreateAssetMenu) to hold multiple entries.

Public API
- Namespace/module: CHAL.Data

- public class RuneForgeEntry [Serializable]
  - public ItemDef remain
    - Tooltip: "Remain-Item, das als Input dient"
  - public List<RuneChance> runes
    - Tooltip: "Mgliche Runen + Gewichtungen"

- public class RuneChance [Serializable]
  - public ItemDef rune
  - [Range(0f, 1f)]
    - public float weight

- public class RuneForgeConfig : ScriptableObject
  - public List<RuneForgeEntry> entries

Notes
- ItemDef is an externally defined type (not defined in this file).
- No methods are defined; this file only declares data structures.
- RuneForgeConfig is a ScriptableObject intended to be created as an asset (CreateAssetMenu attribute).

Key Behavior & Side Effects
- No runtime behavior or methods defined in this file.
- CreateAssetMenu enables editor-based creation of RuneForgeConfig assets.
- Editor tooling attributes:
  - Tooltip annotations for remain and runes fields.
  - Range annotation on RuneChance.weight constrains editor input to [0, 1].

Constraints & Failure Modes
- Weight is editor-constrained to 0–1 via [Range(0f, 1f)].
- No null/validation logic in this file; consumers must handle potential nulls.
- Serialization relies on Unity; external code must supply ItemDef references for remain and rune.

Example
- Minimal illustrative usage (placeholders for ItemDef references):

```csharp
// Example usage (illustrative)
var cfg = ScriptableObject.CreateInstance<RuneForgeConfig>();
cfg.entries = new List<RuneForgeEntry>
{
    new RuneForgeEntry
    {
        remain = someRemainItemDef, // ItemDef reference
        runes = new List<RuneChance>
        {
            new RuneChance { rune = someRuneItemDef, weight = 0.5f }
        }
    }
};
```

Unknowns
- Definition and semantics of ItemDef are not provided here.
- How entries are interpreted/used at runtime (beyond data storage) is not specified.
- Validation rules beyond Weight range are not defined in this file.
- Persistence/asset lifecycle behavior (loading, refreshing) is not shown here.

