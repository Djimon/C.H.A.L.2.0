using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Crafting
{

    [CreateAssetMenu(fileName = "CraftingCatalog", menuName = "Data/CraftingCatalog")]
    public class CraftingCatalog : ScriptableObject
    {
        public List<RecipeDef> recipes = new();
    }
}
