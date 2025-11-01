# CHAL.Data.HeroCatalog

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroCatalog.cs`._

```text
1) Purpose
- Defines a ScriptableObject data asset HeroCatalog in the CHAL.Data namespace.
- Exposes a serialized list allHeroes of HeroDef to drive UI order.
- Provides a lazy lookup index (_byId) to map HeroId to HeroDef; validates duplicates on changes.

2) Public API
- Namespace/module: CHAL.Data

- Type: public class HeroCatalog : ScriptableObject
  - Public fields/properties
    - public List<HeroDef> allHeroes
      - Ordered collection; determines UI order per comment.
  - Public methods
    - public IReadOnlyList<HeroDef> GetAllForUI()
      - Returns allHeroes in the defined order.
    - public HeroDef GetById(string heroId)
      - Returns the HeroDef for the given heroId or null if not found or input is null/empty.

Notes:
- The asset can be created via Unity's CreateAssetMenu (CreateAssetMenu attribute on the class).

3) Key Behavior & Side Effects
- OnValidate()
  - Runs in the editor when the asset is validated/edited.
  - Detects duplicate HeroId values in allHeroes and logs warnings.
  - Resets the internal index _byId to force a rebuild.
- EnsureIndex()
  - Builds _byId as a Dictionary<string, HeroDef> from allHeroes.
  - Skips null entries and entries with empty HeroId.
  - If multiple entries share the same HeroId, the last one wins in the index.
- GetAllForUI()
  - Returns the allHeroes list directly (preserves configured order).
- GetById(string)
  - If heroId is null or empty, returns null.
  - Ensures the index is built, then looks up heroId in _byId and returns the match or null.

4) Constraints & Failure Modes
- Null handling:
  - OnValidate/EnsureIndex skip null entries or entries with empty HeroId.
  - GetById returns null for null/empty heroId.
- Duplicate handling:
  - Duplicates trigger a warning via Debug.LogWarning; last encountered entry wins in the index.
- Lazy/indexing notes:
  - _byId is built lazily; subsequent lookups reuse the index until OnValidate invalidates it.
- Threading/async:
  - No explicit threading or async behavior; index is built on-demand on the main thread.
- Runtime considerations:
  - OnValidate is editor-time; behavior at runtime relies on the asset’s serialized data in the built game.

5) Example
```csharp
// Example usage (in Unity, via a MonoBehaviour or editor script with a reference to a HeroCatalog asset)
HeroCatalog catalog = /* reference to HeroCatalog asset */;
HeroDef hero = catalog.GetById("hero_01");
IReadOnlyList<HeroDef> allForUI = catalog.GetAllForUI();
```

6) Unknowns
- The definition and structure of HeroDef are not provided here beyond usage of HeroDef.HeroId.
- How HeroCatalog assets are loaded or referenced at runtime beyond returning via its public API.
- Any additional behaviors of HeroDef (serialization details, other fields) are not specified in this file.
- The exact UI implications of the order in allHeroes are implied but not detailed beyond the comment.
