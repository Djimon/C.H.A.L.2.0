# CHAL.Systems.Crafting.RecipeDef

_Automatically generated/updated from `Assets/src/Data/Defs/RecipeDef.cs`._

1) Purpose
- Defines a Crafting Recipe as a ScriptableObject (RecipeDef) with item and currency costs and an output.
- Provides two serializable cost value types: MaterialCost and CurrencyCost.
- Includes editor-time validation (OnValidate) to clamp quantities to minimums and sanitize lists.

2) Public API
- Namespace/module: CHAL.Systems.Crafting

- Types
  - public class RecipeDef : ScriptableObject
    - Public fields
      - string displayKey
      - Sprite icon
      - int tier = 1
      - GearType slotType
      - List<MaterialCost> inputs = new()
      - List<CurrencyCost> currencyCosts = new()
      - string outputItemId
      - int outputCount [Min(1)]
    - Public methods
      - (None)

  - public struct MaterialCost
    - Public fields
      - string itemId
      - int qty [Min(1)]

  - public struct CurrencyCost
    - Public fields
      - string currencyId
      - int amount [Min(1)]

3) Key Behavior & Side Effects
- CreateAssetMenu attribute on RecipeDef enables Unity asset creation under Data/Crafting Recipe with default name CraftingRecipe.
- OnValidate (editor-time) behavior:
  - If outputCount < 1, reset to 1.
  - If inputs != null, for each element with qty < 1 replace that element with a new MaterialCost preserving itemId and setting qty = 1.
  - If currencyCosts != null, for each element with amount < 1 replace that element with a new CurrencyCost preserving currencyId and setting amount = 1.
- Inspector organization is indicated by the [Header] attributes: Anzeige, Kosten (Items), Kosten (Whrung), Output.

4) Constraints & Failure Modes
- Guards against null lists in OnValidate (no crash if inputs or currencyCosts are null).
- Enforced minimums via [Min(1)] attributes on outputCount, MaterialCost.qty, and CurrencyCost.amount (in editor).
- No runtime behavior defined in this file beyond asset validation; usage/interpretation of fields is defined elsewhere.

5) Example
- Not provided (no derivable minimal usage snippet beyond the asset/field definitions).

6) Unknowns
- How RecipeDef is consumed by gameplay (crafting system logic not shown here).
- Definition and behavior of GearType and how slotType affects crafting.
- Exact localization/usage of displayKey and the format/lookup for outputItemId.
- Runtime threading/async implications or serialization details beyond Unity ScriptableObject behavior.
