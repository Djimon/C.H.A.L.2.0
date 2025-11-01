# global.SkillDataEditor

_Automatically generated/updated from `Assets/src/Editor/SkillDataEditor.cs`._

```text
1) Purpose
- Defines a Unity Editor custom inspector for SkillData.
- Renders and edits SkillData fields across sections: Identity, Core, Classification, Additional Flags, Composition, Hooks/Impacts, Presentation.
- Enforces allowed AnimationType values per SkillType via FilterAnimationType.

2) Public API
- Type: public class SkillDataEditor : Editor
  - Public methods
    - public override void OnInspectorGUI()

Note: No public fields/properties are exposed by this class.

3) Key Behavior & Side Effects
- Inspector lifecycle
  - OnInspectorGUI starts with serializedObject.Update() and cast target to SkillData.
  - Identity section edits: SkillId (TextField) and DisplayName (TextField) directly on data.
  - Core section edits: DamageTypes (serialized property field, recursive), BaseDamage (float), Cooldown (float), CastTime (float).
  - Composition gating (fields depend on SkillType and flags):
    - Wants flags
      - wantsProjectile = (SkillType == Projectile) OR isProjectile
      - wantsAoE = (SkillType == Spell) OR isAoE
      - wantsDuration = (SkillType == Spell or Summon) OR hasDuration
    - If wantsProjectile: ProjectileSpeed (float), ProjectileCount (int)
    - If wantsAoE: AoERadius (float)
    - If wantsDuration: Duration (float)
  - Hooks / Impacts section
    - OnCastImpactEffects (serialized property field, recursive)
    - OnHitImpactEffects (serialized property field, recursive)
  - Presentation section
    - Animation Type dropdown is constrained by SkillType:
      - Melee: allowed {MeleeSwing, MeleeThrust, Defend}
      - Projectile: allowed {Shoot, Throw}
      - Spell / Summon: allowed {Cast}
    - The current value is filtered via FilterAnimationType and assigned back to data.animationType
    - VFX Prefab: vfxPrefab (GameObject) via ObjectField
  - Dirty/Apply
    - If GUI.changed, call EditorUtility.SetDirty(data)
    - serializedObject.ApplyModifiedProperties() at end
- Filtering behavior
  - FilterAnimationType(current, allowed...) returns:
    - current if it matches any allowed
    - otherwise first allowed value, or AnimationType.None if none allowed
- Data binding and surface
  - Most fields are updated directly on SkillData (not via serialized properties), except for certain serialized fields found with serializedObject.FindProperty.
  - Uses labels/spacing to organize sections in inspector.

4) Constraints & Failure Modes
- Guard behavior
  - FilterAnimationType returns AnimationType.None if there are no allowed entries.
- Serialization side-effects
  - Uses serializedObject.Update/ApplyModifiedProperties to synchronize serialized fields.
  - GUI.changed triggers EditorUtility.SetDirty on SkillData (explicit persistence hint for the editor).
- Runtime types assumed
  - SkillData, SkillType, AnimationType, Range, DamageTypes, OnCastImpactEffects, OnHitImpactEffects, etc. are defined elsewhere; this file relies on their existence and names.
- Editor-only behavior
  - This is an editor script bound to SkillData via CustomEditor; runs only in Unity Editor.

5) Example
- Not derivable from this file alone (no runnable usage example provided).

6) Unknowns
- Exact definitions and values of:
  - SkillData fields: SkillId, DisplayName, BaseDamage, Cooldown, CastTime, SkillType, Range, isProjectile, isAoE, hasDuration, ProjectileSpeed, ProjectileCount, AoERadius, Duration, animationType, vfxPrefab, DamageTypes, OnCastImpactEffects, OnHitImpactEffects.
  - Enumerations: SkillType, AnimationType, and possible Range values.
  - Behavior/effects of editing these fields at runtime beyond editor UI.
```
