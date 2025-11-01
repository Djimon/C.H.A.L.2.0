# CHAL.Systems.Skill.SkillImpactBase

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillImpactBase.cs`._

1) Purpose
- Defines an abstract SkillImpactBase class deriving from ScriptableObject in CHAL.Systems.Skill.
- Provides a serializable public field EffectId with a Tooltip for debugging or balancing.
- Declares an abstract Apply method to execute an effect from a source to a target.

2) Public API
- Namespace/Module: CHAL.Systems.Skill
- Types
  - public abstract class SkillImpactBase : ScriptableObject
    - Public fields/properties
      - public string EffectId
        - Tooltip: "Optional: unique identifier for debugging or balancing."
    - Public methods
      - public abstract void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        - Executes the effect from source to target (as described by the method summary)
        - No implementation in this base class

3) Key Behavior & Side Effects
- This class provides no concrete behavior; derived classes must implement Apply.
- Apply is documented to execute the effect from source to target, taking SkillInstance, source, and target as parameters.

4) Constraints & Failure Modes
- No input validation or guards are defined in this base class.
- Serialization is enabled via [Serializable] on the class; EffectId is a public field with a Tooltip attribute.
- No threading, async, or performance hints are specified.
- No default implementations; behavior depends on concrete subclasses.

5) Example
- Not provided (no concrete implementation shown in this file).

6) Unknowns
- Details of SkillInstance and EffectReceiver definitions (structure, members, nullability).
- How concrete SkillImpactBase implementations are found/created and used at runtime.
- Any additional constraints on EffectId (uniqueness, formatting) beyond the tooltip.
- Whether there are derived classes or how they are authored/managed (assets, scriptable objects, etc.).
