# CHAL.Data.ItemType

_Automatically generated/updated from `Assets/src/Data/Enums/ItemType.cs`._

1) Purpose
- Defines a public serializable enum ItemType in the CHAL.Data namespace.
- Enumerates item categories: Unknown, Remains, Part, Module, Gear, Rune.
- Unknown explicitly assigned to 0; others rely on default incremental values.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public enum ItemType
    - Unknown = 0
    - Remains // Resources
    - Part // Materials
    - Module // Skill
    - Gear
    - Rune

3) Key Behavior & Side Effects
- The enum is marked as [Serializable], enabling standard serialization.
- Underlying type is the default int; values increment from Unknown = 0.

4) Constraints & Failure Modes
- No nullability for enum values; use Unknown to represent an unset/unknown value.
- No methods or behavior defined; surface is limited to enum values.
- No threading/asynchrony details present in this file.

5) Example
```csharp
using CHAL.Data;

class Example {
    void Demo() {
        ItemType t = ItemType.Part;
        // use t as needed
    }
}
```

6) Unknowns
- Semantic meaning of each value beyond its name (e.g., exact use of Remains, Part, Module, etc.) is not defined here.
- Any Unity inspector/display specifics beyond [Serializable] are not specified.
- How this enum interacts with persistence/serialization across versions is not defined in this file.

