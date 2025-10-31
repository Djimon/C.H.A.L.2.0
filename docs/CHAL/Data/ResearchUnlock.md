# CHAL.Data.ResearchUnlock

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchNodeDef.cs`._

# Purpose
- Defines the `ResearchNodeDef` class as a ScriptableObject for research nodes in the game.
- Provides fields for node identity, unlock mappings, and requirements.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class** `ResearchNodeDef` [extends `ScriptableObject`]
    - **public string** `id` - Identifier for the research node.
    - **public string** `title` - Title of the research node.
    - **public List<ResearchUnlock>** `unlocks` - List of unlock mappings for the research node.
    - **public ResearchRequirement** `requirements` - Requirements for the research node.
    - **internal string** `desc` - Description of the research node.
    - **private void** `OnValidate()` - Validates the title; sets it to the name if empty or whitespace.

  - **[Serializable] public struct** `ResearchUnlock`
    - **public ResearchUnlockTypes** `unlockType` - Type of unlock.
    - **public string** `targetId` - Identifier for the target of the unlock.

# Key Behavior & Side Effects
- `OnValidate()` ensures that the `title` is set to the object's name if it is empty or consists only of whitespace.

# Constraints & Failure Modes
- No explicit guards or null/empty handling beyond the `OnValidate()` method for the `title`.
- No threading or async considerations present.

# Example
```csharp
var researchNode = ScriptableObject.CreateInstance<ResearchNodeDef>();
researchNode.id = "node_001";
researchNode.title = "First Research Node";
researchNode.unlocks.Add(new ResearchUnlock { unlockType = ResearchUnlockTypes.SomeType, targetId = "target_001" });
```

# Unknowns
- The implementation details of `ResearchRequirement` and `ResearchUnlockTypes` are not provided in this file.

