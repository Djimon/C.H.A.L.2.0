using CHAL.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillModuleDef", menuName = "Skills/SkillModule")]
public class SkillModuleDef : ScriptableObject
{
    [Header("IDs")]
    [SerializeField] private string id;

    [Header("Family")]
    [SerializeField] private SkillFamilyDef family;

    [Header("Base Overrides")]
    // Overrides auf Family – aktuell nur Platzhalter.
    [SerializeField] private bool overrideDamage;
    [SerializeField] private float damageOverride;

    [SerializeField] private bool overrideRadius;
    [SerializeField] private float radiusOverride;

    [SerializeField] private bool overrideDuration;
    [SerializeField] private float durationOverride;

    [Header("Tags")]
    [SerializeField] private SkillDeliveryTag[] tagsAdd;

    [Header("Effects (Def-Ebene, noch nicht genutzt)")]
    // IDs oder direkte Referenzen auf deine EffectDefs
    [SerializeField] private string[] onCastEffects;
    [SerializeField] private string[] onHitEffects;
    [SerializeField] private string[] onEndEffects;

    public string Id => id;
    public SkillFamilyDef Family => family;
    public SkillDeliveryTag[] TagsAdd => tagsAdd;

    public bool OverrideDamage => overrideDamage;
    public float DamageOverride => damageOverride;

    public bool OverrideRadius => overrideRadius;
    public float RadiusOverride => radiusOverride;

    public bool OverrideDuration => overrideDuration;
    public float DurationOverride => durationOverride;

    public string[] OnCastEffects => onCastEffects;
    public string[] OnHitEffects => onHitEffects;
    public string[] OnEndEffects => onEndEffects;
}
