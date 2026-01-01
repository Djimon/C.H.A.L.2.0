# Assets/src/Data/Defs/MonsterTagDefs.cs

_Automatically generated/updated from `Assets/src/Data/Defs/MonsterTagDefs.cs`._

# Purpose
- Defines the `MonsterTagCategory` enumeration and the `MonsterTagDef` ScriptableObject for categorizing monster tags in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public enum MonsterTagCategory`
    - Values: `Unknown`, `Species`, `Element`, `Role`, `Mechanics`, `Rank`, `Biome`, `Misc`
  - `public sealed class MonsterTagDef : ScriptableObject`
    - Public fields/properties:
      - `public string tagId`: Canonical ID for the monster tag.
      - `public MonsterTagCategory category`: Category of the monster tag, defaulting to `Unknown`.
    - Public methods:
      - `private void OnValidate()`: Trims the `tagId` if it is not null or whitespace.

# Key Behavior & Side Effects
- The `OnValidate` method ensures that the `tagId` is trimmed of whitespace when the object is validated in the Unity editor.

# Constraints & Failure Modes
- The `tagId` can be null or whitespace; the `OnValidate` method handles trimming but does not enforce a non-empty value.

# Example
```csharp
var monsterTagDef = ScriptableObject.CreateInstance<MonsterTagDef>();
monsterTagDef.tagId = "  armored  ";
monsterTagDef.category = MonsterTagCategory.Mechanics;
// After validation, monsterTagDef.tagId will be "armored".
```

# Unknowns
- No unknowns present in the file.
