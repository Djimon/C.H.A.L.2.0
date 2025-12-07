# Assets/src/Systems/Skills/TagContext.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/TagContext.cs`._

# Purpose
- Defines the `TagContext` class, which encapsulates skill-related tags and types.

# Public API
- Namespace/module: None
- Types
  - public sealed class `TagContext`
    - Public fields/properties:
      - `SkillType`: Nullable type representing the skill type.
      - `DeliveryTags`: Read-only list of delivery tags associated with the skill.
      - `MechanicTags`: Read-only list of mechanic tags associated with the skill.
      - `DamageType`: Nullable type representing the damage type.
    - Public methods:
      - `IReadOnlyCollection<string> GetModifierTags()`: Returns a collection of modifier tags built from the context.
      - `IReadOnlyCollection<string> GetUiTags()`: Returns a collection of UI tags built from the context.
      - `static TagContext From(SkillType? type, IEnumerable<SkillDeliveryTag> delivery, IEnumerable<SkillMechanicTag> mechanics, DamageType? damageType)`: Factory method to create a `TagContext` instance.

# Key Behavior & Side Effects
- `GetModifierTags()` and `GetUiTags()` methods build and return collections of tags based on the properties of the `TagContext` instance.
- The `From` method provides a convenient way to instantiate `TagContext` while handling nulls for delivery and mechanic tags.

# Constraints & Failure Modes
- `DeliveryTags` and `MechanicTags` are defaulted to empty arrays if null is provided during instantiation.
- The `BuildModifierTags` and `BuildUiTags` methods utilize `HashSet<string>` to ensure unique tags.

# Example
```csharp
var tagContext = TagContext.From(
    SkillType.Fire,
    new List<SkillDeliveryTag> { SkillDeliveryTag.Projectile },
    new List<SkillMechanicTag> { SkillMechanicTag.AreaOfEffect },
    DamageType.Fire
);

var modifierTags = tagContext.GetModifierTags();
var uiTags = tagContext.GetUiTags();
```

# Unknowns
- None.
