# CHAL.Systems.Skill.DamageImpact

_Automatically generated/updated from `Assets/src/Systems/Skills/Effekte/DamageImpact.cs`._

```text
1) Purpose
- Defines public class DamageImpact deriving from SkillImpactBase.
- Exposes public List<DamageEntry> Damages (with Tooltip: "Damage entries applied by this effect (elemental/physical).").
- Overrides Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target) to apply each damage entry to the target.

2) Public API
- Namespace/module: CHAL.Systems.Skill
- Types
  - public class DamageImpact : SkillImpactBase
    - Public fields/properties
      - public List<DamageEntry> Damages
        - Description: Damage entries applied by this effect (elemental/physical).
    - Public methods
      - public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        - Behavior: For each entry in Damages, computes finalDamage = skill.Damage * damage.DmgMultiplier, logs the action via DebugManager.Log, and calls target.TakeDamage(finalDamage, dmgType) where dmgType = damage.DmgType.

3) Key Behavior & Side Effects
- On Apply:
  - Iterates over each DamageEntry in Damages.
  - For each entry:
    - Reads dmgType from damage.DmgType.
    - Computes finalDamage = skill.Damage * damage.DmgMultiplier.
    - Logs: "[Effect] {source} deals {finalDamage} {dmgType} on {target}" with DebugManager.Log at Test level under category "Skill".
    - Invokes target.TakeDamage(finalDamage, dmgType).
- Result: Potentially multiple damage applications to the target within a single Apply call; each entry contributes independent damage of its type.

4) Constraints & Failure Modes
- Null handling:
  - Damages is used without null checks; null Damages would cause a NullReferenceException if not initialized.
  - Apply does not validate skill, source, or target for null; nulls could cause runtime errors.
- Empty/zero entries:
  - If Damages is empty, no damage is applied.
- Assumptions:
  - DamageEntry provides DmgType and DmgMultiplier fields.
  - DamageType type exists (for DmgType) and TakeDamage exists on EffectReceiver.
- Performance:
  - Each entry yields a separate log and damage application; multiple entries scale linearly with list size.
- Unity/editor note:
  - This class is decorated with CreateAssetMenu, enabling Unity Editor asset creation.

5) Example
// Minimal example of constructing a DamageImpact with two damage entries
var impact = ScriptableObject.CreateInstance<DamageImpact>();
impact.Damages = new List<DamageEntry>
{
    new DamageEntry { DmgType = default(DamageType), DmgMultiplier = 1.0f },
    new DamageEntry { DmgType = default(DamageType), DmgMultiplier = 0.5f }
};

6) Unknowns
- Definitions of DamageEntry, DamageType, SkillInstance, EffectReceiver, and DebugManager are not provided in this file.
- Behavior of SkillImpactBase and how Apply integrates with broader skill execution is not shown.
- Exact semantics of TakeDamage (e.g., death handling, immunities) are not defined here.
```
