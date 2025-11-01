# CHAL.Systems.Inventory.InventoryDef

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryDef.cs`._

```text
1) Purpose
- Defines InventoryDef as a ScriptableObject asset in the CHAL.Systems.Inventory namespace.
- Exposes configuration data for inventory definitions used by the system (types, keys, sizing, and slot filtering).
- Provides an editor-friendly asset creation path via CreateAssetMenu.

2) Public API
- Namespace/module
  - CHAL.Systems.Inventory
- Types
  - public class InventoryDef : ScriptableObject
    - Public fields
      - public PlayerInventoryType TypeId
        - Inventory type identifier for this definition.
      - public string NameKey
        - Localization/key for display name.
      - [Min(1)] public int cols
        - Number of columns in the inventory grid (must be >= 1).
      - [Min(1)] public int rows
        - Number of rows in the inventory grid (must be >= 1).
      - public int defaultMaxStackPerSlot = 250
        - Default maximum items per slot (default 250).
      - public SlotFilter globalSlotFilter
        - Global filter applied to slots for this inventory.
- Attributes
  - [CreateAssetMenu(fileName = "Inventory Def", menuName = "Data/Inventory Def")]
    - Enables creating this asset from Unity’s assets menu.

3) Key Behavior & Side Effects
- No methods or runtime logic defined; this is a data asset.
- Asset creation is enabled via CreateAssetMenu with the specified default file name and menu path.
- Editor-enforced constraints:
  - cols and rows must be at least 1 due to [Min(1)] attributes.

4) Constraints & Failure Modes
- cols/rows must be >= 1 (editor validation via [Min(1)]).
- defaultMaxStackPerSlot has a default of 250; no explicit validation beyond type.
- All fields are public and serialized; relies on Unity serialization for persistence.
- No runtime guards or methods to mutate state within this file.

5) Example
- Not derivable from this file (no usage examples or methods provided).

6) Unknowns
- Definitions and meanings of:
  - PlayerInventoryType
  - SlotFilter
- How NameKey is used (localization or display) beyond being a string.
- How InventoryDef instances are consumed by other systems at runtime.
```
