# CHAL.Data.GearTypePool

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitGearTypeConfig.cs`._

```text
1) Purpose
- Defines a ScriptableObject data container (ImplicitGearTypeConfig) for per-gear-type implicit stat weights.
- Exposes serializable data structures (GearTypePool, ImplicitWeight) used to configure Pools and their entries.
- Provides default implicit IDs and validation logic to deduplicate, trim, clamp, and auto-fill IDs during validation.

```

```text
2) Public API
- Namespace/module: CHAL.Data

- Attributes on type
  - [CreateAssetMenu(fileName = "ImplicitGearTypeConfig", menuName = "Data/ImplicitGearTypeConfig")]

- Types

  - public class ImplicitGearTypeConfig : ScriptableObject
    - Public fields
      - public List<GearTypePool> Pools
        - Role: container of per-gear-type pools; initialized with six gear types (Head, Chest, Gloves, Legs, Boots, Amulet) each with an empty Entries list.

  - public struct GearTypePool
    - public GearType GearType
      - Role: identifies the gear type this pool applies to
    - public List<ImplicitWeight> Entries
      - Role: list of ImplicitWeight entries for this gear type

  - public struct ImplicitWeight
    - public string ImplicitId
      - Role: identifier for the implicit stat
    - public int Weight
      - Role: weight/probability value for the implicit stat

- Notes
  - GearType is referenced but defined elsewhere in the project.
  - DefaultImplicitIds is private and not part of the public API.

```

```text
3) Key Behavior & Side Effects
- OnValidate (private)
  - If Pools is null, early return.
  - For each pool in Pools:
    - Ensure a per-type HashSet<string> to detect duplicates by ImplicitId.
    - If pool.Entries is null, skip to next pool.
    - For each entry:
      - Normalize ImplicitId: trim, coerce null to empty.
      - Clamp Weight: if Weight < 0, set to 0.
      - Warn if ImplicitId is not valid (only a-z, 0-9, _ allowed).
      - Deduplicate within the same GearType:
        - If ImplicitId is non-empty and already seen in this pool, warn and set Weight = 0.
        - Otherwise, mark as seen.
      - Write back the possibly updated entry to pool.Entries[i].
    - Ensure all default IDs are present:
      - For each id in DefaultImplicitIds, if not already seen, add new ImplicitWeight { ImplicitId = id, Weight = 0 } to pool.Entries.
    - Save pool back into Pools[p].
- IsValidId (private static)
  - Returns false if id is null/empty.
  - Returns true only if all characters are a-z, 0-9, or '_'.
  - Used to emit warnings for non-conforming IDs.

- DefaultImplicitIds (private static)
  - Array of string IDs used to fill in missing defaults during validation:
    - "dmg_pct", "thorns_flat", "phys_dmg_flat", "elem_dmg_flat", "armor_pct",
      "elem_resist_pct", "dodge_pct", "barrier_pct", "armor_flat", "barrier_flat",
      "life_pct", "life_flat", "item_rarity_pct", "move_speed_pct"

```

```text
4) Constraints & Failure Modes
- Pools null: gracefully skips processing.
- Entries null: skipped for that pool; still fills in missing defaults.
- Negative Weight: clamped to 0.
- ImplicitId null/empty: treated as empty; can still cause missing-defaults filling but may trigger “unexpected ID” warnings if non-empty after trim.
- Invalid ImplicitId (not lowercase letters, digits, or underscore): triggers a Debug.LogWarning.
- Duplicate ImplicitId within the same GearType: triggers a Debug.LogWarning and sets that entry’s Weight to 0.
- Default IDs: missing IDs from DefaultImplicitIds are auto-added with Weight 0, ensuring a baseline set for every pool.
- Threading/async: OnValidate is a Unity editor callback; behavior is editor-time data validation (not runtime guarantees) and relies on Unity serialization lifecycle.
- Performance: validation touches all pools/entries and may allocate for per-type lookups; intended for editor-time data hygiene rather than frequent runtime cost.

```

```text
5) Example
- Minimal usage snippet (illustrative; relies on Unity/CHAL types):

// C# example demonstrating creating a config instance programmatically
using UnityEngine;
using CHAL.Data;
using System.Collections.Generic;

public class ExampleUsage
{
    public void CreateConfig()
    {
        var cfg = ScriptableObject.CreateInstance<ImplicitGearTypeConfig>();
        cfg.Pools = new List<GearTypePool>
        {
            new GearTypePool
            {
                GearType = GearType.Head,
                Entries = new List<ImplicitWeight>
                {
                    new ImplicitWeight { ImplicitId = "dmg_pct", Weight = 5 }
                }
            }
        };
        // In the editor, you would typically create an asset via CreateAssetMenu instead.
    }
}

```

```text
6) Unknowns
- The exact definition and values of GearType (enum) are not present in this file.
- The behavior and usage of the created asset at runtime (beyond OnValidate-time validation) are not specified here.
- Any additional validation or usage of the Pools data outside OnValidate is not defined in this file.
- External tooling or scripts that may rely on DefaultImplicitIds order or presence are not described here.
```
