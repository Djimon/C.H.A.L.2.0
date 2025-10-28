# Assets/src/Systems/Heroes/HeroCatalog.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `HeroCatalog` ScriptableObject for managing a collection of heroes.
- Provides methods to retrieve heroes for UI and by their unique identifiers.

## Public API
- Namespace: `CHAL.Data`
- Types
  - `public class HeroCatalog : ScriptableObject`
    - Public fields/properties:
      - `public List<HeroDef> allHeroes`: List of all hero definitions.
    - Public methods:
      - `public IReadOnlyList<HeroDef> GetAllForUI()`: Returns the list of heroes for UI display.
      - `public HeroDef GetById(string heroId)`: Returns a hero by its ID or null if not found.

## Key Behavior & Side Effects
- `OnValidate()`: Warns about duplicate `HeroId`s and resets the index when changes are made.
- `EnsureIndex()`: Builds a dictionary index of heroes by their `HeroId` if it does not already exist.

## Constraints & Failure Modes
- `GetById(string heroId)`: Returns null if `heroId` is null or empty.
- Handles duplicates in `allHeroes` by logging a warning.
- Uses a lazy initialization pattern for the `_byId` index.

## Example
```csharp
var heroCatalog = ScriptableObject.CreateInstance<HeroCatalog>();
var allHeroes = heroCatalog.GetAllForUI();
var specificHero = heroCatalog.GetById("hero123");
```

## Unknowns
- The structure and properties of `HeroDef` are not defined in this file.
```
