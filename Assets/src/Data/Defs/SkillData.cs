using CHAL.Data;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkilData", menuName = "Data/SkillData")]
public class SkillData : ScriptableObject
{
    public string SkillId;
    public string DisplayName;
    public float BaseDamage;
    public float Cooldown;
    public float Range;
    public float Duration;
    public float ProjectileSpeed;
    public int ProjectileCount;
    public float AoERadius;
    public List<SkillTag> Tags;    // Projectile, Fire, DoT, Buff, etc.
    //public DamageType DamageType;  // Physical, Fire, Poison ...
    public List<DamageEntry> DamageTypes;
    //TODO: kan have multible dmtypes?
}
