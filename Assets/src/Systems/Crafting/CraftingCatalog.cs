using CHAL.Systems.Crafting;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingCatalog", menuName = "Data/CraftingCatalog")]
public class CraftingCatalog : ScriptableObject
{
    public List<RecipeDef> recipes = new();
}
