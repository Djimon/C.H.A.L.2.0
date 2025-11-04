# CHAL.Data.ImplicitWeight

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitGearTypeConfig.cs`._

1) Purpose
- Defines a Unity ScriptableObject (ImplicitGearTypeConfig) that holds per-gear-type implicit weight configurations.
- Provides a default asset menu path for creating ImplicitGearTypeConfig assets in the Unity editor.
- Enforces editor-time validation to deduplicate IDs, clamp negative weights, warn on unusual IDs, and ensure default IDs exist across all gear-type pools.

2) Public API
- Namespace: CHAL.Data

- public class ImplicitGearTypeConfig : ScriptableObject
  - Public fields
    - public List<GearTypePool> Pools
      - Mapping from GearType to a list of ImplicitWeight entries (weights per implicit ID)
  - Public/Inherited members not declared here (inherited from ScriptableObject)

- public struct GearTypePool [Serializable]
  - public GearType GearType
  - public List<ImplicitWeight> Entries

- public struct ImplicitWeight [Serializable]
  - public string ImplicitId
  - public int Weight

Notes:
- ImplicitGearTypeConfig uses [CreateAssetMenu(fileName = "ImplicitGearTypeConfig", menuName = "Data/ImplicitGearTypeConfig")] to enable asset creation.
- GearType, defined elsewhere, is referenced as the key for Pools.

3) Key Behavior & Side Effects
- OnValidate() behavior (editor-time):
  - If Pools is null, returns early.
  - For each pool in Pools:
    - If pool.Entries is null, skip processing for that pool.
    - For each entry e in pool.Entries:
      - Normalize ID: e.ImplicitId = (e.ImplicitId ?? "").Trim()
      - Clamp negative weights: if e.Weight < 0, set to 0
      - Validate ID format: if !IsValidId(e.ImplicitId), log a warning about unusual IDs
      - Deduplicate within the same GearType: if non-empty ImplicitId already seen in this pool, log a warning and set e.Weight = 0; otherwise record the ID as seen
      - Write back updated entry: pool.Entries[i] = e
    - After processing entries, ensure default IDs exist:
      - For each id in DefaultImplicitIds, if not already seen in this pool, append a new ImplicitWeight { ImplicitId = id, Weight = 0 } and mark as seen
    - Save back the modified pool: Pools[p] = pool
- ID validation helper IsValidId(string id):
  - Returns true only for non-empty IDs consisting of a-z, 0-9, or underscore; otherwise false
- Warnings are emitted via DebugManager.Warning to aid editor feedback

4) Constraints & Failure Modes
- Pools may be null; OnValidate exits gracefully.
- pool.Entries may be null; that pool is skipped during validation.
- ImplicitId normalization trims whitespace; empty IDs are allowed but treated as empty in dedup logic and default-ID handling.
- Weights are clamped to non-negative values; negative inputs become 0.
- ID format rule is strict: only a-z, 0-9, and underscore; other characters trigger warnings.
- Duplicate ImplicitId within the same GearType pool is detected; duplicates are ignored by setting Weight to 0 and warning.
- Missing default IDs (listed in DefaultImplicitIds) are added with Weight = 0 to ensure baseline coverage per pool.
- Runtime behavior relies on Unity’s editor/player lifecycle (OnValidate runs in the editor; not a runtime guarantee outside editor workflow).

5) Example
- Not provided: minimal runnable example not derivable directly from the file without external context. (The file focuses on data structure and editor-time validation.)

6) Unknowns
- Exact definition and values of GearType (enum) used by GearTypePool; defined elsewhere.
- How ImplicitWeight.Weight is used at runtime (e.g., in RNG/pooling logic) beyond this file.
- Any external consumers or serialization specifics beyond Unity’s standard ScriptableObject/Serializable behavior.
