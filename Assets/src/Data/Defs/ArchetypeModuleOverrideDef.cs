using CHAL.Data;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArchetypeModuleOverrideDef", menuName = "Skills/ArchetypeModuleOverride")]
public class ArchetypeModuleOverrideDef : ScriptableObject
{
    [Header("IDs")]
    [SerializeField] private string moduleId;
    [SerializeField] private string archetypeId; // oder CombatProfile-ID

    [Header("Overrides")]
    [Tooltip("Wenn true, wird DamageOverride statt des Modul-Basiswerts für Damage verwendet.")]
    [SerializeField] private bool overrideDamage;
    [SerializeField] private float damageOverride;

    [Tooltip("Wenn true, wird RadiusOverride statt des Modul-Basiswerts für Radius (AoERadius) verwendet.")]
    [SerializeField] private bool overrideRadius;
    [SerializeField] private float radiusOverride;

    [Tooltip("Wenn true, wird DurationOverride statt des Modul-Basiswerts für Duration verwendet.")]
    [SerializeField] private bool overrideDuration;
    [SerializeField] private float durationOverride;

    [Header("Tags")]
    [SerializeField] private List<SkillDeliveryTag> deliveryTagsAdd;

    [Header("Effects Add/Remove (IDs)")]
    [SerializeField] private string[] effectsAdd;
    [SerializeField] private string[] effectsRemove;

    public string ModuleId => moduleId;
    public string ArchetypeId => archetypeId;

    // Echte Overrides: Flags + Werte
    public bool OverrideDamage => overrideDamage;
    public float DamageOverride => damageOverride;

    public bool OverrideRadius => overrideRadius;
    public float RadiusOverride => radiusOverride;

    public bool OverrideDuration => overrideDuration;
    public float DurationOverride => durationOverride;

    public List<SkillDeliveryTag> DeliveryTagsAdd => deliveryTagsAdd;
    public string[] EffectsAdd => effectsAdd;
    public string[] EffectsRemove => effectsRemove;
}
