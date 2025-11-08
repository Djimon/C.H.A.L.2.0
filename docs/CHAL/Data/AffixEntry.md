# CHAL.Data.AffixEntry

_Automatically generated/updated from `Assets/src/Data/Defs/AffixFamilyDef.cs`._

# Purpose
- Defines the `AffixFamilyDef` class as a ScriptableObject for managing affix family data in Unity.
- Contains metadata, weighted affix entries, and optional tag limits.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public sealed class AffixFamilyDef : ScriptableObject`
    - Public fields/properties:
      - `public string FamilyName;` - Name of the affix family.
      - `public List<AffixEntry> Entries;` - List of weighted affix entries.
      - `public List<TagLimitEntry> TagLimits;` - List of tag limits (optional).
  - `public struct AffixEntry`
    - Public fields/properties:
      - `public string AffixId;` - Identifier for the affix.
      - `public int Weight;` - Weight of the affix.
  - `public struct TagLimitEntry`
    - Public fields/properties:
      - `public string Tag;` - Tag associated with the limit.
      - `public int Limit;` - Limit for the tag.

# Key Behavior & Side Effects
- `OnValidate()` method ensures:
  - Affix weights are non-negative.
  - Affix IDs are trimmed of whitespace.
  - Tag limits are normalized and set to -1 if invalid.

# Constraints & Failure Modes
- Handles null/empty lists for `Entries` and `TagLimits`.
- Ensures that `Limit` values are valid (-1 for no limit, >=0 for valid limits).
- Trims whitespace from `AffixId` and normalizes `Tag` to lowercase.

# Example
```csharp
var affixFamily = ScriptableObject.CreateInstance<AffixFamilyDef>();
affixFamily.FamilyName = "Example Affix Family";
affixFamily.Entries.Add(new AffixEntry { AffixId = "example_affix", Weight = 10 });
affixFamily.TagLimits.Add(new TagLimitEntry { Tag = "example_tag", Limit = 5 });
```

# Unknowns
- None.

