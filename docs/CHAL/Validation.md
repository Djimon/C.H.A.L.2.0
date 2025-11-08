# ItemRegistry: Reports & CSV Schemas

## Report List
- `missing_items.csv` — references without matching `itemId`
- `duplicate_ids.csv` — collisions
- `recipe_outputs_missing.csv` — recipes with missing output
- `affix_conflicts.csv` — inconsistent tier/implicit assignment

## CSV Schemas

### missing_items.csv
| referrer_type | referrer_id         | missing_item_id        | path                          |
|---------------|---------------------|------------------------|-------------------------------|
| recipe        | craft_boots_iron_01 | gear_boots_iron_01     | data/Recipes/craft_boots...   |

### recipe_outputs_missing.csv
| recipe_id           | output_item_id | hint                          |
|---------------------|----------------|--------------------------------|
| craft_boots_iron_01 | gear_boots...  | ItemRegistry add or rename?    |

## Storage Location
`/Reports/Validation/YYYY-MM-DD_HHMM/`

## Ingest Process (for BI/Spreadsheets)
1. Select folder
2. Load CSVs
3. Pivot by `referrer_type/id` to find hotspots
