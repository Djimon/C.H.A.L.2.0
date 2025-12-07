# Assets/src/Systems/Skills/SkillResolveUtility.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillResolveUtility.cs`._

# Purpose
- Provides utility functions for resolving skills in the game, including building tag contexts and resolving base skill properties.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public static class `SkillResolveUtility`
    - Public methods:
      - `BuildTagContext(SkillModuleDef module, SkillFamilyDef family = null, ArchetypeModuleOverrideDef overrideDef = null)`: Builds a `TagContext` from the provided skill module, family, and override definitions.
      - `ResolvedSkill ResolveBaseSkill(SkillModuleDef module, ArchetypeModuleOverrideDef overrideDef, string archetypeId)`: Resolves and returns a `ResolvedSkill` object based on the provided skill module, override definitions, and archetype ID.
      - `float ResolveRangeAsFloat(SkillRange range)`: Converts a `SkillRange` enum to its corresponding float value based on game configuration.

# Key Behavior & Side Effects
- `BuildTagContext` aggregates delivery and mechanic tags from the skill module, family, and override definitions.
- `ResolveBaseSkill` applies overrides from the `ArchetypeModuleOverrideDef` to the base skill properties and constructs a `ResolvedSkill` object.
- `ResolveRangeAsFloat` retrieves range values from the game configuration based on the provided `SkillRange`.

# Constraints & Failure Modes
- `BuildTagContext` and `ResolveBaseSkill` handle null checks for optional parameters (`family`, `overrideDef`).
- `ResolveRangeAsFloat` returns `0f` for unrecognized `SkillRange` values.

# Example
```csharp
var skillModule = new SkillModuleDef();
var resolvedSkill = SkillResolveUtility.ResolveBaseSkill(skillModule, null, "archetypeId");
```

# Unknowns
- The exact structure and properties of `SkillModuleDef`, `SkillFamilyDef`, `ArchetypeModuleOverrideDef`, `ResolvedSkill`, `TagContext`, and `DamageEntry` cannot be determined from this file.

