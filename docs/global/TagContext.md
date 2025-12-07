# Assets/src/Systems/Skills/TagContext.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/TagContext.cs`._

# Purpose
- Defines the `TagContext` class, which encapsulates skill-related tags and types.

# Public API
- Namespace/module: None
- Types
  - public sealed class `TagContext`
    - Public fields/properties:
      - `SkillType`: Nullable type of the skill.
      - `DeliveryTags`: Read-only list of delivery tags.
      - `MechanicTags`: Read-only list of mechanic tags.
      - `DamageType`: Nullable type of damage.
    - Public methods:
      - `GetModifierTags()`: Returns a collection of modifier tags.
      - `GetUiTags()`: Returns a collection of UI tags.
      - `static TagContext From(SkillType? type, IEnumerable<SkillDeliveryTag> delivery, IEnumerable<SkillMechanicTag> mechanics, DamageType? damageType)`: Factory method to create a `TagContext` instance.

# Key Behavior & Side Effects
- `GetModifierTags()` and `GetUiTags()` build and return collections of tags based on the current state of the `TagContext`.
- The `From` method allows for convenient creation of a `TagContext` instance from various input types.

# Constraints & Failure Modes
- If `deliveryTags` or `mechanicTags` are null, they are replaced with empty arrays.
- The `BuildModifierTags` and `BuildUiTags` methods utilize `HashSet` to ensure unique tags.

# Example
```csharp
var tagContext = TagContext.From(SkillType.Fire, new[] { SkillDeliveryTag.Projectile }, new[] { SkillMechanicTag.Explosion }, DamageType.Fire);
var modifierTags = tagContext.GetModifierTags();
var uiTags = tagContext.GetUiTags();
```

# Unknowns
- None.
