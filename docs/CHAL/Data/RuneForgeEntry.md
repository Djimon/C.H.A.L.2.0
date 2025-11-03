# CHAL.Data.RuneForgeEntry

_Automatically generated/updated from `Assets/src/Data/Config/RuneForgeConfig.cs`._

1) Purpose
- Define data structures for Rune Forge configuration in CHAL.Data.
- Provide a ScriptableObject config (RuneForgeConfig) that holds a list of RuneForgeEntry items for editing in the Unity inspector.
- Include editor hints (Tooltip, Range) to describe fields and constrain inputs.

2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class RuneForgeEntry
    - remain: ItemDef
      - Tooltip: "Remain-Item, das als als Input dient"
    - runes: List<RuneChance>
      - Tooltip: "Mgliche Runen + Gewichtungen"

  - public class RuneChance
    - rune: ItemDef
    - [Range(0f, 1f)] weight: float
      - Weight between 0 and 1 (editor-enforced)

  - [CreateAssetMenu(fileName = "RuneForgeConfig", menuName = "Config/RuneForgeConfig")]
    public class RuneForgeConfig : ScriptableObject
      - entries: List<RuneForgeEntry>

3) Key Behavior & Side Effects
- No methods or runtime logic; purely data containers.
- RuneForgeConfig is a ScriptableObject asset type, creatable via Unity editor menu due to CreateAssetMenu attribute.
- Editor-imposed constraints:
  - weight is edited via a slider or field limited to 0.0–1.0 due to Range attribute.
- Tooltip attributes provide in-Editor descriptions for remain and runes fields.

4) Constraints & Failure Modes
- No explicit null-safety/defaults in code; fields may be null if not initialized.
- Range attribute constrains editor input; runtime validation is not defined here.
- ItemDef type is defined elsewhere; this file does not provide its implementation.

5) Example
```csharp
// Minimal runtime/example usage (types referenced from CHAL.Data)
var exampleEntry = new CHAL.Data.RuneForgeEntry
{
    remain = someItemDef,
    runes = new List<CHAL.Data.RuneChance>
    {
        new CHAL.Data.RuneChance { rune = someRuneItemDef, weight = 0.5f }
    }
};

var cfg = UnityEngine.ScriptableObject.CreateInstance<CHAL.Data.RuneForgeConfig>();
cfg.entries = new List<CHAL.Data.RuneForgeEntry> { exampleEntry };
```

6) Unknowns
- How ItemDef is defined/implemented.
- How RuneForgeConfig.entries is consumed (where and how it’s loaded/used at runtime).
- Validation rules beyond editor Range (e.g., sum of weights, non-empty lists).
- Any serialization defaults or Unity-specific lifecycle behavior beyond CreateAssetMenu.

