# CHAL.Data.HeroDef

_Automatically generated/updated from `Assets/src/Data/Defs/HeroDef.cs`._

# Purpose
- Defines a hero definition in the game, including identity, lore, and gameplay attributes.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `HeroDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string HeroId` - Unique identifier for the hero.
      - `string DisplayName` - Name displayed for the hero.
      - `string Lore` - Flavour text or story for the hero.
      - `ArchetypeDef Archetype` - Reference to the hero's archetype definition.
      - `int BaseHealth` - The base health of the hero (default is 100).
      - `float BaseDamage` - The base damage dealt by the hero (default is 10f).
      - `float BaseMovementSpeed` - The base movement speed of the hero (default is 2f).
      - `float sightRange` - The range at which the hero can see (default is 20f).
      - `Sprite Portrait` - Visual representation of the hero.
      - `GameObject Prefab` - 3D or 2D model for in-game representation.
      - `AudioClip VoiceSample` - Optional audio clip for the hero's voice sample.

# Key Behavior & Side Effects
- None explicitly defined in the code.

# Constraints & Failure Modes
- None explicitly defined in the code.

# Example
```csharp
HeroDef hero = ScriptableObject.CreateInstance<HeroDef>();
hero.HeroId = "Hero_Piercer_01";
hero.DisplayName = "Kaelen the Piercer";
hero.BaseHealth = 150;
```

# Unknowns
- None.
