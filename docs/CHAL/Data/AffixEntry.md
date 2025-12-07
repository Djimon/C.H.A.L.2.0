# Assets/src/Data/Defs/AffixFamilyDef.cs

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
      - `public List<TagLimitEntry> TagLimits;` - List of optional tag limits.
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
  - Tag limits are normalized and constrained to valid values.

# Constraints & Failure Modes
- Handles null/empty lists for `Entries` and `TagLimits`.
- Ensures that `Weight` in `AffixEntry` is not negative.
- Normalizes `Tag` in `TagLimitEntry` to lowercase and trims whitespace.
- Limits in `TagLimitEntry` are constrained to be -1 (no limit) or greater than or equal to 0.

# Example
```csharp
AffixFamilyDef affixFamily = ScriptableObject.CreateInstance<AffixFamilyDef>();
affixFamily.FamilyName = "Example Affix Family";
affixFamily.Entries.Add(new AffixEntry { AffixId = "example_affix", Weight = 10 });
affixFamily.TagLimits.Add(new TagLimitEntry { Tag = "example_tag", Limit = 5 });
```

# Unknowns
- None.
