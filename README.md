# C.H.A.L.2

![Status](https://img.shields.io/badge/Status-Prototype-F5C542) ![Engine](https://img.shields.io/badge/Unity-Engine-000?logo=unity&logoColor=white) ![Language](https://img.shields.io/badge/C%23-Project-239120?logo=csharp&logoColor=white) [![Docs](https://img.shields.io/badge/Docs-Handbook-2B90D9)](docs/handbook/README.md) ![License](https://img.shields.io/badge/License-All%20rights%20reserved-lightgrey)

> Auto-battler / team-builder where you play as a team architect and build alchemist.

C.H.A.L.2 (Customized Hero Arena Looter - **working title**) is a systems-driven auto-battler prototype focused on data-first design. You plan builds, craft gear, and set up hero loadouts in the hub, then watch fully automated wave battles. Progress is long-term and permanent, driven by deterministic crafting and research-based unlocks instead of pure loot lottery.

## What Makes It Different
- Build-first, no APM: combat is fully automated; skill expression is in planning and team composition.
- Deterministic crafting with controlled RNG, not a loot lottery.
- Research-gated progression that unlocks systems, heroes, items, and map tiers.
- Long-term progress is preserved; failure mainly costs the current wave's rewards.

## Highlights
- Wave- and map-based encounters with difficulty tiers.
- Hero growth via levels, skill trees, and loadout synergies.
- Deterministic crafting, reforging, and item progression.
- Loot and rarity rules designed for predictable progress.
- Hub loop that alternates focus (planning) and reward (watching).
- Data-first configs and debug tooling for fast iteration.

## Systems At A Glance
```mermaid
flowchart LR
  subgraph Hub["Hub / Planning Phase (active)"]
    HP[Hero Loadout Panel]
    CR[Crafting & Reforge]
    INV[Inventory]
    MD[Map Device]
    HP <--> INV
    CR <--> INV
    HP --> MD    
  end

  subgraph Map["Map / Battle Phase (passive)"]
    MAPS[Maps & Difficulty]
    WAVES[Waves - Monster Spawns]
    COMBAT[Auto-Combat]
    LOOT[Loot]
  end

  subgraph XX["Progression"]
    XP[Player and Hero XP]
    CDX[Codex]
  end

  CDX2[Codex] 

  MD --> MAPS --> WAVES --> COMBAT --> LOOT --> INV
  COMBAT --> XP
  XP --> CDX

  CDX2 --> HP
  CDX2 --> CR

```
## Status
Current Status: ![Status](https://img.shields.io/badge/Status-Prototype-F5C542)
#### Roadmap
```mermaid
flowchart LR
  classDef prototype fill:#F5C542,stroke:#B58900,color:#111;
  classDef mvp fill:#E67E22,stroke:#A65100,color:#111;
  classDef alpha fill:#9B59B6,stroke:#6F3D86,color:#111;
  classDef early fill:#66C0F4,stroke:#2A7DB7,color:#111;
  classDef release fill:#2ECC71,stroke:#1E9C55,color:#111;

  P[Prototype]:::prototype --> M[MVP]:::mvp --> A[Alpha]:::alpha --> E[Early Access]:::early --> R[Release]:::release
```

## Docs
- [Handbook](docs/handbook/README.md)
- [API Docs](docs/CHAL/README.md)


## License
Proprietary. All rights reserved. See [LICENSE](LICENSE).
