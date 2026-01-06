using CHAL.Data;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillModuleDef))]
/// <summary>
/// Custom editor for SkillData objects in the Unity Inspector.
/// </summary>
public class SkillDataEditor : Editor
{
/// <summary>
/// Draws the custom inspector GUI for the SkillData object.
/// </summary>
/// <param name="target">The target object being inspected.</param>
/// <returns>None.</returns>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SkillModuleDef data = (SkillModuleDef)target;

        // Identity
        EditorGUILayout.LabelField("Identity", EditorStyles.boldLabel);
        data.SkillId = EditorGUILayout.TextField("Skill ID", data.SkillId);
        data.DisplayName = EditorGUILayout.TextField("Display Name", data.DisplayName);
        data.AttributeAffinity = (HeroAttribs)EditorGUILayout.EnumPopup("MainScalingAttribute", data.AttributeAffinity);

        // Core
        EditorGUILayout.LabelField("Core", EditorStyles.boldLabel);
        data.BaseDamage = EditorGUILayout.FloatField("Base Damage", data.BaseDamage);
        data.Cooldown = EditorGUILayout.FloatField("Cooldown", data.Cooldown);
        data.CastTime = EditorGUILayout.FloatField("Cast Time", data.CastTime);
        data.damageAttributeScalingFactor = EditorGUILayout.FloatField("AttributeScalingFactor", data.damageAttributeScalingFactor);
        data.minRequiredTier = EditorGUILayout.IntField("minSkillFrameTier", data.minRequiredTier);

        //core ingredients
        EditorGUILayout.LabelField("ModuleCore", EditorStyles.boldLabel);
        data.BaseDamageType = (DamageType)EditorGUILayout.EnumPopup("Base Damage Type", data.BaseDamageType);
        data.defaultCore = (CoreType)EditorGUILayout.EnumPopup("defaultCoreType", data.defaultCore);
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SkillModuleDef.changeCoreTypesAllowed)), true); //"changeCoreTypesAllowed"

        EditorGUILayout.Space();

        // Classification
        EditorGUILayout.LabelField("Skill Classification", EditorStyles.boldLabel);
        data.SkillType = (SkillType)EditorGUILayout.EnumPopup("Skill Type", data.SkillType);
        data.Range = (SkillRange)EditorGUILayout.EnumPopup("Range", data.Range);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Additional Flags", EditorStyles.boldLabel);
        data.isProjectile = EditorGUILayout.Toggle(new GUIContent("Projectile?", "Also unlock projectile fields for this skill."), data.isProjectile);
        data.isAoE = EditorGUILayout.Toggle(new GUIContent("AoE?", "Also unlock AoE fields for this skill."), data.isAoE);
        data.hasDuration = EditorGUILayout.Toggle(new GUIContent("Has Duration?", "Also unlock duration fields for this skill."), data.hasDuration);

        EditorGUILayout.Space();

        // Composition (fields depend on SkillType)
        EditorGUILayout.LabelField("Composition", EditorStyles.boldLabel);
        bool wantsProjectile = (data.SkillType == SkillType.Ranged) || data.isProjectile;
        bool wantsAoE = (data.SkillType == SkillType.Spell) || data.isAoE;
        bool wantsDuration = (data.SkillType == SkillType.Spell || data.SkillType == SkillType.Summon) || data.hasDuration;

        EditorGUILayout.Space();
        if (wantsProjectile)
        {
            data.ProjectileSpeed = EditorGUILayout.FloatField("Projectile Speed", data.ProjectileSpeed);
            data.ProjectileCount = EditorGUILayout.IntField("Projectile Count", data.ProjectileCount);
        }

        if (wantsAoE)
        {
            data.Radius = EditorGUILayout.FloatField("AoE Radius", data.Radius);
        }

        if (wantsDuration)
        {
            data.Duration = EditorGUILayout.FloatField("Duration", data.Duration);
        }

        EditorGUILayout.Space();

        // Hooks / Impacts (wieder sichtbar)
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SkillModuleDef.OnCastImpact)), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SkillModuleDef.OnHitImpact)), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SkillModuleDef.OnEndImpact)), true);

        EditorGUILayout.Space();

        // Meta-Tags (optional, aber sinnvoll)
        EditorGUILayout.LabelField("Meta / Tags", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SkillModuleDef.DeliveryTags)), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SkillModuleDef.MechanicTags)), true);

        EditorGUILayout.Space();

        // Presentation
        EditorGUILayout.LabelField("Presentation", EditorStyles.boldLabel);

        // Filter AnimationType dropdown depending on SkillType
        AnimationType chosen = data.animationType;
        switch (data.SkillType)
        {
            case SkillType.Melee:
                chosen = (AnimationType)EditorGUILayout.EnumPopup("Animation Type",
                    FilterAnimationType(data.animationType,
                        AnimationType.MeleeSwing, AnimationType.MeleeThrust, AnimationType.Defend));
                break;

            case SkillType.Ranged:
                chosen = (AnimationType)EditorGUILayout.EnumPopup("Animation Type",
                    FilterAnimationType(data.animationType,
                        AnimationType.Shoot, AnimationType.Throw));
                break;

            case SkillType.Spell:
            case SkillType.Summon:
                chosen = (AnimationType)EditorGUILayout.EnumPopup("Animation Type",
                    FilterAnimationType(data.animationType, AnimationType.Cast));
                break;
        }
        data.animationType = chosen;

        data.vfxPrefab = (GameObject)EditorGUILayout.ObjectField("VFX Prefab", data.vfxPrefab, typeof(GameObject), false);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(data);
        }

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Ensures the current animationType is within the allowed set.
    /// If not, fallback to the first allowed type.
    /// </summary>
    private AnimationType FilterAnimationType(AnimationType current, params AnimationType[] allowed)
    {
        foreach (var a in allowed)
        {
            if (current == a)
                return current;
        }
        return allowed.Length > 0 ? allowed[0] : AnimationType.None;
    }
}
