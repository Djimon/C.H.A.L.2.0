# CHAL.Systems.Crafting.MaterialCost

_Automatically generated/updated from `Assets/src/Data/Defs/RecipeDef.cs`._

# Purpose
- Defines a crafting recipe as a ScriptableObject in Unity.
- Provides fields for display, input materials, currency costs, and output item details.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - **public class RecipeDef : ScriptableObject**
    - **public string displayKey**: Key for displaying the recipe.
    - **public Sprite icon**: Icon representing the recipe.
    - **public List<MaterialCost> inputs**: List of materials required for the recipe.
    - **public List<CurrencyCost> currencyCosts**: List of currency costs for the recipe.
    - **public string outputItemId**: ID of the output item (e.g., "gear:chest_leather").
    - **[Min(1)] public int outputCount**: Number of output items produced (minimum 1).
    - **private void OnValidate()**: Validates and corrects input/output values when the asset is modified.

  - **[Serializable] public struct MaterialCost**
    - **public string itemId**: ID of the material (e.g., "part:iron_ingot").
    - **[Min(1)] public int qty**: Quantity of the material required (minimum 1).

  - **[Serializable] public struct CurrencyCost**
    - **public string currencyId**: ID of the currency (e.g., "gold", "orb_rare").
    - **[Min(1)] public int amount**: Amount of currency required (minimum 1).

# Key Behavior & Side Effects
- The `OnValidate` method ensures that:
  - `outputCount` is at least 1.
  - Each `MaterialCost` in `inputs` has a quantity of at least 1.
  - Each `CurrencyCost` in `currencyCosts` has an amount of at least 1.

# Constraints & Failure Modes
- If `outputCount`, `qty` in `inputs`, or `amount` in `currencyCosts` are less than 1, they are reset to 1 during validation.
- Lists `inputs` and `currencyCosts` can be null; checks are performed before iterating.

# Example
```csharp
RecipeDef recipe = ScriptableObject.CreateInstance<RecipeDef>();
recipe.displayKey = "Basic Recipe";
recipe.outputItemId = "gear:chest_leather";
recipe.outputCount = 2;
recipe.inputs.Add(new MaterialCost { itemId = "part:iron_ingot", qty = 3 });
recipe.currencyCosts.Add(new CurrencyCost { currencyId = "gold", amount = 5 });
```

# Unknowns
- No information on how `RecipeDef` is utilized within the broader crafting system.
- No details on the behavior of `MaterialCost` and `CurrencyCost` beyond their definitions.

