namespace CHAL.Data
{
    [System.Serializable]
    public class RarityGuaranteeKV
    {
        public string rarity; // "Common", "Rare", "Epic", "Legendary"
        public int min;
    }

    [System.Serializable]
    public class LootDropDto
    {
        public string itemId;
        public float chance;       // optional wenn "chances" gesetzt
        public float[] chances;    // optional
        public int quantity = 1;

        public string sourceTag;

    }

    [System.Serializable]
    public class LootRuleDto
    {
        public string tag;
        public LootDropDto[] drops;
        public int minDrops = 0;
        public int maxDrops = 0;
        public RarityGuaranteeKV[] rarityGuarantees; // optional
    }

    [System.Serializable]
    public class SpecialRule
    {
        public string[] tags;
        public LootDropDto[] drops;
    }

    [System.Serializable]
    public class SpecialRulesWrapper
    {
        public SpecialRule[] rules;
    }
}