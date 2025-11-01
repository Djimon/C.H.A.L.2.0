# CHAL.Systems.Inventory.InventoryRules

_Automatically generated/updated from `Assets/src/Systems/Inventory/InventroyRules.cs`._

```text
1) Purpose
- Defines InventoryRules with two static helpers to determine item/layer limits by prefix.
- GetMaxStack(string prefix) returns the max stack size for a given item prefix.
- GetMaxSlots(string prefix) returns the max number of slots for a given item prefix.
```

```text
2) Public API
- Namespace/module
  - CHAL.Systems.Inventory
- Types
  - public class InventoryRules
    - public static int GetMaxStack(string prefix)
      - Returns max stack size based on prefix:
        - "rune" => 1
        - "remains" => 10000
        - "part" => 250
        - "module" => 10
        - _ (default) => 100
      - Side effects: none
    - public static int GetMaxSlots(string prefix)
      - Returns max slots based on prefix:
        - "rune" => 20
        - "remain" => 12
        - "part" => 100
        - "module" => 30
        - _ (default) => 30
      - Side effects: none
```

```text
3) Key Behavior & Side Effects
- Pure helpers: no internal state mutation; always compute from input prefix.
- Mapping via switch expressions: returns constant limits for known prefixes; defaults apply for unknown prefixes.
- Potential inconsistency in prefixes:
  - GetMaxStack uses "remains" for one key.
  - GetMaxSlots uses "remain" (singular) for a potentially related key.
  - This discrepancy may affect expectations when prefixes are varied.
```

```text
4) Constraints & Failure Modes
- Null handling: prefix may be null; switch expression falls through to default (_) and returns the default value (100 for GetMaxStack; 30 for GetMaxSlots).
- No threading/async concerns; methods are static and stateless.
- No input validation beyond switch matching; behavior defined entirely by switch cases and defaults.
```

```text
5) Example
- Usage examples (minimal):
```csharp
using CHAL.Systems.Inventory;

int maxRuneStack = InventoryRules.GetMaxStack("rune");     // 1
int maxRuneSlots = InventoryRules.GetMaxSlots("rune");     // 20

int maxUnknownStack = InventoryRules.GetMaxStack("unknown"); // 100 (default)
int maxUnknownSlots = InventoryRules.GetMaxSlots("unknown"); // 30 (default)
```

```

```text
6) Unknowns
- No additional prefixes defined beyond those in the switch cases; behavior for unlisted prefixes is defaulted.
- The mismatch between "remains" (GetMaxStack) and "remain" (GetMaxSlots) could indicate a typo or intentional design; not determinable from this file alone.
- If prefixes are sourced externally, their exact set is not documented here.
```
