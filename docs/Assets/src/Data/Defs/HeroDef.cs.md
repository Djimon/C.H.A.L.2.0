# Assets/src/Data/Defs/HeroDef.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `HeroDef` class as a ScriptableObject for hero data in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `HeroDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string HeroId` - Unique identifier for the hero.
      - `string DisplayName` - Name displayed for the hero.
      - `string Lore` - Flavor text or story for the hero.
      - `ArchetypeDef Archetype` - Reference to the hero's archetype.
      - `int BaseHealth` - Base health value of the hero (default 100).
      - `float BaseDamage` - Base damage value of the hero (default 10f).
      - `float BaseMovementSpeed` - Base movement speed of the hero (default 2f).
      - `float sightRange` - Range at which the hero can see (default 20f).
      - `Sprite Portrait` - Visual representation of the hero.
      - `GameObject Prefab` - 3D or 2D model for in-game representation.
      - `AudioClip VoiceSample` - Optional voice sample for the hero.

# Key Behavior & Side Effects
- None explicitly defined in the file.

# Constraints & Failure Modes
- None explicitly defined in the file.

# Example
```csharp
HeroDef hero = ScriptableObject.CreateInstance<HeroDef>();
hero.HeroId = "Hero_Piercer_01";
hero.DisplayName = "Kaelen the Piercer";
hero.BaseHealth = 150;
```

# Unknowns
- No information on how `ArchetypeDef` is defined or used.
```
