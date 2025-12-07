using CHAL.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillFamilyDef", menuName = "Skills/SkillFamily")]
public class SkillFamilyDef : ScriptableObject
{
    [Header("IDs")]
    [SerializeField] private string familyId;

    [Header("Base Values (optional)")]
    // Hier kommt später dein "Base-Block" aus dem MD rein:
    // Damage, Radius, Duration etc. – vorerst leer.
    // Beispiel-Platzhalter:
    [SerializeField] private float baseDamage;
    [SerializeField] private float baseRadius;
    [SerializeField] private float baseDuration;

    [Header("Scaling & Tags")]
    [SerializeField] private AnimationCurve mainStatScaling;   // optional, kann auch anders aussehen
    [SerializeField] private string[] defaultScaleAxes;        // z.B. "Damage", "Radius"
    [SerializeField] private SkillDeliveryTag[] tags;                  // Baseline Tags für die Familie

    public string FamilyId => familyId;
    public SkillDeliveryTag[] Tags => tags;

    // Getter für Base-Werte – aktuell noch nicht verwendet,
    // später vom Resolver herangezogen.
    public float BaseDamage => baseDamage;
    public float BaseRadius => baseRadius;
    public float BaseDuration => baseDuration;
}
