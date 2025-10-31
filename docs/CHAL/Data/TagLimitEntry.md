# CHAL.Data.TagLimitEntry

_Automatically generated/updated from `Assets/src/Data/Defs/AffixFamilyDef.cs`._

# Purpose
- Defines the `AffixFamilyDef` class as a ScriptableObject for managing affix family data.
- Contains metadata, weighted affix entries, and optional tag limits.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public sealed class AffixFamilyDef : ScriptableObject`
    - Public fields/properties:
      - `public string FamilyName;` - Name of the affix family.
      - `public List<AffixEntry> Entries;` - List of weighted affix entries.
      - `public List<TagLimitEntry> TagLimits;` - List of tag limits.
    - Public methods:
      - `private void OnValidate();` - Validates and normalizes entries and tag limits.

  - `public struct AffixEntry`
    - Public fields/properties:
      - `public string AffixId;` - Identifier for the affix.
      - `public int Weight;` - Weight of the affix.

  - `public struct TagLimitEntry`
    - Public fields/properties:
      - `public string Tag;` - Tag associated with the limit.
      - `public int Limit;` - Limit for the tag.

# Key Behavior & Side Effects
- `OnValidate` method:
  - Ensures that weights in `Entries` are non-negative.
  - Trims whitespace from `AffixId` in `Entries`.
  - Normalizes tags in `TagLimits` to lowercase and trims whitespace.
  - Sets `Limit` in `TagLimits` to -1 if less than -1.

# Constraints & Failure Modes
- Handles null checks for `Entries` and `TagLimits`.
- Ensures that `Weight` and `Limit` values are within valid ranges.

# Example
```csharp
var affixFamily = ScriptableObject.CreateInstance<AffixFamilyDef>();
affixFamily.FamilyName = "Example Affix Family";
affixFamily.Entries.Add(new AffixEntry { AffixId = "affix1", Weight = 10 });
affixFamily.TagLimits.Add(new TagLimitEntry { Tag = "exampleTag", Limit = 5 });
```

# Unknowns
- No external dependencies or usage context is provided in this file.

