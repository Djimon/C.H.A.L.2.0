using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    public enum Rarity
    {
        unknown = -1,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic,
        Holy,
        Daemonic
    }

    public static class RarityColors
    {
        private static readonly Dictionary<Rarity, Color> _map = new()
        {
            { Rarity.unknown, Color.gray },
            { Rarity.Common, Color.white },
            { Rarity.Uncommon, new Color(108f/255f, 215f/255f, 145f/255f) }, // Hellgrün
            { Rarity.Rare,     new Color(  0f/255f, 156f/255f, 230f/255f) }, // Cyan
            { Rarity.Epic,     new Color(235f/255f, 160f/255f,  60f/255f) }, // Rot-Gold
            { Rarity.Legendary,new Color(130f/255f,  70f/255f, 220f/255f) }, // Violett
            { Rarity.Mythic,   new Color(210f/255f,  85f/255f, 160f/255f) }, // Magenta
            { Rarity.Holy,     new Color(249f/255f, 232f/255f, 156f/255f) }, // Radiant-Gold
            { Rarity.Daemonic, new Color( 87f/255f,  12f/255f,  27f/255f) }  // Blutrot
            // Uniques: 206,40,45
        };
  
/// <summary>
/// Retrieves the color associated with the specified rarity.
/// </summary>
/// <param name="rarity">The rarity to get the corresponding color for.</param>
/// <returns>The color associated with the given rarity, or white if not found.</returns>
        public static Color Get(Rarity rarity) =>
            _map.TryGetValue(rarity, out var c) ? c : Color.white;
    }
}
