# CHAL.Systems.Crafting.CraftingCatalog

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingCatalog.cs`._

# Purpose
- Defines a crafting catalog for managing recipes in the game.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - `public class CraftingCatalog : ScriptableObject`
    - Public fields/properties:
      - `public List<RecipeDef> recipes`: List of crafting recipes.

# Key Behavior & Side Effects
- None explicitly defined beyond the storage of recipes.

# Constraints & Failure Modes
- None evident.

# Example
```csharp
CraftingCatalog catalog = ScriptableObject.CreateInstance<CraftingCatalog>();
catalog.recipes.Add(new RecipeDef());
```

# Unknowns
- The structure and details of `RecipeDef` are not defined in this file.
