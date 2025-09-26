using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{

    /* Example
    AttackDef: fireball_poison
    damages:
      - { type: Fire, multiplier: 2.0 }  <- durch externe modifier angesteuert
      - { type: Poison, multiplier: 1.0 }
    cooldown: 3.0  <- autoattack
    tags: ["projectile", "aoe"]
    animationType: "Cast"  <- to tell the enemy which animation it shoul play using this skill
    vfxPrefab <- visual projectil/ swing or explosion effekt
     */

    [CreateAssetMenu(fileName = "AttackDef", menuName = "Data/AttackDef")]
    public class AttackDef : ScriptableObject
    {
        [Header("Identity")]
        public string attackId;
        public string displayNameKey;

        [Header("Damage")]
        public List<DamageEntry> damages = new();
        // z. B. [{Phys, 1.5}, {Fire, 2.0}]

        [Header("Cooldown")]
        public float cooldown = 0f;

        [Header("Meta")]
        public string[] tags; // "aoe", "projectile", "buff"

        [Header("Presentation")]
        public string animationType; // "MeleeSwing", "Cast", "Shoot"
        public GameObject vfxPrefab;
    }

    [System.Serializable]
    public struct DamageEntry
    {
        public DamageType type;
        public float baseMultilpier; // z. B. 1.5f = 150 % EnemyBaseDamage
    }
}
