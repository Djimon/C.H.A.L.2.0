# Assets/src/Data/Defs/ResearchNodeDef.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `ResearchNodeDef` class for representing research nodes in a game.
- Provides a structure for unlock mappings and requirements associated with research nodes.

## Public API
- **Namespace**: CHAL.Data
- **Types**:
  - **public sealed class ResearchNodeDef** [extends ScriptableObject]
    - **Public fields/properties**:
      - `string id`: Identifier for the research node.
      - `string title`: Title of the research node.
      - `List<ResearchUnlock> unlocks`: List of unlock mappings for the research node.
      - `ResearchRequirement requirements`: Requirements needed to unlock the research node.
      - `string desc`: Internal description of the research node.
    - **Public methods**:
      - `void OnValidate()`: Validates the title; sets it to the name if it is null or whitespace.

  - **[Serializable] struct ResearchUnlock**
    - **Public fields/properties**:
      - `ResearchUnlockTypes unlockType`: Type of unlock.
      - `string targetId`: Identifier for the target of the unlock.

## Key Behavior & Side Effects
- The `OnValidate` method ensures that the `title` is set to the object's name if it is empty or consists only of whitespace.

## Constraints & Failure Modes
- The `unlocks` list is initialized to a new list to prevent null reference exceptions.
- The `OnValidate` method modifies the `title` field based on its current state.

## Example
```csharp
var researchNode = ScriptableObject.CreateInstance<ResearchNodeDef>();
researchNode.id = "node_001";
researchNode.title = "First Research Node";
researchNode.unlocks.Add(new ResearchUnlock { unlockType = ResearchUnlockTypes.SomeType, targetId = "target_001" });
```

## Unknowns
- The implementation details of `ResearchRequirement` and `ResearchUnlockTypes` cannot be determined from this file.
```
