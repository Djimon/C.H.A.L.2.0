# CHAL.Data.HeroDef

_Automatically generated/updated from `Assets/src/Data/Defs/HeroDef.cs`._

1) Purpose
- Defines HeroDef as a ScriptableObject in the CHAL.Data namespace for hero configuration.
- Groups identity/flavor, gameplay, and visuals data for a hero asset.
- Enables editor asset creation via CreateAssetMenu (fileName = "Hero", menuName = "Data/Hero").

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public class HeroDef : ScriptableObject
    - Public fields
      - string HeroId
        - Identifier for the hero (e.g., "Hero_Piercer_01")
      - string DisplayName
        - Display name shown to players (e.g., "Kaelen the Piercer")
      - string Lore
        - Flavour text; [TextArea] serialized
      - ArchetypeDef Archetype
        - Reference to the hero's ArchetypeDef
      - int BaseHealth
        - Base health; default 100
      - float BaseDamage
        - Base damage; default 10f
      - float BaseMovementSpeed
        - Base movement speed; default 2f
      - float sightRange
        - Sight range; default 20f
      - Sprite Portrait
        - Portrait sprite
      - GameObject Prefab
        - In-game model (3D or 2D)
      - AudioClip VoiceSample
        - Optional voice sample

3) Key Behavior & Side Effects
- Not defined (no methods or runtime logic in this file).

4) Constraints & Failure Modes
- Not defined (no guards, threading, or error handling in this file).

5) Example
- Not provided / not derivable from this file.

6) Unknowns
- Definition and structure of ArchetypeDef (not provided here).
- How HeroDef assets are loaded, instantiated, or referenced at runtime.
- Nullability expectations for reference fields (Archetype, Portrait, Prefab, VoiceSample) cannot be inferred.
- Any runtime side effects or validation beyond data storage.

