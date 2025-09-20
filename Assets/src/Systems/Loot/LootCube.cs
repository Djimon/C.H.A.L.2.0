using CHAL.Data;
using CHAL.Systems.Items;
using CHAL.Systems.Wave;
using UnityEngine;

public class LootCube : MonoBehaviour
{
    public string _itemId { get; private set; }
    public int _qunatity { get; private set; } = 1;

    public void Init(string itemId, int quantity=1)
    {
        _itemId = itemId;
        _qunatity = quantity;

        var rarity = ItemRegistry.Instance.GetRarity(itemId);
        var renderer = GetComponent<Renderer>();

        renderer.material.color = RarityColors.Get(rarity);

        
    }

    private void OnMouseDown()
    {
        float pickupRadius = 0.3f;
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRadius);

        foreach (var hit in hits)
        {
            var lc = hit.GetComponent<LootCube>();
            if (lc != null)
            {
                OnLootCollected?.Invoke(lc._itemId, _qunatity);
                Destroy(lc.gameObject);
            }
        }
    }

    public static event System.Action<string,int> OnLootCollected;

    //after loot dropped physically freeze it in place
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // dein Plane sollte "Ground"-Tag haben
        {
            var rb = GetComponent<Rigidbody>();
            if (rb != null) Destroy(rb);

            var col = GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
        }
    }
}
