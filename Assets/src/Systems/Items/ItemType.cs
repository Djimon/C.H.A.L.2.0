using CHAL.Data;
using System;

namespace CHAL.Systems.Items
{
    public static class ItemTypeUtils
    {
        public static ItemType FromId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return ItemType.Unknown;
            var p = itemId.IndexOf(':');
            var prefix = p >= 0 ? itemId.Substring(0, p) : itemId;

            switch (prefix)
            {
                case "remains": return ItemType.Remains;
                case "rune": return ItemType.Rune;
                case "part": return ItemType.Part;
                case "module": return ItemType.Module;
                case "gear": return ItemType.Gear;
                default: return ItemType.Unknown;
            }
        }
    }
}