using CHAL.Systems.Skill;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents data for a skill, including its identity, damage, and casting properties.
/// </summary>
namespace CHAL.Data
{

    [CreateAssetMenu(fileName = "SkillModuleDef", menuName = "Skills/SkillModule")]
/// <summary>
/// Represents data for a skill, including its identity, damage, and casting properties.
/// </summary>
    public class 
        SkillModuleDef : ScriptableObject
    {
        [Header("Identity")]
        public string SkillId;
        public string DisplayName;
        public HeroAttribs AttributeAffinity = HeroAttribs.STR;
        public float BaseDamage = 1;
        public int minRequiredTier = 1;
        public DamageType BaseDamageType = DamageType.Physical;

        [Header("ModuleCore Ingredients")]
        public CoreType defualtCore;
        public List<CoreType> changeCoreTypesAllowed;

        //TODO: Deprecated: Delete if all references are remapped
        //public List<DamageEntry> DamageTypes;

        [Tooltip("Time in seconds to cast this skill. 0 = instant.")]
        public float CastTime = 0f;
        [Tooltip("Cooldown in seconds before this skill can be used again.")]
        public float Cooldown = 2f;

        [Header("SkillType")]
        [Tooltip("Determines the main behavior of the skill: Melee, Projectile, Spell, or Summon.")]
        public SkillType SkillType = SkillType.Melee;   // should default to skillFamily.SkillType;
        public bool isProjectile = false;
        public bool isAoE = false;
        public bool hasDuration = false;

        [Header("Composition")]
        public SkillRange Range = SkillRange.Reach;
        [Tooltip("Duration in seconds for effects like buffs, debuffs, or DoTs.")]
        public float Duration = 0f;
        public float ProjectileSpeed = 0f;
        public int ProjectileCount = 0;
        public float Radius = 0f;
        public float damageAttributeScalingFactor = 1.0f;

        [Header("Hooks / Effects")]
        [Tooltip("Effects applied immediately when the skill is cast.")]
        public List<SkillImpactBase> OnCastImpact;
        [Tooltip("Effects applied when this skill successfully hits a target.")]
        public List<SkillImpactBase> OnHitImpact;
        public List<SkillImpactBase> OnEndImpact;

        [Header("Meta")]
        public List<SkillDeliveryTag> DeliveryTags;    // Projectile, Fire, DoT, Buff, etc.
        public List<SkillMechanicTag> MechanicTags;

        [Header("Presentation")]
        [Tooltip("Prefab spawned when the skill effect is triggered (VFX, projectile, etc.).")]
        public GameObject vfxPrefab; //which the Skilluser will spawn, when he finsihes his animation
        [Tooltip("Animation type used when performing this skill.")]
        public AnimationType animationType;

    }
}
