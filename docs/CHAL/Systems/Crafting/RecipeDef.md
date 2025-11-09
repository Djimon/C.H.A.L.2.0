# Assets/src/Data/Defs/RecipeDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/RecipeDef.cs`._

1) Purpose
- Defines a recipe for crafting in the game, including costs and display properties.

2) Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public class `RecipeDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string Id`: Unique identifier for the recipe.
      - `string displayKey`: Key for displaying the recipe.
      - `Sprite icon`: Icon representing the recipe.
      - `int tier`: Tier level of the recipe, default is 1.
      - `GearType slotType`: Type of gear slot for the recipe.
      - `List<MaterialCost> inputs`: List of materials required for the recipe.
      - `List<CurrencyCost> currencyCosts`: List of currency costs for the recipe.
      - `string outputItemId`: Identifier for the output item (e.g., "gear:chest_leather").
      - `int outputCount`: Number of output items produced, minimum is 1.
    - Public methods:
      - `void OnValidate()`: Validates and corrects the values of `outputCount`, `inputs`, and `currencyCosts` to ensure they meet minimum requirements.

3) Key Behavior & Side Effects
- `OnValidate` method ensures that:
  - `outputCount` is at least 1.
  - Each `MaterialCost` in `inputs` has a quantity of at least 1.
  - Each `CurrencyCost` in `currencyCosts` has an amount of at least 1.

4) Constraints & Failure Modes
- If `outputCount`, `qty` in `inputs`, or `amount` in `currencyCosts` are less than 1, they are reset to 1 during validation.
- The `inputs` and `currencyCosts` lists can be null, which is handled in the `OnValidate` method.

5) Example
```csharp
RecipeDef recipe = ScriptableObject.CreateInstance<RecipeDef>();
recipe.Id = "recipe:example";
recipe.displayKey = "Example Recipe";
recipe.icon = someSprite;
recipe.tier = 1;
recipe.slotType = GearType.Weapon;
recipe.inputs = new List<MaterialCost> { new MaterialCost { itemId = "part:iron_ingot", qty = 2 } };
recipe.currencyCosts = new List<CurrencyCost> { new CurrencyCost { currencyId = "gold", amount = 10 } };
recipe.outputItemId = "gear:chest_leather";
recipe.outputCount = 1;
```

6) Unknowns
- None.
