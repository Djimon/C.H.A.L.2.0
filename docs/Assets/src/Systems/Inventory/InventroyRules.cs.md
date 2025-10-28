# Assets/src/Systems/Inventory/InventroyRules.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines inventory rules for maximum stack sizes and slot counts based on item prefixes.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public class InventoryRules`
    - Public methods:
      - `public static int GetMaxStack(string prefix)`: Returns the maximum stack size for the given item prefix.
      - `public static int GetMaxSlots(string prefix)`: Returns the maximum number of slots for the given item prefix.

# Key Behavior & Side Effects
- `GetMaxStack` and `GetMaxSlots` use a switch expression to determine values based on the provided prefix.
- Default values are returned for unrecognized prefixes.

# Constraints & Failure Modes
- The methods do not handle null or empty strings; behavior is undefined for such inputs.
- No threading or async considerations are present.

# Example
```csharp
int maxStack = InventoryRules.GetMaxStack("part"); // Returns 250
int maxSlots = InventoryRules.GetMaxSlots("rune"); // Returns 20
```

# Unknowns
- None.
```
