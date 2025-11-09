# Assets/src/Systems/Crafting/CraftingCatalog.cs

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingCatalog.cs`._

# Purpose
- Defines a crafting catalog for managing recipes in the game.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public class `CraftingCatalog` : `ScriptableObject`
    - Public fields/properties:
      - `List<RecipeDef> recipes`: A list of crafting recipes.

# Key Behavior & Side Effects
- None explicitly defined in this file.

# Constraints & Failure Modes
- None explicitly defined in this file.

# Example
```csharp
CraftingCatalog catalog = ScriptableObject.CreateInstance<CraftingCatalog>();
catalog.recipes.Add(new RecipeDef());
```

# Unknowns
- The definition and structure of `RecipeDef` cannot be determined from this file.
