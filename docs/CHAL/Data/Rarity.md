# CHAL.Data.Rarity

_Automatically generated/updated from `Assets/src/Data/Enums/ItemRarity.cs`._

```text
1) Purpose
- Define item rarity levels via the Rarity enum (including an explicit unknown sentinel).
- Provide a color mapper (RarityColors) to translate a Rarity into a Unity Color.
- Namespace: CHAL.Data

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public enum Rarity
    - unknown = -1
    - Common
    - Uncommon
    - Rare
    - Epic
    - Legendary
    - Mythic
    - Holy
    - Daemonic
  - public static class RarityColors
    - private static readonly Dictionary<Rarity, Color> _map
      - Mapping from Rarity to Color:
        - { Rarity.unknown, Color.gray }
        - { Rarity.Common, Color.white }
        - { Rarity.Uncommon, new Color(138f/255f, 165f/255f, 230f/255f) }
        - { Rarity.Rare,     new Color(47f/255f, 181f/255f, 105f/255f) }
        - { Rarity.Epic,     new Color(217f/255f, 175f/255f, 61f/255f) }
        - { Rarity.Legendary,new Color(110f/255f,  38f/255f, 212f/255f) }
        - { Rarity.Mythic,   new Color(191f/255f,  63f/255f, 178f/255f) }
        - { Rarity.Holy,     new Color(172f/255f, 232f/255f, 217f/255f) }
        - { Rarity.Daemonic, new Color( 87f/255f,  12f/255f,  27f/255f) }
    - public static Color Get(Rarity rarity)
      - Returns the mapped Color for the given rarity, or Color.white if not found

3) Key Behavior & Side Effects
- RarityColors.Get(rarity) resolves a color via _map.TryGetValue(rarity, out var c) and returns:
  - the mapped color if present
  - Color.white if the rarity key is absent
- No side effects; reads from a static, private mapping; no mutations.

4) Constraints & Failure Modes
- Null handling: Rarity is an enum; cannot be null.
- Unknown keys: If a rarity is not present in _map, returns Color.white.
- Threading: _map is a private static readonly dictionary; Get performs a read-only lookup; safe for concurrent use (no mutation).

5) Example
```csharp
using CHAL.Data;

var commonColor = RarityColors.Get(Rarity.Common);
// commonColor is the Color mapped for Rarity.Common
```

6) Unknowns
- External usage or extension points beyond this file (e.g., additional rarities or dynamic color mappings) are not defined here.
- Any runtime behavior outside the defined enum-to-color mapping is not specified.
