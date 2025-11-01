# CHAL.Data.TagLimitEntry

_Automatically generated/updated from `Assets/src/Data/Defs/AffixFamilyDef.cs`._

Purpose
- Defines a Unity ScriptableObject (AffixFamilyDef) that holds a family of weighted affix entries and optional tag limits.
- Provides two serializable data structs: AffixEntry and TagLimitEntry, used within the lists.
- Includes Unity editor integration via CreateAssetMenu for easy asset creation.

Public API
- Namespace: CHAL.Data

- Types
  - public sealed class AffixFamilyDef : ScriptableObject
    - Public fields
      - string FamilyName
        - Family name for this affix family
      - List<AffixEntry> Entries = new List<AffixEntry>()
        - Weighted affix entries
      - List<TagLimitEntry> TagLimits = new List<TagLimitEntry>()
        - Optional tag limits
    - Notes
      - No public methods defined

  - public struct AffixEntry [Serializable]
    - Public fields
      - string AffixId
      - int Weight

  - public struct TagLimitEntry [Serializable]
    - Public fields
      - string Tag
      - int Limit

Key Behavior & Side Effects
- NormalizeTag (private static string)
  - Returns "" if input is null/whitespace; otherwise returns trimmed, lowercase string.
- OnValidate (private)
  - Runs in the editor when the asset is loaded or values change.
  - Entries handling:
    - If Entries != null:
      - For each entry:
        - If Weight < 0, set Weight = 0
        - If AffixId is not null/empty, trim whitespace
      - Writes back modified entries to the list
  - TagLimits handling:
    - If TagLimits != null:
      - For each tagLimit:
        - Tag = NormalizeTag(Tag)
        - If Limit < -1, set Limit = -1 (where -1 means no limit)
      - Writes back modified tag limits to the list

Constraints & Failure Modes
- Null safety
  - OnValidate guards against null Entries and TagLimits.
- Data normalization
  - Weight is clamped to non-negative values.
  - AffixId is trimmed when present.
  - Tag is normalized to lowercase, trimmed; empty/whitespace becomes "".
  - TagLimit Limit is clamped to -1 or a non-negative value (no other validation).
- Semantics exposed only through editor-time validation; runtime behavior is not defined in this file.

Unknowns
- How AffixFamilyDef is consumed or applied at runtime beyond this file.
- Any external validation rules or constraints applied by other parts of the project.
- Default expectations for required fields (e.g., FamilyName) beyond editor-time constraints.
