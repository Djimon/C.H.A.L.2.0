# Assets/src/Systems/Items/Gear/GearModRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Items/Gear/GearModRegistry.cs`._

# Purpose
- Defines a registry for gear modifications, including implicits and affixes, providing a unified API for access.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - `public sealed class GearModRegistry`
    - **Public Methods**
      - `bool TryGetImplicit(string implicitId, out ImplicitDef def)`
      - `bool TryGetAffix(string affixId, out AffixDef def)`
      - `void GetImplicitCandidates(GearType gearType, int poolMaskAsInt, int roleAsInt, List<ImplicitDef> buffer)`
      - `void GetAffixCandidates(AffixFamily family, GearType gearType, List<AffixDef> buffer)`
      - `void ExportModsCsv(string outputPath)`

# Key Behavior & Side Effects
- `TryGetImplicit` and `TryGetAffix` methods return false if the provided ID is null or empty.
- `GetImplicitCandidates` and `GetAffixCandidates` methods clear the provided buffer before populating it with valid candidates.
- The `BuildImplicitsFromResources` and `BuildAffixesFromResources` methods log errors if the provided definitions are null or empty, and warnings for duplicate IDs.
- `ExportModsCsv` logs a warning if the output path is null or empty and catches exceptions during file writing, logging the error.

# Constraints & Failure Modes
- The `GetImplicitCandidates` method expects a single pool flag for lookups, not a multi-flag mask.
- The `BuildImplicitsFromResources` and `BuildAffixesFromResources` methods handle null checks for input definitions and log errors accordingly.
- `ExportModsCsv` requires a valid output path and handles exceptions during file operations.

# Example
```csharp
var registry = new GearModRegistry();
if (registry.TryGetImplicit("someImplicitId", out var implicitDef)) {
    // Use implicitDef
}
```

# Unknowns
- None.
