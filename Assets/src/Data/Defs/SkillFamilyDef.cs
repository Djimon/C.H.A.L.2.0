using CHAL.Data;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a skill family definition used in the game.
/// Contains base values and identity for skills.
/// </summary>
[CreateAssetMenu(fileName = "SkillFamilyDef", menuName = "Skills/SkillFamily")]
public class SkillFamilyDef : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string familyId;
    public SkillType SkillType;

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
    [SerializeField] private List<SkillDeliveryTag> deliveryTags;                  // Baseline Tags für die Familie
    [SerializeField] private List<SkillMechanicTag> mechanicTags;

    public string FamilyId => familyId;
    public List<SkillDeliveryTag> DeliveryTags => deliveryTags;
    public List<SkillMechanicTag> MechanicTags => mechanicTags;

    // Getter für Base-Werte – aktuell noch nicht verwendet,
    // später vom Resolver herangezogen.
    public float BaseDamage => baseDamage;
    public float BaseRadius => baseRadius;
    public float BaseDuration => baseDuration;
}
