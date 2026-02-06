# Design vs Implementation

This appendix summarizes where the GDD aligns with the current codebase. It is non-authoritative and may be outdated relative to shipped code.

## Implemented or Matching
- Shared elemental resistance in V1.
- Focus slot gating and claim behavior in Codex.
- Unlucky or pity protection and secret drops in loot.

## Not Yet Implemented
- Luck factor scaling in loot.
- MapDevice empowerment and live events.
- Orbit graph and socket orbits.
- Crafting hardening, attunement, and infusion tiers.
- Repeatable deeds.

## References
- `Systems/Unit/EffectReceiver.cs` (API: [EffectReceiver](../CHAL/Systems/Unit/EffectReceiver.md))
- `Systems/Research/CodexService.cs` (API: [CodexService](../CHAL/Systems/Research/CodexService.md))
- `Systems/Loot/UnluckyProtection.cs` (API: [UnluckyProtection](../CHAL/Systems/Loot/UnluckyProtection.md))
- `Systems/Loot/LootRulesService.cs` (API: [LootRulesService](../CHAL/Systems/Loot/LootRulesService.md))

## Source
- Alignment notes in `Assets/src/_notes/09_GDD_Alignment.md`.
