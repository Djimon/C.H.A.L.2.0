# CHAL.Data.ImplicitGearTypeConfig

_Automatically generated/updated from `Assets/src/Data/Defs/ImplicitGearTypeConfig.cs`._

1) Purpose
- Defines a ScriptableObject ImplicitGearTypeConfig to configure per-gear-type implicit weight entries.
- Declares serializable data structures: GearTypePool and ImplicitWeight (public) to model gear-type pools and their weighted implicit IDs.
- Provides default implicit IDs and an editor-time OnValidate flow to normalize, deduplicate, and complete pools.

2) Public API
- Namespace/module: CHAL.Data
- Types
  - public class ImplicitGearTypeConfig : ScriptableObject
    - [CreateAssetMenu(fileName = "ImplicitGearTypeConfig", menuName = "Data/ImplicitGearTypeConfig")]
    - Public fields
      - public List<GearTypePool> Pools
    - Private methods
      - private void OnValidate()
        - Editor-time validation: trims IDs, clamps weights, warns on invalid/duplicate IDs, deduplicates within a pool, and appends missing default IDs with Weight = 0
      - private static bool IsValidId(string id)
        - Returns true if id consists of a-z, 0-9, or _ (non-empty); otherwise false
  - public struct GearTypePool
    - public GearType GearType
    - public List<ImplicitWeight> Entries
  - public struct ImplicitWeight
    - public string ImplicitId
    - public int Weight

3) Key Behavior & Side Effects
- OnValidate (Unity editor-time)
  - Returns early if Pools is null
  - For each pool:
    - Ensures per-GearType unique ImplicitId within pool (warns and nullifies duplicates by setting Weight = 0)
    - Trims ImplicitId and clamps negative Weight to 0
    - Logs warning if ImplicitId is not valid per IsValidId
    - Deduplicates by tracking seen IDs per GearType
    - Adds missing DefaultImplicitIds with Weight 0 if not already present
    - Writes back updated pool to Pools[p]
- DefaultImplicitIds: static array of string IDs used to fill missing entries
- Logging: uses DebugManager.Warning for invalid IDs and duplicates

4) Constraints & Failure Modes
- Pools can be null; OnValidate returns without changes
- pool.Entries can be null; that pool is skipped for processing
- IsValidId requires non-empty and only lowercase letters, digits, or underscore
- Negative weights are coerced to 0
- Duplicate IDs within a pool cause weights to be reset to 0 and the duplicate to be ignored
- Missing default IDs are appended with Weight = 0
- Editor-only behavior: OnValidate is invoked by Unity editor; runtime behavior not specified here

5) Example
- Not provided (not clearly derivable from file)

6) Unknowns
- Definition and location of the GearType enum
- How this config is used at runtime beyond editor validation
- Whether OnValidate runs in builds (likely editor-only)
- Any external constraints on the DefaultImplicitIds beyond this file
