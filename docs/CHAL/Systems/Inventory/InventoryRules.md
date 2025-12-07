# Assets/src/Systems/Inventory/InventroyRules.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/InventroyRules.cs`._

# Purpose
- Provides rules and methods for managing inventory items.

# Public API
- Namespace: CHAL.Systems.Inventory
- Types
  - public class InventoryRules
    - Public methods:
      - static int GetMaxStack(string prefix) : Returns the maximum stack size for the specified prefix.
      - static int GetMaxSlots(string prefix) : Returns the maximum number of slots for the specified prefix.

# Key Behavior & Side Effects
- `GetMaxStack` and `GetMaxSlots` methods determine maximum stack sizes and slots based on the provided prefix using a switch expression.

# Constraints & Failure Modes
- The methods return default values if the prefix does not match any specified cases.

# Example
```csharp
int maxStack = InventoryRules.GetMaxStack("part"); // Returns 250
int maxSlots = InventoryRules.GetMaxSlots("rune"); // Returns 20
```

# Unknowns
- None.
