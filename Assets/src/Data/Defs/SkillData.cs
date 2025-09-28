using CHAL.Data;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkilData", menuName = "Data/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("Identity")]
    public string SkillId;
    public string DisplayName;
    public float BaseDamage = 1;
    public List<DamageEntry> DamageTypes;
    public float CastTime = 0f;
    public float Cooldown = 2f;

    [Header("SkillType")]
    public bool isProjectile =false;
    public bool isAoE = false;
    public bool hasDuration = false;

    [Header("Composition")]
    public float Range = 1f;
    public float Duration = 0f;
    public float ProjectileSpeed = 0f;
    public int ProjectileCount = 0;
    public float AoERadius = 0f;

    [Header("Meta")]
    public List<SkillTag> Tags;    // Projectile, Fire, DoT, Buff, etc.

    [Header("Presentation")]
    public GameObject vfxPrefab; //which the Skilluser will spawn, when he finsihes his animation
    public AnimationType animationType;

}
