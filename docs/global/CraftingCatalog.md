# global.CraftingCatalog

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingCatalog.cs`._

# Purpose
- Defines a crafting catalog for managing recipes in the game.

# Public API
- Namespace: CHAL.Systems.Crafting
- Types
  - public class CraftingCatalog : ScriptableObject
    - Public fields/properties:
      - List<RecipeDef> recipes: Stores a list of crafting recipes.

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
- Details about the RecipeDef type and its properties/methods cannot be determined from this file.

