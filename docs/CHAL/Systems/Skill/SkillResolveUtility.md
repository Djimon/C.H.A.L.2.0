# Assets/src/Systems/Skills/SkillResolveUtility.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillResolveUtility.cs`._

# Purpose
- Provides utility methods for resolving skills in the game, including building tag contexts and resolving base skill properties.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public static class `SkillResolveUtility`
    - Public methods:
      - `BuildTagContext(SkillModuleDef module, CoreType core)`: Builds a `TagContext` based on the provided skill module and core type.
      - `ResolvedSkill ResolveBaseSkill(SkillModuleDef module, int skilltier, CoreType core)`: Resolves the base skill properties and returns a `ResolvedSkill` object.
      - `float ResolveRangeAsFloat(SkillRange range)`: Converts a `SkillRange` enum to its corresponding float value based on game configuration.

# Key Behavior & Side Effects
- `BuildTagContext` aggregates delivery and mechanic tags from the skill module and core type.
- `ResolveBaseSkill` applies checks for null modules and minimum required tiers, constructs a `ResolvedSkill` object, and builds a tag context.
- `ResolveRangeAsFloat` retrieves range values from the game configuration based on the provided `SkillRange`.

# Constraints & Failure Modes
- `BuildTagContext` and `ResolveBaseSkill` handle null checks for the module.
- `ResolveBaseSkill` checks if the skill tier is less than the minimum required tier and logs a warning.
- `ResolveRangeAsFloat` returns `0f` for unrecognized `SkillRange` values.

# Example
```csharp
var skillModule = new SkillModuleDef();
var resolvedSkill = SkillResolveUtility.ResolveBaseSkill(skillModule, 1, CoreType.Basic);
```

# Unknowns
- The definitions and structures of `SkillModuleDef`, `CoreType`, `ResolvedSkill`, `TagContext`, `DamageEntry`, and `SkillRange` are not provided in this file.
