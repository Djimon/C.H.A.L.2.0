# CHAL.Data.DamageType

_Automatically generated/updated from `Assets/src/Data/Enums/DamageType.cs`._

```text
1) Purpose
- Defines the DamageType enum under CHAL.Data.
- Enumerates damage categories: Physical, Fire, Cold, Poison, Arcane, Void, Holy.
- Includes a note that the enum is expandable.

2) Public API
- Namespace/module: CHAL.Data
- Types
  - public enum DamageType
    - Physical
    - Fire
    - Cold
    - Poison
    - Arcane
    - Void
    - Holy

3) Key Behavior & Side Effects
- No runtime behavior; only a type definition with enumerators.

4) Constraints & Failure Modes
- None explicit in this file (no guards, no methods, no async logic).

5) Example
```csharp
CHAL.Data.DamageType damage = CHAL.Data.DamageType.Fire;
```

6) Unknowns
- Underlying integral type not explicitly declared (default int in C# language rules).
- How this enum is serialized or mapped in broader systems.
- Any future additions beyond the current seven values.
- Any Unity-specific usage or attributes (not present in this file).
```
