# Assets/src/Data/Defs/RecipeDef.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a crafting recipe as a ScriptableObject for use in a crafting system.
- Provides fields for display information, input materials, currency costs, and output items.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - `public class RecipeDef : ScriptableObject`
    - Public fields/properties:
      - `public string displayKey;` - Key for displaying the recipe.
      - `public Sprite icon;` - Icon representing the recipe.
      - `public List<MaterialCost> inputs;` - List of materials required for the recipe.
      - `public List<CurrencyCost> currencyCosts;` - List of currency costs for the recipe.
      - `public string outputItemId;` - ID of the output item (e.g., "gear:chest_leather").
      - `public int outputCount;` - Number of output items produced (minimum 1).
    - Public methods:
      - `private void OnValidate();` - Validates and corrects input values when the asset is modified.

  - `public struct MaterialCost`
    - Public fields/properties:
      - `public string itemId;` - ID of the material (e.g., "part:iron_ingot").
      - `public int qty;` - Quantity of the material required (minimum 1).

  - `public struct CurrencyCost`
    - Public fields/properties:
      - `public string currencyId;` - ID of the currency (e.g., "gold").
      - `public int amount;` - Amount of currency required (minimum 1).

# Key Behavior & Side Effects
- `OnValidate` ensures:
  - `outputCount` is at least 1.
  - Each `MaterialCost` in `inputs` has a quantity of at least 1.
  - Each `CurrencyCost` in `currencyCosts` has an amount of at least 1.

# Constraints & Failure Modes
- `outputCount`, `qty`, and `amount` must be at least 1; otherwise, they are reset to 1.
- Handles null checks for `inputs` and `currencyCosts` before validation.

# Example
```csharp
RecipeDef recipe = ScriptableObject.CreateInstance<RecipeDef>();
recipe.displayKey = "Basic Craft";
recipe.icon = someSprite;
recipe.inputs.Add(new MaterialCost { itemId = "part:iron_ingot", qty = 2 });
recipe.currencyCosts.Add(new CurrencyCost { currencyId = "gold", amount = 5 });
recipe.outputItemId = "gear:chest_leather";
recipe.outputCount = 1;
```

# Unknowns
- No information on how `RecipeDef` is utilized within the broader crafting system.
```
