# CHAL.Systems.Skill.SkillImpactBase

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillImpactBase.cs`._

1) Purpose
- Defines an abstract ScriptableObject base for skill impact effects in the CHAL.Systems.Skill namespace.
- Exposes a serializable public field EffectId (Tooltip: "Optional: unique identifier for debugging or balancing.").
- Declares an abstract Apply method to be implemented by derived skill-impact classes.

2) Public API
- Namespace/module
  - CHAL.Systems.Skill
- Types
  - public abstract class SkillImpactBase : ScriptableObject
    - Public fields/properties
      - public string EffectId; // Optional identifier for debugging/balancing
    - Public methods
      - public abstract void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target);
        - Executes the effect from source to target (no implementation here; to be provided by derived classes)

3) Key Behavior & Side Effects
- No runtime behavior in this base class; only contract via abstract Apply.
- Derived classes implement Apply to apply an effect from a source to a target using a given SkillInstance context.
- EffectId is a metadata field intended for debugging or balancing; its usage is not defined in this file.

4) Constraints & Failure Modes
- EffectId is optional; no non-null enforcement in this class.
- No guards or error handling provided; behavior determined by derived implementations.
- This is a Unity ScriptableObject with [Serializable] attribute; relies on Unity serialization.

5) Example
- Not provided in this file.

6) Unknowns
- Definitions and namespaces of SkillInstance and EffectReceiver beyond their usage here.
- Concrete derived implementations of SkillImpactBase and their exact effect semantics.
- How EffectId is used at runtime (e.g., lookup, balancing, debugging workflows) since not defined here.
- Any threading, async, or lifecycle considerations for Apply beyond the abstract contract.
