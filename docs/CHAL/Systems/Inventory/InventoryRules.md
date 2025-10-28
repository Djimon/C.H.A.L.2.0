# CHAL.Systems.Inventory.InventoryRules

_Automatically generated/updated from `Assets/src/Systems/Inventory/InventroyRules.cs`._

# Purpose
- Defines inventory rules for maximum stack sizes and slot counts based on item prefixes.

# Public API
- Namespace: CHAL.Systems.Inventory
- Types
  - public class InventoryRules
    - Public methods:
      - static int GetMaxStack(string prefix) : returns maximum stack size for the given prefix.
      - static int GetMaxSlots(string prefix) : returns maximum slots for the given prefix.

# Key Behavior & Side Effects
- Uses switch expressions to determine maximum stack sizes and slots based on the provided prefix.

# Constraints & Failure Modes
- Default values are returned for unrecognized prefixes.

