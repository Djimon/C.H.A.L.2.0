using CHAL.Data;
using System.Collections.Generic;

namespace CHAL.Systems.Loot.Models
{
    public sealed class LootDrop
    {
        public string itemId;
        public int quantity;
        // wir halten BEIDE Varianten; der Roller darf "chances" expandieren
        public float? chance;        // null wenn "chances" genutzt wird
        public float[] chancesArray; // null wenn "chance" genutzt wird

        // angereichert aus Registry:
        public Rarity rarity;
        public int lootValue;
    }

    public sealed class LootRule
    {
        public string tag;
        public List<LootDrop> drops = new();
        public int minDrops; // 0 = ignorieren
        public int maxDrops; // 0 = ignorieren
        public Dictionary<Rarity, int> rarityGuarantees = new();
    }

    public sealed class MergedLoot
    {
        public List<LootDrop> drops = new();
        public int minDrops;
        public int maxDrops;
        public Dictionary<Rarity, int> rarityGuarantees = new();
    }

    public sealed class LootResultEntry
    {
        public string EnemyId;   // optional: Referenz, welches Monster den Drop generiert hat
        public string PickedTag; // der Tag, der für diesen Drop relevant war
        public string ItemId;    // das eigentliche Item
    }
}