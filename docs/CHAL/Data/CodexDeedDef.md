# Assets/src/Data/Defs/CodexDeedDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/CodexDeedDef.cs`._

# Purpose
- Defines the `CodexDeedDef` class as a ScriptableObject for managing research node definitions in the game.
- Provides a structure for unlocking mechanisms through `CodexUnlock`.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class** `CodexDeedDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string id`: Unique identifier for the deed.
      - `string title`: Title of the deed.
      - `List<CodexUnlock> unlocks`: List of unlocks associated with the deed.
      - `DeedRequirement requirements`: Requirements needed to unlock the deed.
      - `internal string desc`: Description of the deed.
    - Public methods:
      - `void OnValidate()`: Ensures `title` is set to the name if it is null or whitespace.

  - **[Serializable] struct** `CodexUnlock`
    - Public fields/properties:
      - `CodexUnlockTypes unlockType`: Type of unlock.
      - `string targetId`: Identifier for the target of the unlock.

# Key Behavior & Side Effects
- The `OnValidate` method automatically assigns the `title` to the `name` of the object if `title` is empty or whitespace.

# Constraints & Failure Modes
- The `unlocks` list is initialized to prevent null reference exceptions.
- The `OnValidate` method ensures that the `title` is always set to a valid string.

# Example
```csharp
CodexDeedDef myDeed = ScriptableObject.CreateInstance<CodexDeedDef>();
myDeed.id = "deed_001";
myDeed.title = "First Deed";
myDeed.unlocks.Add(new CodexUnlock { unlockType = CodexUnlockTypes.SomeType, targetId = "target_001" });
```

# Unknowns
- The specifics of `DeedRequirement` and `CodexUnlockTypes` are not defined in this file.
