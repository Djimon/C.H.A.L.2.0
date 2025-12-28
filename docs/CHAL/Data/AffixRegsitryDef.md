# Assets/src/Data/Defs/AffixRegsitryDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/AffixRegsitryDef.cs`._

# Purpose
- Defines the `AffixRegistryDef` class as a ScriptableObject for managing a list of affixes.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public sealed class AffixRegistryDef : ScriptableObject`
    - Public fields/properties:
      - `public List<AffixDef> Affixes`: List of affix definitions.
    - Public methods:
      - `private void OnValidate()`: Validates the `Affixes` list by removing null entries and duplicates based on `AffixId`.

# Key Behavior & Side Effects
- On validation, the method removes null entries from the `Affixes` list.
- It trims duplicates by `AffixId`, keeping the first occurrence and logging warnings for duplicates and empty IDs.

# Constraints & Failure Modes
- If `Affixes` is null, the method exits early without processing.
- Logs warnings for affixes with empty IDs and for duplicate IDs detected during validation.

# Example
```csharp
// Example of creating an AffixRegistryDef asset
var affixRegistry = ScriptableObject.CreateInstance<AffixRegistryDef>();
affixRegistry.Affixes.Add(new AffixDef { AffixId = "unique_id_1" });
```

# Unknowns
- The structure and properties of `AffixDef` are not defined in this file.
