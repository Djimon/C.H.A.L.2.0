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
            { Rarity.Uncommon, new Color(138f/255f, 165f/255f, 230f/255f) }, // Hellblau
            { Rarity.Rare,     new Color(47f/255f, 181f/255f, 105f/255f) },  // Smaragdgrün
            { Rarity.Epic,     new Color(217f/255f, 175f/255f, 61f/255f) },  // Gold
            { Rarity.Legendary,new Color(110f/255f,  38f/255f, 212f/255f) }, // Violett
            { Rarity.Mythic,   new Color(191f/255f,  63f/255f, 178f/255f) }, // Magenta
            { Rarity.Holy,     new Color(172f/255f, 232f/255f, 217f/255f) }, // Türkis
            { Rarity.Daemonic, new Color( 87f/255f,  12f/255f,  27f/255f) }  // Blutrot
        };
  
        public static Color Get(Rarity rarity) =>
            _map.TryGetValue(rarity, out var c) ? c : Color.white;
    }
}
