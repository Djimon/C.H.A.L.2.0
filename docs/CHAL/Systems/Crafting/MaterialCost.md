# CHAL.Systems.Crafting.MaterialCost

_Automatically generated/updated from `Assets/src/Data/Defs/RecipeDef.cs`._

1) Purpose
- Defines a crafting recipe as a ScriptableObject for use in a crafting system.
- Provides fields for recipe identification, display, costs, and output.

2) Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public class `RecipeDef` : `ScriptableObject`
    - Public fields/properties:
      - `string Id` - Unique identifier for the recipe.
      - `string displayKey` - Key for displaying the recipe.
      - `Sprite icon` - Icon representing the recipe.
      - `int tier` - Tier level of the recipe (default is 1).
      - `GearType slotType` - Type of gear slot for the recipe.
      - `List<MaterialCost> inputs` - List of material costs required for the recipe.
      - `List<CurrencyCost> currencyCosts` - List of currency costs required for the recipe.
      - `string outputItemId` - Identifier for the output item (e.g., "gear:chest_leather").
      - `int outputCount` - Number of output items produced (minimum is 1).
    - Public methods:
      - `void OnValidate()` - Ensures `outputCount` and costs are valid; adjusts values if necessary.

3) Key Behavior & Side Effects
- `OnValidate()` method is called when the object is modified in the editor, enforcing minimum values for `outputCount`, `inputs`, and `currencyCosts`.

4) Constraints & Failure Modes
- `outputCount` must be at least 1; if less, it is set to 1.
- Each `MaterialCost` and `CurrencyCost` must have a quantity or amount of at least 1; if less, they are reset to 1.

5) Example
```csharp
RecipeDef recipe = ScriptableObject.CreateInstance<RecipeDef>();
recipe.Id = "recipe:example";
recipe.displayKey = "Example Recipe";
recipe.icon = someSpriteReference;
recipe.tier = 1;
recipe.slotType = GearType.Weapon;
recipe.inputs.Add(new MaterialCost { itemId = "part:iron_ingot", qty = 2 });
recipe.currencyCosts.Add(new CurrencyCost { currencyId = "gold", amount = 5 });
recipe.outputItemId = "gear:chest_leather";
recipe.outputCount = 1;
```

6) Unknowns
- None.
