# How-To: Items (ScriptableObject)

**Purpose**  
Let designers add/modify items safely using the canonical `ItemDef` ScriptableObject. This guide lists every field, allowed values, validation rules, and practical examples.

---

## 1) Location & Asset Creation
- **Folder (typical):** `/Assets/Data/Items/`
- **Create menu:** `Create > Data > ItemDef`
- **Type:** `CHAL.Data.ItemDef`

---

## 2) Schema (Fields of `ItemDef`)
| Field | Type | Required | Notes |
|---|---|---:|---|
| `itemId` | `string` | ✅ | Must match schema **`category:item`**, e.g. `gear:boots_iron_01`. Parsed by `ItemTypeUtils.FromId` and validated by `ItemKey.TryParse`. |
| `itemType` | `ItemType` (Hidden) | auto | Derived from `itemId`. Do **not** edit manually. |
| `description` | `string` (TextArea) | – | Localized name is derived elsewhere (TODO in project). |
| `icon` | `Sprite` | – | Optional, used by UI. |
| `rarity` | `Rarity` | – | Default: `Common`. Comment suggests typical tiers: Common, Rare, Epic, Legendary. |
| `lootValue` | `int` | ✅ | Softcap/Budget. Suggested ranges: Common 10, Rare 30, Epic 50, Legendary 80. Clamped to ≥ 0. |
| `remainData` | `RemainData` | 1-of-N | Only set when `itemType == Remains`. |
| `runeData` | `RuneData` | 1-of-N | Only set when `itemType == Rune`. |
| `partData` | `PartData` | 1-of-N | Only set when `itemType == Part`. |
| `moduleData` | `ModuleData` | 1-of-N | Only set when `itemType == Module`. |
| `gearData` | `GearData` | 1-of-N | Only set when `itemType == Gear`. |

**Type-specific structs**  
- `RemainData` → `remainType: string` (e.g., *Insect*, *Beast*).  
- `RuneData` → `effectType: string`, `runeColortType: RuneColorType` (derived color via `RuneColors.Get`).  
- `PartData` → `dnaType: string`, `moduleFuel: List<ItemDef>` (fuel whitelist).  
- `ModuleData` → `effect: string`, `modulePower: float`.  
- `GearData` → `slotType: GearType` (Head/Chest/Gloves/Legs/Boots/Amulet …), `tags: string[]` (e.g., `gear`, `leather`, `light`), `runeSocketType: RuneColorType` (optional, keep `None` if unused).

> On validation (`OnValidate`):  
> - `itemType` derived from `itemId`.  
> - `itemId` must parse (`ItemKey.TryParse`).  
> - `lootValue` clamped to ≥ 0.  
> - **Type exclusivity** enforced: only the matching data block remains; others are cleared.  fileciteturn1file4

---

## 3) IDs & Naming
- **Pattern:** `category:item` (two-part). For gear, recommended detail in item: `gear:boots_iron_01`.
- Keep IDs stable (prefer deprecation + migration over renaming).

---

## 4) Minimal Examples

### 4.1 Remains
```yaml
itemId: "remains:gland"
rarity: Common
lootValue: 10
remainData:
  remainType: "Insect"
```

### 4.2 Rune
```yaml
itemId: "rune:sky_strike"
rarity: Rare
lootValue: 30
runeData:
  effectType: "Armor+"
  runeColortType: Sky
```

### 4.3 Gear
```yaml
itemId: "gear:boots_iron_01"
rarity: Common
lootValue: 12
gearData:
  slotType: Boots
  tags: ["gear","metal","common"]
  runeSocketType: None
```

---

## 5) Validation Checklist
- [ ] `itemId` matches `category:item` and parses.
- [ ] Only the **relevant** data block is populated (others are null).
- [ ] `lootValue` non-negative; roughly aligned with rarity budget.
- [ ] `GearData.tags` include appropriate filters (drop/biome/faction).

---

## 6) Common Pitfalls
- Filling multiple type blocks at once (will be cleared).
- Missing tags for gear → weak filters in drops/research.
- Cryptic IDs; keep structure human-readable.

---

## 7) Related
- `ItemDef_SO.cs` (source) — **CHAL.Data.ItemDef**.  fileciteturn1file4
- Drop tables / registries (project-specific).
