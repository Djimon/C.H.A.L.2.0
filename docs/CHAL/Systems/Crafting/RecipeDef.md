# CHAL.Systems.Crafting.RecipeDef

_Automatically generated/updated from `Assets/src/Data/Defs/RecipeDef.cs`._

1) Purpose
- Defines a recipe for crafting in the game, including costs and display properties.

2) Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public class `RecipeDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string Id`
      - `string displayKey`
      - `Sprite icon`
      - `int tier` (default is 1)
      - `GearType slotType`
      - `List<MaterialCost> inputs`
      - `List<CurrencyCost> currencyCosts`
      - `string outputItemId`
      - `int outputCount` (minimum value is 1)
    - Public methods:
      - `void OnValidate()` (ensures outputCount and costs are valid)

  - public struct `MaterialCost`
    - Public fields/properties:
      - `string itemId`
      - `int qty` (minimum value is 1)

  - public struct `CurrencyCost`
    - Public fields/properties:
      - `string currencyId`
      - `int amount` (minimum value is 1)

3) Key Behavior & Side Effects
- `OnValidate()` method adjusts `outputCount`, `inputs`, and `currencyCosts` to ensure they meet minimum requirements when the object is validated in the editor.

4) Constraints & Failure Modes
- `outputCount`, `qty`, and `amount` must be at least 1; otherwise, they are reset to 1 in `OnValidate()`.
- `inputs` and `currencyCosts` can be null; checks are performed in `OnValidate()` to handle this.

5) Example
```csharp
RecipeDef recipe = ScriptableObject.CreateInstance<RecipeDef>();
recipe.Id = "recipe:example";
recipe.displayKey = "Example Recipe";
recipe.icon = someSpriteReference;
recipe.tier = 2;
recipe.slotType = GearType.SomeType;
recipe.inputs.Add(new MaterialCost { itemId = "part:iron_ingot", qty = 5 });
recipe.currencyCosts.Add(new CurrencyCost { currencyId = "gold", amount = 10 });
recipe.outputItemId = "gear:chest_leather";
recipe.outputCount = 1;
```

6) Unknowns
- None.
