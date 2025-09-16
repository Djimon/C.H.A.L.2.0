using UnityEngine;

namespace CHAL.Data
{

    [CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "Config/GameBalanceConfig")]
    public class GameBalanceConfig : ScriptableObject
    {
        // ==========================
        // LOOT
        // ==========================
        [System.Serializable]
        public struct LootBudgetSettings
        {
            public float levelFactor;      // z. B. 0.08
            public float budgetVariance;   // z. B. 0.2 (20 %)
            public float beta;             // Dämpfungsstärke Overflow
        }

        [System.Serializable]
        public struct LootFloorSettings
        {
            [Range(0, 1)] public float rare;       // z. B. 0.3
            [Range(0, 1)] public float epic;       // z. B. 0.2
            [Range(0, 1)] public float legendary;  // z. B. 0.1
            [Range(0, 1)] public float specials;  // z. B. 0
        }

        [System.Serializable]
        public struct LootUnluckySettings
        {
            public float alphaRare;       // z. B. 0.20
            public float alphaEpic;       // z. B. 0.15
            public float alphaLegendary;  // z. B. 0.10
            public float alphaSpecials; //Mythi,Holy,Daemonic, etc. //0
        }

        [System.Serializable]
        public struct LootTrimSettings
        {
            [Range(0, 1)] public float common;     // Wahrscheinlichkeit, dass Common entfernt wird
            [Range(0, 1)] public float uncommon;   // "
            [Range(0, 1)] public float rare;       // "
            [Range(0, 1)] public float epic;       // "
            [Range(0, 1)] public float legendary;  // "
        }

        [System.Serializable]
        public struct LootSettings
        {
            public LootBudgetSettings budget;
            public LootFloorSettings floors;
            public LootUnluckySettings unlucky;
            public LootTrimSettings trim;
        }




        [Header("Loot Settings")]
        public LootSettings loot;

        // ==========================
        // ENEMIES
        // ==========================
        [System.Serializable]
        public struct EnemyBudget
        {
            public int spawn;   // z. B. 5
            public int normal;  // z. B. 10
            public int magic;   // 20
            public int elite;   // 30
            public int boss;    // z. B. 50
            public int champion; // 100
        }

        [System.Serializable]
        public struct EnemyScaling
        {
            public float hpPerLevel;     // z. B. 0.1 = +10 % HP pro Level
            public float dmgPerLevel;    // z. B. 0.08 = +8 % DMG pro Level
        }

        [System.Serializable]
        public struct EnemySettings
        {
            public EnemyBudget budgetPoints;
            public EnemyScaling scaling;
        }

        [Header("Enemy Settings")]
        public EnemySettings enemies;

        // ==========================
        // ECONOMY
        // ==========================
        [System.Serializable]
        public struct CurrencySettings
        {
            public int baseGoldReward;    // z. B. 100 pro Kampf
            public int baseXpReward;      // z. B. 50 pro Kampf
            public float goldPerLevel;    // z. B. +20 % Gold je Level
            public float xpPerLevel;      // z. B. +15 % XP je Level
        }

        [System.Serializable]
        public struct EconomySettings
        {
            public CurrencySettings currencies;
        }

        [Header("Economy Settings")]
        public EconomySettings economy;

    }
}
