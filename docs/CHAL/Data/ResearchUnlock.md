# Assets/src/Data/Defs/ResearchNodeDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchNodeDef.cs`._

# Purpose
- Defines the `ResearchNodeDef` class as a ScriptableObject for research nodes in the game.
- Provides a structure for defining research unlocks and requirements.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class** `ResearchNodeDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string id`: Unique identifier for the research node.
      - `string title`: Title of the research node.
      - `List<ResearchUnlock> unlocks`: List of unlocks associated with the research node.
      - `ResearchRequirement requirements`: Requirements needed to unlock this research node.
      - `string desc`: Internal description of the research node.
    - Public methods:
      - `void OnValidate()`: Ensures the title is set to the name if it is empty or whitespace.

  - **[Serializable] public struct** `ResearchUnlock`
    - Public fields/properties:
      - `ResearchUnlockTypes unlockType`: Type of unlock.
      - `string targetId`: Identifier for the target of the unlock.

# Key Behavior & Side Effects
- The `OnValidate` method automatically sets the `title` to the object's name if `title` is empty or consists only of whitespace.

# Constraints & Failure Modes
- The `unlocks` list is initialized to a new list to avoid null references.
- The `requirements` field is initialized to a new `ResearchRequirement` instance to ensure it is not null.

# Example
```csharp
ResearchNodeDef researchNode = ScriptableObject.CreateInstance<ResearchNodeDef>();
researchNode.id = "node_1";
researchNode.title = "First Research Node";
researchNode.unlocks.Add(new ResearchUnlock { unlockType = ResearchUnlockTypes.SomeType, targetId = "target_1" });
```

# Unknowns
- The definition and behavior of `ResearchRequirement` and `ResearchUnlockTypes` are not provided in this file.
