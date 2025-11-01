# CHAL.Data.AffixEntry

_Automatically generated/updated from `Assets/src/Data/Defs/AffixFamilyDef.cs`._

```text
1) Purpose
- Defines a ScriptableObject asset type AffixFamilyDef that groups a collection of AffixEntry items with optional TagLimitEntries.
- Provides serializable helper types:
  - AffixEntry: string AffixId, int Weight
  - TagLimitEntry: string Tag, int Limit
- Enables Unity Editor asset creation via CreateAssetMenu.

2) Public API
- Namespace/module: CHAL.Data
- Types
  - public sealed class AffixFamilyDef : ScriptableObject
    - Public fields
      - string FamilyName
      - List<AffixEntry> Entries
      - List<TagLimitEntry> TagLimits
  - public struct AffixEntry
    - public string AffixId
    - public int Weight
  - public struct TagLimitEntry
    - public string Tag
    - public int Limit

3) Key Behavior & Side Effects
- OnValidate (private) runs in the Unity Editor to normalize and clamp data:
  - Entries: for each entry, if Weight < 0 -> set to 0; if AffixId is not null/empty -> trim whitespace.
  - TagLimits: for each entry, Tag = NormalizeTag(Tag); if Limit < -1 -> set to -1.
- NormalizeTag(string t): returns "" if t is null or whitespace; else t.Trim().ToLowerInvariant().
- TagLimits semantics: -1 means no limit; >=0 means a concrete limit.
- Modifications persist on the asset via the in-place updates of Entries/TagLimits.

4) Constraints & Failure Modes
- OnValidate guards against null lists before iterating.
- OnValidate executes in the Unity Editor; runtime behavior is not defined here.
- No runtime validation beyond OnValidate is specified.

6) Unknowns
- How other parts of the project consume AffixFamilyDef beyond these fields is not defined in this file.
- Any runtime serialization or usage specifics outside OnValidate are not described here.
