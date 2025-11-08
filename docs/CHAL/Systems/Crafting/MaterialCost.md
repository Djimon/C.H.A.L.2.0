# CHAL.Systems.Crafting.MaterialCost

_Automatically generated/updated from `Assets/src/Data/Defs/RecipeDef.cs`._

1) Purpose
- Defines a recipe for crafting in the game, including costs and display properties.

2) Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public class `RecipeDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string Id` - Unique identifier for the recipe.
      - `string displayKey` - Key for displaying the recipe.
      - `Sprite icon` - Icon representing the recipe.
      - `int tier` - Tier level of the recipe, default is 1.
      - `GearType slotType` - Type of gear slot for the recipe.
      - `List<MaterialCost> inputs` - List of material costs required for the recipe.
      - `List<CurrencyCost> currencyCosts` - List of currency costs required for the recipe.
      - `string outputItemId` - Identifier for the output item (e.g., "gear:chest_leather").
      - `int outputCount` - Number of output items produced, minimum is 1.
    - Public methods:
      - `void OnValidate()` - Validates the recipe properties; ensures outputCount and costs are at least 1.

3) Key Behavior & Side Effects
- `OnValidate` method ensures that:
  - `outputCount` is set to at least 1 if it is less.
  - Each `MaterialCost` in `inputs` has a quantity of at least 1.
  - Each `CurrencyCost` in `currencyCosts` has an amount of at least 1.

4) Constraints & Failure Modes
- `OnValidate` method handles null checks for `inputs` and `currencyCosts` before iterating.
- No threading or async behavior is present.

5) Example
```csharp
RecipeDef recipe = ScriptableObject.CreateInstance<RecipeDef>();
recipe.Id = "recipe:example";
recipe.displayKey = "Example Recipe";
recipe.icon = someSprite;
recipe.tier = 1;
recipe.slotType = GearType.Weapon;
recipe.inputs.Add(new MaterialCost { itemId = "part:iron_ingot", qty = 2 });
recipe.currencyCosts.Add(new CurrencyCost { currencyId = "gold", amount = 10 });
recipe.outputItemId = "gear:chest_leather";
recipe.outputCount = 1;
```

6) Unknowns
- None.
