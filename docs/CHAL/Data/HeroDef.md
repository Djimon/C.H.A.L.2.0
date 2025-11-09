# Assets/src/Data/Defs/HeroDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/HeroDef.cs`._

# Purpose
- Defines a hero definition in the game, including identity, lore, and gameplay attributes.

# Public API
- Namespace: CHAL.Data
- Types
  - public class HeroDef : ScriptableObject
    - Public fields/properties:
      - string HeroId: Unique identifier for the hero (e.g., "Hero_Piercer_01").
      - string DisplayName: Display name of the hero (e.g., "Kaelen the Piercer").
      - string Lore: Flavour text or story associated with the hero.
      - ArchetypeDef Archetype: Reference to the hero's archetype definition.
      - int BaseHealth: Base health of the hero (default is 100).
      - float BaseDamage: Base damage dealt by the hero (default is 10f).
      - float BaseMovementSpeed: Base movement speed of the hero (default is 2f).
      - float sightRange: Range at which the hero can see (default is 20f).
      - Sprite Portrait: Visual representation of the hero.
      - GameObject Prefab: 3D or 2D model for in-game representation.
      - AudioClip VoiceSample: Optional voice sample for the hero.

# Key Behavior & Side Effects
- None explicitly defined in the provided code.

# Constraints & Failure Modes
- None explicitly defined in the provided code.

# Example
- 
```csharp
HeroDef hero = ScriptableObject.CreateInstance<HeroDef>();
hero.HeroId = "Hero_Piercer_01";
hero.DisplayName = "Kaelen the Piercer";
hero.BaseHealth = 100;
```

# Unknowns
- None.

