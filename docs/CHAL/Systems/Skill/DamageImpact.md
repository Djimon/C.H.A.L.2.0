# CHAL.Systems.Skill.DamageImpact

_Automatically generated/updated from `Assets/src/Systems/Skills/Effekte/DamageImpact.cs`._

1) Purpose
- Defines a ScriptableObject DamageImpact that applies multiple damage entries as part of a skill effect.
- Holds a list of damage entries (Damages) to apply (elemental/physical).
- Exposes a Unity editor asset creation entry via CreateAssetMenu.

2) Public API
- Namespace/module
  - CHAL.Systems.Skill
- Types
  - public class DamageImpact : SkillImpactBase
    - Public fields/properties
      - public List<DamageEntry> Damages
        - Tooltip: "Damage entries applied by this effect (elemental/physical)."
    - Public methods
      - public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        - For each entry in Damages:
          - var dmgType = damage.DmgType
          - var finalDamage = skill.Damage * damage.DmgMultiplier
          - DebugManager.Log($"[Effect] {source} deals {finalDamage} {dmgType} on {target}", DebugManager.EDebugLevel.Test, "Skill")
          - target.TakeDamage(finalDamage, dmgType)

3) Key Behavior & Side Effects
- Asset creation metadata
  - CreateAssetMenu(fileName = "DamageImpact", menuName = "Skills/Impact/Damage")
- Apply flow
  - Iterates over Damages
  - Computes finalDamage = skill.Damage * damage.DmgMultiplier
  - Logs damage event via DebugManager.Log
  - Applies damage to target via target.TakeDamage(finalDamage, dmgType)

4) Constraints & Failure Modes
- No null checks for Damages; potential NullReferenceException if Damages is null.
- No explicit validation of DmgMultiplier or finalDamage values.
- Relies on external types: DamageEntry, SkillInstance, EffectReceiver, DebugManager, etc. not defined in this file.
- Apply executes synchronously; no asynchronous handling.
- Possible null source/target would cause runtime errors if not validated upstream.

5) Example
- (No explicit example derivable from this file without external type definitions; omitted.)

6) Unknowns
- Exact structure of DamageEntry (beyond DmgType and DmgMultiplier usage).
- Definitions/behaviors of SkillImpactBase, SkillInstance, EffectReceiver.
- Exact DamageType enum or type used by DmgType.
- Behavior of DebugManager.Log and its DebugLevel enum values.
- Lifecycle and serialization details for the ScriptableObject asset beyond [CreateAssetMenu].
