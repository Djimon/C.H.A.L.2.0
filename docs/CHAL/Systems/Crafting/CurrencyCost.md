# CHAL.Systems.Crafting.CurrencyCost

_Automatically generated/updated from `Assets/src/Data/Defs/RecipeDef.cs`._

# Purpose
- Defines a crafting recipe as a ScriptableObject in Unity.
- Provides fields for display information, input materials, currency costs, and output item details.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - `public class RecipeDef : ScriptableObject`
    - Public fields/properties:
      - `public string displayKey` - Key for displaying the recipe.
      - `public Sprite icon` - Icon representing the recipe.
      - `public List<MaterialCost> inputs` - List of materials required for the recipe.
      - `public List<CurrencyCost> currencyCosts` - List of currency costs for the recipe.
      - `public string outputItemId` - Identifier for the output item (e.g., "gear:chest_leather").
      - `public int outputCount` - Number of output items produced (minimum 1).
    - Public methods:
      - `private void OnValidate()` - Validates and adjusts `outputCount`, `inputs`, and `currencyCosts` to ensure they meet minimum requirements.

  - `public struct MaterialCost`
    - Public fields/properties:
      - `public string itemId` - Identifier for the material (e.g., "part:iron_ingot").
      - `public int qty` - Quantity of the material required (minimum 1).

  - `public struct CurrencyCost`
    - Public fields/properties:
      - `public string currencyId` - Identifier for the currency (e.g., "gold", "orb_rare").
      - `public int amount` - Amount of currency required (minimum 1).

# Key Behavior & Side Effects
- `OnValidate` method ensures:
  - `outputCount` is at least 1.
  - Each `MaterialCost` in `inputs` has a quantity of at least 1.
  - Each `CurrencyCost` in `currencyCosts` has an amount of at least 1.

# Constraints & Failure Modes
- `outputCount`, `qty`, and `amount` must be at least 1; otherwise, they are reset to 1.
- Lists `inputs` and `currencyCosts` are validated for null before processing.

# Example
```csharp
RecipeDef recipe = ScriptableObject.CreateInstance<RecipeDef>();
recipe.displayKey = "Iron Armor";
recipe.icon = someSpriteReference;
recipe.inputs.Add(new MaterialCost { itemId = "part:iron_ingot", qty = 5 });
recipe.currencyCosts.Add(new CurrencyCost { currencyId = "gold", amount = 10 });
recipe.outputItemId = "gear:chest_leather";
recipe.outputCount = 1;
```

# Unknowns
- No information on how `RecipeDef` is utilized within the broader crafting system.
- No details on the context or environment in which this ScriptableObject is expected to operate.

