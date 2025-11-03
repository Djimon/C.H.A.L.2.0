# CHAL.Data.ResearchUnlock

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchNodeDef.cs`._

1) Purpose
- Defines the `ResearchNodeDef` class as a ScriptableObject for research nodes in the game.
- Provides a structure for defining research unlocks and requirements.

2) Public API
- Namespace/module: `CHAL.Data`
- Types
  - public sealed class `ResearchNodeDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string id`: Unique identifier for the research node.
      - `string title`: Title of the research node.
      - `List<ResearchUnlock> unlocks`: List of unlocks associated with the research node.
      - `ResearchRequirement requirements`: Requirements needed to unlock this research node.
      - `string desc`: Internal description of the research node.
    - Public methods:
      - `void OnValidate()`: Ensures the title is set to the name if it is empty or whitespace.

3) Key Behavior & Side Effects
- The `OnValidate` method automatically sets the `title` to the object's name if the `title` is not provided.

4) Constraints & Failure Modes
- The `title` field is validated to ensure it is not empty or whitespace during the validation phase.

5) Example
```csharp
// Example of creating a ResearchNodeDef in Unity
ResearchNodeDef researchNode = ScriptableObject.CreateInstance<ResearchNodeDef>();
researchNode.id = "node_001";
researchNode.title = "Basic Research";
researchNode.unlocks.Add(new ResearchUnlock { unlockType = ResearchUnlockTypes.SomeType, targetId = "target_001" });
```

6) Unknowns
- None.
