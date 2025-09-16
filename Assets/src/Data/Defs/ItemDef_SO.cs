using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "ItemDef", menuName = "Data/ItemDef")]
    public class ItemDef : ScriptableObject
    {
        [Tooltip("Schema: category:item, z.B. remains:gland")]
        public string itemId;

        public string displayName;
        [TextArea] public string description;
        public Sprite icon;

        public Rarity rarity = Rarity.Common;

        [Tooltip("Wert für Softcap/Budget (empf.: Common 10, Rare 30, Epic 50, Legendary 80)")]
        public int lootValue = 10;

        void OnValidate()
        {
            // Basisschutz: korrekte ID
            if (!ItemKey.TryParse(itemId, out _))
            {
                Debug.LogWarning($"[ItemDef] Ungültige itemId '{itemId}' in {name}. Erwartet 'category:item'.");
            }
            // Sanity für LootValue
            if (lootValue < 0) lootValue = 0;
        }

    }
}
