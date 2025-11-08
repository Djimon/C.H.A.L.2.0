# CHAL.Data.HeroCatalog

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroCatalog.cs`._

# Purpose
- Manages a catalog of heroes for the game.
- Provides functionality to store and retrieve hero definitions.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `HeroCatalog` [extends `ScriptableObject`]
    - Public fields/properties:
      - `List<HeroDef> allHeroes`: List of all hero definitions.
    - Public methods:
      - `IReadOnlyList<HeroDef> GetAllForUI()`: Retrieves a read-only list of hero definitions for the UI.
      - `HeroDef GetById(string heroId)`: Retrieves a hero definition by its unique identifier; returns null if not found.

# Key Behavior & Side Effects
- `OnValidate()`: Warns about duplicate `HeroId`s and resets the index when changes are made.
- `EnsureIndex()`: Builds an index of hero definitions by their `HeroId` if it does not already exist.

# Constraints & Failure Modes
- `GetById(string heroId)`: Returns null if `heroId` is null or empty.
- `OnValidate()`: Ignores null heroes or those with empty `HeroId`s when checking for duplicates.

# Example
```csharp
HeroCatalog heroCatalog = ScriptableObject.CreateInstance<HeroCatalog>();
IReadOnlyList<HeroDef> heroes = heroCatalog.GetAllForUI();
HeroDef specificHero = heroCatalog.GetById("hero123");
```

# Unknowns
- The structure and properties of `HeroDef` are not defined in this file.
