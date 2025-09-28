using CHAL.Data;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillData))]
public class SkillDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SkillData data = (SkillData)target;

        // Basis-Felder
        EditorGUILayout.LabelField("Identity", EditorStyles.boldLabel);
        data.SkillId = EditorGUILayout.TextField("Skill ID", data.SkillId);
        data.DisplayName = EditorGUILayout.TextField("Display Name", data.DisplayName);

        // Core Damage + Cooldown
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DamageTypes"), true);
        data.Cooldown = EditorGUILayout.FloatField("Cooldown", data.Cooldown);
        data.CastTime = EditorGUILayout.FloatField("Cast Time", data.CastTime);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Skil lType", EditorStyles.boldLabel);
        data.isProjectile = EditorGUILayout.Toggle("Projectile?", data.isProjectile);
        data.isAoE = EditorGUILayout.Toggle("AoE?", data.isAoE);
        data.hasDuration = EditorGUILayout.Toggle("Has Duration?", data.hasDuration);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Composition", EditorStyles.boldLabel);

        if (data.isProjectile)
        {
            data.Range = EditorGUILayout.FloatField("Range", data.Range);
            data.ProjectileSpeed = EditorGUILayout.FloatField("Projectile Speed", data.ProjectileSpeed);
            data.ProjectileCount = EditorGUILayout.IntField("Projectile Count", data.ProjectileCount);
        }

        if (data.isAoE)
        {
            data.AoERadius = EditorGUILayout.FloatField("AoE Radius", data.AoERadius);
        }

        if (data.hasDuration)
        {
            data.Duration = EditorGUILayout.FloatField("Duration", data.Duration);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Meta", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tags"), true);

        EditorGUILayout.LabelField("Presentation", EditorStyles.boldLabel);
        data.animationType = (AnimationType)EditorGUILayout.EnumPopup("Animation Type", data.animationType);
        data.vfxPrefab = (GameObject)EditorGUILayout.ObjectField("VFX Prefab", data.vfxPrefab, typeof(GameObject), false);

        serializedObject.ApplyModifiedProperties();
    }
}
