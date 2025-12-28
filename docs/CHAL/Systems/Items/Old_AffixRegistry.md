# Assets/src/Systems/Items/Gear/Old_AffixRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Items/Gear/Old_AffixRegistry.cs`._

# Purpose
- Defines the `AffixRegistry` class for managing affix definitions in a game system.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - `public sealed class AffixRegistry`
    - **Public Methods**
      - `public bool TryGet(string affixId, out AffixDef def)`
        - Returns `false` if the ID is null or empty; otherwise, returns `true` and outputs the corresponding `AffixDef`.
      - `public void GetCandidates(AffixFamily family, GearType gearType, List<AffixDef> buffer)`
        - Fills `buffer` with candidates for the specified family, filtered by allowed gear types; clears `buffer` first.

# Key Behavior & Side Effects
- Constructor validates the provided `AffixRegistryDef` and populates internal dictionaries.
- Logs errors for missing or empty definitions and warnings for duplicate affix IDs.
- `GetCandidates` clears the provided buffer before populating it.

# Constraints & Failure Modes
- Constructor handles null or empty `AffixRegistryDef` and its `Affixes` list.
- `TryGet` method handles null or empty `affixId`.
- `GetCandidates` checks for the existence of family keys and handles empty lists gracefully.

# Unknowns
- None.

