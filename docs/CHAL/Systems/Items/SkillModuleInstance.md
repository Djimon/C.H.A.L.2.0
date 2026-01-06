# Assets/src/Systems/Items/SkillModules/SkillModuleInstance.cs

_Automatically generated/updated from `Assets/src/Systems/Items/SkillModules/SkillModuleInstance.cs`._

# Purpose
- Defines the `SkillModuleInstance` class, which represents a persisted payload for a specific variant of a skill module.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - **public class SkillModuleInstance**
    - Public fields/properties:
      - `string instanceId`: deterministic variant key (NOT a GUID)
      - `string moduleItemId`: e.g. "module:fireball"
      - `string skillId`: e.g. "fireball"
      - `int frameTier`: 1..N
      - `CoreType coreType`: Kinetic/Blazing/...
    - Public methods:
      - `static string BuildVariantKey(string moduleItemId, int frameTier, CoreType coreType)`: returns a stable, readable variant key for JSON.
      - `static SkillModuleInstance Create(string moduleItemId, string skillId, int frameTier, CoreType coreType)`: creates a new `SkillModuleInstance` with validated `frameTier` and generated `instanceId`.

# Key Behavior & Side Effects
- The `Create` method ensures that `frameTier` is at least 1.
- The `BuildVariantKey` method generates a deterministic key for the instance based on the provided parameters.

# Constraints & Failure Modes
- The `frameTier` is clamped to a minimum of 1 in the `Create` method.
- No explicit threading or async handling is present.

# Example
```csharp
var skillModule = SkillModuleInstance.Create("module:fireball", "fireball", 2, CoreType.Kinetic);
```

# Unknowns
- No information on the `CoreType` enumeration or its possible values.
