using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Crafting
{
    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Data/Crafting Recipe")]
    public class RecipeDef : ScriptableObject
    {
        public string Id;
     
        [Header("Anzeige")]
        public string displayKey;
        public Sprite icon;
        public int tier = 1;
        public GearType slotType;

        [Header("Kosten (Items)")]
        public List<MaterialCost> inputs = new();

        [Header("Kosten (Währung)")]
        public List<CurrencyCost> currencyCosts = new();

        [Header("Output")]
        public string outputItemId;    // z.B. "gear:chest_leather"
        [Min(1)] public int outputCount = 1;

        private void OnValidate()
        {
            if (outputCount < 1) outputCount = 1;
            if (inputs != null)
            {
                for (int i = 0; i < inputs.Count; i++)
                    if (inputs[i].qty < 1) inputs[i] = new MaterialCost { itemId = inputs[i].itemId, qty = 1 };
            }
            if (currencyCosts != null)
            {
                for (int i = 0; i < currencyCosts.Count; i++)
                    if (currencyCosts[i].amount < 1) currencyCosts[i] = new CurrencyCost { currencyId = currencyCosts[i].currencyId, amount = 1 };
            }
        }
    }

    [Serializable]
    public struct MaterialCost
    {
        public string itemId;       // z.B. "part:iron_ingot"
        [Min(1)] public int qty;
    }

    [Serializable]
    public struct CurrencyCost
    {
        public string currencyId;   // z.B. "gold", "orb_rare"
        [Min(1)] public int amount;
    }
}
