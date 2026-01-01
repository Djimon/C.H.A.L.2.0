using CHAL.Data;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CHAL.Systems.Crafting
{
    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Data/Crafting Recipe")]
/// <summary>
/// Represents a recipe definition used in the game.
/// Contains details about the recipe's costs and display properties.
/// </summary>
    public class RecipeDef : ScriptableObject
    {
        public string Id;
     
        [Header("Anzeige")]
        public string displayKey;
        public Sprite icon;
        public int tier = 1;
        public GearType slotType;

        [Header("Kosten (Items)")]
        public List<MaterialCost> inputs;

        [Header("Kosten (Währung)")]
        public List<CurrencyCost> currencyCosts = new();

        [Header("Output")]
        public ItemDef outputRef;
        public string outputItemId;    // z.B. "gear:chest_leather"
        [Min(1)] public int outputCount = 1;

        private void OnValidate()
        {
            if(outputRef != null && outputRef.itemId != outputItemId)
                outputItemId = outputRef.itemId;

            for (int k = 0; k < inputs.Count; k++)
            {
                if (inputs[k].itemref != null && inputs[k].itemId != inputs[k].itemref.itemId)
                {
                    var id = inputs[k].itemref.itemId;
                    int q = inputs[k].qty < 0 ? 0 : inputs[k].qty;
                    inputs[k] = new MaterialCost { itemref = inputs[k].itemref, itemId = id, qty = q };
                }
            }

            if (outputCount < 1) outputCount = 1;

            if (currencyCosts != null)
            {
                for (int i = 0; i < currencyCosts.Count; i++)
                    if (currencyCosts[i].amount < 0) currencyCosts[i] = new CurrencyCost { currencyId = currencyCosts[i].currencyId, amount = 0};
            }

        }
    }

    [Serializable]
    public struct MaterialCost
    {
        public ItemDef itemref;
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
