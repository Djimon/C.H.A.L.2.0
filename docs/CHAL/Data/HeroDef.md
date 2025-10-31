# CHAL.Data.HeroDef

_Automatically generated/updated from `Assets/src/Data/Defs/HeroDef.cs`._

# Purpose
- Defines the `HeroDef` class as a ScriptableObject for hero data in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `HeroDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string HeroId` - Unique identifier for the hero.
      - `string DisplayName` - Name displayed for the hero.
      - `string Lore` - Background story or flavor text for the hero.
      - `ArchetypeDef Archetype` - Reference to the hero's archetype definition.
      - `int BaseHealth` - Initial health value of the hero (default 100).
      - `float BaseDamage` - Initial damage value of the hero (default 10.0).
      - `float BaseMovementSpeed` - Initial movement speed of the hero (default 2.0).
      - `float sightRange` - Range at which the hero can see (default 20.0).
      - `Sprite Portrait` - Visual representation of the hero.
      - `GameObject Prefab` - 3D or 2D model for in-game representation.
      - `AudioClip VoiceSample` - Optional audio clip for the hero's voice.

# Key Behavior & Side Effects
- The `HeroDef` class is used to define properties of a hero, which can be instantiated as assets in Unity.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid references for `ArchetypeDef`, `Sprite`, `GameObject`, and `AudioClip`.

# Example
```csharp
HeroDef hero = ScriptableObject.CreateInstance<HeroDef>();
hero.HeroId = "Hero_Piercer_01";
hero.DisplayName = "Kaelen the Piercer";
hero.BaseHealth = 150;
```

# Unknowns
- No information on the `ArchetypeDef` class or its properties.

