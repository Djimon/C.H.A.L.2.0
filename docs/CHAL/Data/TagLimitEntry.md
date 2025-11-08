# CHAL.Data.TagLimitEntry

_Automatically generated/updated from `Assets/src/Data/Defs/AffixFamilyDef.cs`._

# Purpose
- Defines the `AffixFamilyDef` class as a ScriptableObject for managing a family of affixes in the game.
- Contains metadata, weighted affix entries, and optional tag limits.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public sealed class `AffixFamilyDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string FamilyName`: Name of the affix family.
      - `List<AffixEntry> Entries`: List of weighted affix entries.
      - `List<TagLimitEntry> TagLimits`: List of optional tag limits.
    - Public methods:
      - `void OnValidate()`: Validates and normalizes entries and tag limits when the object is modified in the editor.

  - [Serializable] struct `AffixEntry`
    - Public fields/properties:
      - `string AffixId`: Identifier for the affix.
      - `int Weight`: Weight of the affix.

  - [Serializable] struct `TagLimitEntry`
    - Public fields/properties:
      - `string Tag`: Tag associated with the limit.
      - `int Limit`: Limit for the tag.

# Key Behavior & Side Effects
- `OnValidate()` ensures:
  - Affix weights are non-negative.
  - Affix IDs are trimmed of whitespace.
  - Tag limits are normalized and set to -1 if negative.

# Constraints & Failure Modes
- Handles null/empty lists for `Entries` and `TagLimits`.
- Ensures that `Limit` in `TagLimitEntry` is set to -1 if it is less than -1.

# Example
```csharp
var affixFamily = ScriptableObject.CreateInstance<AffixFamilyDef>();
affixFamily.FamilyName = "Example Affix Family";
affixFamily.Entries.Add(new AffixEntry { AffixId = "example_affix", Weight = 10 });
affixFamily.TagLimits.Add(new TagLimitEntry { Tag = "example_tag", Limit = 5 });
```

# Unknowns
- No external dependencies or usage contexts are defined in this file.

