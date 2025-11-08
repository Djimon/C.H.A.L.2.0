# CHAL.Systems.Crafting.CurrencyCost

_Automatically generated/updated from `Assets/src/Data/Defs/RecipeDef.cs`._

# Purpose
- Defines a recipe definition used in the game, containing details about costs and display properties.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - **public class** `RecipeDef` [extends `ScriptableObject`]
    - **Public fields/properties:**
      - `string Id`
      - `string displayKey`
      - `Sprite icon`
      - `int tier` (default is 1)
      - `GearType slotType`
      - `List<MaterialCost> inputs`
      - `List<CurrencyCost> currencyCosts`
      - `string outputItemId`
      - `int outputCount` (minimum value is 1)
    - **Public methods:**
      - `void OnValidate()` (ensures `outputCount` and quantities in `inputs` and `currencyCosts` are at least 1)

  - **public struct** `MaterialCost`
    - **Public fields/properties:**
      - `string itemId`
      - `int qty` (minimum value is 1)

  - **public struct** `CurrencyCost`
    - **Public fields/properties:**
      - `string currencyId`
      - `int amount` (minimum value is 1)

# Key Behavior & Side Effects
- `OnValidate` method adjusts `outputCount` and ensures that quantities in `inputs` and `currencyCosts` are not less than 1 when the object is validated.

# Constraints & Failure Modes
- `outputCount`, `qty` in `MaterialCost`, and `amount` in `CurrencyCost` must be at least 1; otherwise, they are reset to 1 during validation.
- Lists `inputs` and `currencyCosts` can be null; checks are performed to avoid null reference exceptions.

# Example
```csharp
RecipeDef recipe = ScriptableObject.CreateInstance<RecipeDef>();
recipe.Id = "recipe_001";
recipe.displayKey = "Leather Armor";
recipe.icon = someSpriteReference;
recipe.tier = 1;
recipe.slotType = GearType.Armor;
recipe.inputs = new List<MaterialCost> { new MaterialCost { itemId = "part:leather", qty = 2 } };
recipe.currencyCosts = new List<CurrencyCost> { new CurrencyCost { currencyId = "gold", amount = 10 } };
recipe.outputItemId = "gear:chest_leather";
recipe.outputCount = 1;
```

# Unknowns
- None.

