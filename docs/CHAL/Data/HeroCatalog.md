# CHAL.Data.HeroCatalog

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroCatalog.cs`._

# Purpose
- Defines a `HeroCatalog` ScriptableObject for managing a collection of heroes.
- Provides methods to retrieve heroes for UI and by their unique identifiers.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `HeroCatalog` [extends `ScriptableObject`]
    - Public fields/properties:
      - `List<HeroDef> allHeroes`: List of all hero definitions.
    - Public methods:
      - `IReadOnlyList<HeroDef> GetAllForUI()`: Returns the list of heroes for UI display.
      - `HeroDef GetById(string heroId)`: Returns a hero by its ID or null if not found.

# Key Behavior & Side Effects
- `OnValidate()`: Warns about duplicate `HeroId`s and resets the index when the asset is modified.
- `EnsureIndex()`: Builds a dictionary index of heroes by their `HeroId` if it does not exist.

# Constraints & Failure Modes
- `GetById(string heroId)`: Returns null if `heroId` is null or empty.
- Handles duplicates in `allHeroes` by logging a warning.

# Example
```csharp
var heroCatalog = ScriptableObject.CreateInstance<HeroCatalog>();
var allHeroes = heroCatalog.GetAllForUI();
var specificHero = heroCatalog.GetById("hero123");
```

# Unknowns
- The structure and properties of `HeroDef` are not defined in this file.

