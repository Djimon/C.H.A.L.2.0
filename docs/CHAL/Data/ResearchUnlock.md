# CHAL.Data.ResearchUnlock

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchNodeDef.cs`._

```text
1) Purpose
- Define a Unity ScriptableObject asset ResearchNodeDef that represents a research node with identity, unlock mapping, and requirements.
- Provide a serializable structure for unlock entries via ResearchUnlock.
- Scope: CHAL.Data namespace; asset creation via menu path "Research/Node".
```

```csharp
2) Public API
- Namespace/module: CHAL.Data
- Types
  - public sealed class ResearchNodeDef : ScriptableObject
    - Public fields
      - public string id
      - public string title
      - public List<ResearchUnlock> unlocks
      - public ResearchRequirement requirements
    - Public methods
      - (none)
  - public struct ResearchUnlock
    - Public fields
      - public ResearchUnlockTypes unlockType
      - public string targetId
```

```text
3) Key Behavior & Side Effects
- OnValidate
  - If title is null/empty/whitespace, set title = name
  - This is a Unity editor callback to normalize the title when values change
- Field initialization
  - unlocks is initialized to an empty List<ResearchUnlock> by default
```

```text
4) Constraints & Failure Modes
- unlocks is initialized to a non-null list; no explicit null checks needed at runtime
- desc is internal and not exposed publicly; no initialization or serialization behavior defined here
- No validation beyond OnValidate; id/title/requirements/unlocks have no enforced constraints in this file
- Requires Unity context for OnValidate and ScriptableObject assets; CreateAssetMenu attribute enables editor asset creation at path: Research/Node
- Dependencies not defined in this file
```

```text
5) Example
- Minimal runtime instantiation (requires Unity)
```csharp
// Note: In Unity, assets are usually created via the editor, but this shows a minimal runtime instantiation.
var node = ScriptableObject.CreateInstance<CHAL.Data.ResearchNodeDef>();
node.id = "node01";
node.title = "First Node";
// Ensure a valid ResearchUnlockTypes value exists in your project
node.unlocks.Add(new CHAL.Data.ResearchUnlock
{
    unlockType = CHAL.Systems.Research.ResearchUnlockTypes.SomeType,
    targetId = "node02"
});
```
```

```text
6) Unknowns
- Details of ResearchRequirement (its structure and validation rules) are not defined here.
- Possible values and semantics of ResearchUnlockTypes are not defined here.
- How ResearchNodeDef is consumed at runtime (lookup, matching by id, effect of unlocks) is not defined in this file.
- Whether OnValidate behavior is relied upon in builds beyond the editor is not specified.
```
