using CHAL.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "ArchetypeModuleOverrideDef", menuName = "Skills/ArchetypeModuleOverride")]
public class ArchetypeModuleOverrideDef : ScriptableObject
{
    [Header("IDs")]
    [SerializeField] private string moduleId;
    [SerializeField] private string archetypeId; // oder CombatProfile-ID

    [Header("Multipliers")]
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float radiusMultiplier = 1f;
    [SerializeField] private float durationMultiplier = 1f;

    [Header("Tags")]
    [SerializeField] private SkillDeliveryTag[] tagsAdd;

    [Header("Effects Add/Remove (IDs)")]
    [SerializeField] private string[] effectsAdd;
    [SerializeField] private string[] effectsRemove;

    public string ModuleId => moduleId;
    public string ArchetypeId => archetypeId;

    public float DamageMultiplier => damageMultiplier;
    public float RadiusMultiplier => radiusMultiplier;
    public float DurationMultiplier => durationMultiplier;

    public SkillDeliveryTag[] TagsAdd => tagsAdd;
    public string[] EffectsAdd => effectsAdd;
    public string[] EffectsRemove => effectsRemove;
}
