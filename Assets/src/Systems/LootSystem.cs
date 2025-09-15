using CHAL.Core;
using UnityEngine;

namespace CHAL.Systems.Loot
{
    public class LootSystem : MonoBehaviour
    {
        private GameBalanceConfig cfg_BALANCE => BalanceManager.Instance.Config;

        public float GetRareFloor() => cfg_BALANCE.loot.floors.rare;

        public float GetOverflowFactor(float U, float v_i, float B)
        {
            if (U + v_i <= B) return 1f;

            float overflow = (U + v_i - B) / B;
            float beta = cfg_BALANCE.loot.budget.beta;
            float floor = cfg_BALANCE.loot.floors.rare; // Beispiel: Rarity-spezifisch wählen
            return Mathf.Max(floor, Mathf.Exp(-beta * overflow));
        }

        public float ApplyUnluckyRare(float pBase, int streakRare)
        {
            float alpha = cfg_BALANCE.loot.unlucky.alphaRare; // z. B. 0.20
            return pBase * (1f + alpha * streakRare);
        }
    }
}
