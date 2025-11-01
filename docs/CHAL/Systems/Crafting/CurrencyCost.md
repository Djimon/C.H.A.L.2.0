# CHAL.Systems.Crafting.CurrencyCost

_Automatically generated/updated from `Assets/src/Data/Defs/RecipeDef.cs`._

1) Purpose
- Defines a Crafting Recipe as a Unity ScriptableObject asset.
- Represents required inputs (MaterialCost) and currency costs (CurrencyCost) and the produced output (itemId and count).
- Stores metadata for display (displayKey, icon, tier, slotType) and output specification (outputItemId, outputCount).

2) Public API
- Namespace: CHAL.Systems.Crafting
- Types
  - public class RecipeDef : ScriptableObject
    - Public fields
      - string displayKey; // UI key for display
      - Sprite icon; // icon shown for the recipe
      - int tier = 1; // recipe tier
      - GearType slotType; // required gear slot type
      - List<MaterialCost> inputs = new(); // required item costs
      - List<CurrencyCost> currencyCosts = new(); // required currency costs
      - string outputItemId; // e.g. "gear:chest_leather"
      - [Min(1)] int outputCount = 1; // quantity produced
    - Private methods
      - private void OnValidate()
        - Ensures outputCount >= 1
        - If inputs != null, ensures each inputs[i].qty >= 1; otherwise replaces with qty = 1 while preserving itemId
        - If currencyCosts != null, ensures each currencyCosts[i].amount >= 1; otherwise replaces with amount = 1 while preserving currencyId
  - public struct MaterialCost : [Serializable]
    - Public fields
      - string itemId; // e.g. "part:iron_ingot"
      - [Min(1)] int qty; // quantity required
  - public struct CurrencyCost : [Serializable]
    - Public fields
      - string currencyId; // e.g. "gold", "orb_rare"
      - [Min(1)] int amount; // amount required

3) Key Behavior & Side Effects
- OnValidate() behavior (Unity editor/runtime validation trigger)
  - Clamps outputCount to a minimum of 1.
  - Normalizes inputs entries with qty < 1 to qty = 1, preserving itemId.
  - Normalizes currencyCosts entries with amount < 1 to amount = 1, preserving currencyId.
- Default list initializations ensure non-null lists, but individual list elements may be null (not guarded).

4) Constraints & Failure Modes
- Null-handling guards in OnValidate for inputs and currencyCosts; does not guard against null elements within those lists.
- Min(1) attributes rely on Unity editor validation; runtime enforcement depends on usage.
- Unknown external types: GearType (definition elsewhere) and any semantics of itemId/currencyId formats.
- No runtime validation beyond OnValidate; in gameplay, additional checks may be required when consuming costs or creating outputs.

5) Example
- Not derivable from this file alone; omitted.

6) Unknowns
- Definition and semantics of GearType and how it maps to actual gear slots.
- How RecipeDef assets are created/loaded and used in the crafting system beyond this file.
- The exact formats and validation rules for itemId and currencyId strings in the broader project (e.g., "gear:chest_leather", "gold").
- Any runtime implications of OnValidate beyond editor-time sanitation.
