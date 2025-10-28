# Assets/src/Systems/Crafting/CraftingCatalog.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a crafting catalog for managing recipes in the game.

# Public API
- Namespace: CHAL.Systems.Crafting
- Types
  - public class CraftingCatalog : ScriptableObject
    - Public fields/properties:
      - public List<RecipeDef> recipes: List of crafting recipes.

# Constraints & Failure Modes
- None evident in the provided code.

# Example
```csharp
CraftingCatalog catalog = ScriptableObject.CreateInstance<CraftingCatalog>();
catalog.recipes = new List<RecipeDef>();
```

# Unknowns
- Details about the `RecipeDef` type are not provided in this file.
```
