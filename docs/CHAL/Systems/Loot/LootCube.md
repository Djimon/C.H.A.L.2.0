# CHAL.Systems.Loot.LootCube

_Automatically generated/updated from `Assets/src/Systems/Loot/LootCube.cs`._

```csharp
// (Documentation for LootCube.cs)

1) Purpose
- Defines LootCube as a MonoBehaviour representing a loot item in the world.
- Stores the item identifier and quantity for the loot cube; initializes visual tint based on item rarity.
- Handles mouse pickup interactions and physics-based state changes to freeze loot on the ground; broadcasts loot-collection events.

2) Public API
- Namespace/module: CHAL.Systems.Loot
- Types
  - public class LootCube : MonoBehaviour
    - Public properties
      - public string _itemId { get; private set; } // serialized; item id
      - public int _quantity { get; private set; } = 1 // serialized; item quantity
    - Public methods
      - public void Init(string itemId, int quantity = 1)
        - Sets _itemId, _quantity
        - rarity = ItemRegistry.Instance.GetRarity(itemId)
        - renderer = GetComponent<Renderer>()
        - renderer.material.color = RarityColors.Get(rarity)
    - Public static events
      - public static event System.Action<string, int> OnLootCollected
    - Private methods (surface exposed by Unity, not public API, but part of behavior)
      - private void OnMouseDown()
        - pickupRadius = 0.3f
        - hits = Physics.OverlapSphere(transform.position, pickupRadius)
        - foreach (var hit in hits)
          - lc = hit.GetComponent<LootCube>()
          - if (lc != null)
            - OnLootCollected?.Invoke(lc._itemId, _quantity)
            - Destroy(lc.gameObject)
      - private void OnCollisionEnter(Collision collision)
        - if (collision.gameObject.CompareTag("Ground"))
          - rb = GetComponent<Rigidbody>()
          - if (rb != null) Destroy(rb)
          - col = GetComponent<Collider>()
          - if (col != null) col.isTrigger = true

3) Key Behavior & Side Effects
- Init behavior
  - Stores itemId and quantity
  - Looks up rarity via ItemRegistry.Instance.GetRarity(itemId)
  - Applies color via RarityColors.Get(rarity) to this object's Renderer material
- Loot pickup flow (OnMouseDown)
  - Detects all colliders within 0.3 units of this LootCube
  - For each nearby LootCube hit, invokes OnLootCollected with the nearby lc’s itemId and this cube’s quantity
  - Destroys the nearby LootCube object
- Collision behavior (OnCollisionEnter)
  - When colliding with an object tagged "Ground", removes the Rigidbody component if present
  - Sets this object's Collider to isTrigger = true to freeze physical interactions

4) Constraints & Failure Modes
- Assumes a Renderer component exists; no null check for renderer before setting color
- Depends on ItemRegistry.Instance.GetRarity(itemId) and RarityColors.Get(rarity); potential null or invalid rarity handling not explicit
- OverlapSphere has no layer mask; may detect unintended objects nearby
- OnLootCollected is invoked with lc._itemId and this._quantity; may be odd if lc is different from this object
- OnMouseDown relies on a mouse input path; requires collider and camera setup per Unity input system
- Serialization of properties with [SerializeField] on properties (instead of fields) is unusual in Unity; behavior depends on Unity serialization support

5) Example
- Basic usage sketch
// Subscribe to loot collection
LootCube.OnLootCollected += (itemId, qty) => {
    // handle itemId and quantity
};

// Initialize a LootCube instance
var loot = someLootCubeInstance; // e.g., via scene setup
loot.Init("item_sword_01", 2);

6) Unknowns
- Exact item data associated with itemId (name, stats, etc.)
- How rarity values map to colors via RarityColors.Get
- Whether [SerializeField] on properties behaves as expected in this project
- How LootCube instances are spawned and managed lifecycle-wise outside Init
- Any additional behavior triggered by OnLootCollected subscribers beyond this file’s scope
```
