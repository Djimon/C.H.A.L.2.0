# How-To: Recipes (ScriptableObject)

**Purpose**  
Define crafting operations that convert materials and currencies into a specific output item.

---

## 1) Location & Asset Creation
- **Folder (typical):** `/Assets/Data/Recipes/`
- **Create menu:** `Create > Data > Crafting Recipe`
- **Type:** `CHAL.Systems.Crafting.RecipeDef`

---

## 2) Schema (Fields of `RecipeDef`)
| Field | Type | Required | Notes |
|---|---|---:|---|
| `Id` | `string` | ✅ | Stable unique ID. |
| `displayKey` | `string` | – | For localization of the recipe’s name. |
| `icon` | `Sprite` | – | Optional. |
| `tier` | `int` | – | Balance grouping (default 1). |
| `slotType` | `GearType` | – | Target slot type for display/filters. |
| `inputs` | `List<MaterialCost>` | – | Each `MaterialCost` = `{ itemId: string, qty: int ≥ 1 }`. |
| `currencyCosts` | `List<CurrencyCost>` | – | `{ currencyId: string, amount: int ≥ 1 }`. |
| `outputItemId` | `string` | ✅ | Must exist (e.g., `gear:chest_leather`). |
| `outputCount` | `int ≥ 1` | ✅ | Default 1; clamped in `OnValidate`. |

> On validation (`OnValidate`):  
> - `outputCount` clamped to ≥ 1.  
> - Each `MaterialCost.qty` clamped to ≥ 1.  
> - Each `CurrencyCost.amount` clamped to ≥ 1.  fileciteturn1file5

---

## 3) Minimal Example
```yaml
Id: "craft_boots_iron_01"
displayKey: "RECIPE_BOOTS_IRON_01"
tier: 1
slotType: Boots
inputs:
  - { itemId: "part:leather_strip", qty: 2 }
  - { itemId: "part:iron_ingot", qty: 4 }
currencyCosts:
  - { currencyId: "gold", amount: 250 }
outputItemId: "gear:boots_iron_01"
outputCount: 1
```

---

## 4) Validation Checklist
- [ ] `outputItemId` exists in Item Registry.
- [ ] All `inputs[].itemId` exist (materials or items).
- [ ] `currencyCosts[]` non-zero, balanced.
- [ ] Tier/slotType align with output item.

---

## 5) Common Pitfalls
- Output missing from registry → crafting silently fails later.
- Zero/negative quantities (prevented by `OnValidate` but still check design intent).
- Display mismatch (slotType vs. actual output gear slot).

---

## 6) Related
- `RecipeDef.cs` (source).  fileciteturn1file5
