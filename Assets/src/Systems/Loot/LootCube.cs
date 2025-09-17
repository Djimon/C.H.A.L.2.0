using CHAL.Data;
using CHAL.Systems.Items;
using CHAL.Systems.Wave;
using UnityEngine;

public class LootCube : MonoBehaviour
{
    private string _itemId;

    public void Init(string itemId)
    {
        _itemId = itemId;


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
                OnLootCollected?.Invoke(lc._itemId);
                Destroy(lc.gameObject);
            }
        }
    }

    public static event System.Action<string> OnLootCollected;

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
