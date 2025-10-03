using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "EnemyDef", menuName = "Data/EnemyDef")]
    public class EnemyDef : ScriptableObject
    {
        [Header("Identity")]
        public string enemyId;               // z. B. "insect_worker"
        public string displayNameKey;        // Lokalisierungs-Key für Namen

        [Header("Base Stats")]
        public int baseHP = 10;
        public int baseDamage = 2;
        public float moveSpeed = 2f;
        public float sightRange = 10f;

        [Header("Reward Settings")]
        public int lootValue = 1;            // Einfluss aufs Budget-System
        public int xpReward = 1;
        public EnemyRank BaseRank = EnemyRank.Normal;  // Rank = Spawn, Normal, Magic, Elite, Boss …
        public List<string> baseTags = new();             // z. B. "insectoid", "poison"

        [Header("Combat")]
        public List<SkillData> baseAttacks = new();       // Basis-Skills
        public EnemyAIType aiType = EnemyAIType.AttackFirst;  // Simple AI-Strategie

        [Header("Visuals / Prefabs")]
        public GameObject prefab;            // Model/Prefab zum Spawnen
        public Sprite icon;                  // UI-Icon
    }
}
