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

# Key Behavior & Side Effects
- `TryGetImplicit` and `TryGetAffix` methods return false if the provided ID is null or empty.
- `GetImplicitCandidates` and `GetAffixCandidates` methods clear the provided buffer before populating it with valid candidates.
- The `BuildImplicits` and `BuildAffixes` methods log errors if the provided definitions are null or empty, and warnings for duplicate IDs.

# Constraints & Failure Modes
- The `GetImplicitCandidates` method expects a single pool flag for lookups, not a multi-flag mask.
- The `BuildImplicits` and `BuildAffixes` methods handle null checks for input definitions and log errors accordingly.

# Example
```csharp
var registry = new GearModRegistry(implicitDef, affixDef);
if (registry.TryGetImplicit("someImplicitId", out var implicitDef)) {
    // Use implicitDef
}
```

# Unknowns
- None.

