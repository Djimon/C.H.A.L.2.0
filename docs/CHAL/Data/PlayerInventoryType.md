# CHAL.Data.PlayerInventoryType

_Automatically generated/updated from `Assets/src/Data/Enums/InventroyType.cs`._

```text
1) Purpose
- Defines a serializable enum PlayerInventoryType in CHAL.Data.
- Enumerates inventory category values: all, Remains, Part, Rune, Module, Gear.
- Decorated with [Serializable] for Unity serialization.

2) Public API
- Namespace: CHAL.Data
- Types
  - public enum PlayerInventoryType
    - Attributes: [Serializable]
    - Public fields/properties: none
    - Public methods: none

3) Key Behavior & Side Effects
- No runtime behavior; this is a type definition only.
- No state changes or side effects on access.

4) Constraints & Failure Modes
- None explicit in this file.
- Serializable attribute implies Unity can serialize/inspector-bind this enum.

5) Example
```csharp
using CHAL.Data;

public class ExampleUsage
{
    public PlayerInventoryType Inventory = PlayerInventoryType.all;
}
```

6) Unknowns
- File name InventroyType.cs uses a different spelling than the enum name (Inventroy vs PlayerInventoryType).
- No usage context beyond this file; behavior in serialization or UI depends on consuming code.
```
