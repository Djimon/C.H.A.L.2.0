# CHAL.Data.MapDifficulty

_Automatically generated/updated from `Assets/src/Data/Enums/MapDifficulty.cs`._

```text
Purpose
- Defines a serializable public enum MapDifficulty in CHAL.Data.
- Declares four difficulty levels: Stable, Strained, Volatile, Chaos.
- Notes potential values Reactive and Oblivion are commented out.

Public API
- Namespace/module: CHAL.Data
- Types
  - public enum MapDifficulty
    - Members:
      - Stable
      - Strained
      - Volatile
      - Chaos
    - Public methods: none
    - Attributes: [Serializable] on the enum

Key Behavior & Side Effects
- No runtime behavior or side effects; this file only defines a data type.

Constraints & Failure Modes
- Serializable attribute indicates it can be serialized by serializers that respect [Serializable].
- No guards, null handling, or asynchronous behavior defined.

Example
```csharp
using CHAL.Data;

MapDifficulty current = MapDifficulty.Stable;
```

Unknowns
- Numeric underlying values for members are not explicit (default C# behavior not stated here).
- Intent for commented-out values Reactive and Oblivion is not determined from this file.
- Any project-specific serialization behavior beyond [Serializable] is not specified.
```
