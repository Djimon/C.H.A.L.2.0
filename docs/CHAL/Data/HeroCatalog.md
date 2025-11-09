# Assets/src/Systems/Heroes/HeroCatalog.cs

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroCatalog.cs`._

1) Purpose
- Manages a catalog of heroes for the game.
- Provides functionality to store and retrieve hero definitions.

2) Public API
- Namespace: CHAL.Data
- Types
  - public class HeroCatalog : ScriptableObject
    - Public fields/properties:
      - List<HeroDef> allHeroes: Stores all hero definitions.
    - Public methods:
      - IReadOnlyList<HeroDef> GetAllForUI(): Retrieves a read-only list of hero definitions for the UI.
      - HeroDef GetById(string heroId): Retrieves a hero definition by its unique identifier; returns null if not found.

3) Key Behavior & Side Effects
- OnValidate: Warns about duplicate HeroIds and resets the index when changes are made.
- EnsureIndex: Builds a dictionary index for quick lookups of hero definitions by HeroId.

4) Constraints & Failure Modes
- GetById: Returns null if heroId is null or empty.
- EnsureIndex: Only builds the index if it has not been built yet.

5) Example
```csharp
HeroCatalog heroCatalog = ScriptableObject.CreateInstance<HeroCatalog>();
var allHeroes = heroCatalog.GetAllForUI();
HeroDef hero = heroCatalog.GetById("heroId123");
```

6) Unknowns
- None.
