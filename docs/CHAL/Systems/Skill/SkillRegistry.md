# Assets/src/Systems/Skills/SkillRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillRegistry.cs`._

# Purpose
- Defines a singleton `SkillRegistry` for managing skill definitions in the game.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public sealed class SkillRegistry : ScriptableObject`
    - Public fields/properties:
      - `public static SkillRegistry Instance`: Singleton instance of `SkillRegistry`.
    - Public methods:
      - `public void Reload()`: Reloads skill definitions from resources.
      - `public SkillModuleDef GetById(string skillId)`: Retrieves a skill definition by its ID.
      - `public bool TryGet(string skillId, out SkillModuleDef def)`: Attempts to get a skill definition by ID; returns success status.
      - `public IEnumerable<string> GetAllSkillIds()`: Returns all skill IDs.
      - `public IEnumerable<SkillModuleDef> GetAllSkills()`: Returns all skill definitions.

# Key Behavior & Side Effects
- `Reload()` clears existing skills and loads new definitions from the specified resources path.
- Logs warnings for invalid or duplicate skill IDs during the reload process.
- Logs the number of skills loaded after reloading.

# Constraints & Failure Modes
- Handles null or whitespace skill IDs by skipping those definitions.
- Skips duplicate skill IDs and logs a warning.
- The `EditorAutoReload` method automatically reloads skills when the editor is not in play mode.

# Example
```csharp
var skill = SkillRegistry.Instance.GetById("someSkillId");
if (skill != null)
{
    // Use the skill definition
}
```

# Unknowns
- The structure and properties of `SkillModuleDef` are not defined in this file.
- The exact behavior of `DebugManager` is not detailed in this file.

