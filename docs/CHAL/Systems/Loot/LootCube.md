# CHAL.Systems.Loot.LootCube

_Automatically generated/updated from `Assets/src/Systems/Loot/LootCube.cs`._

```text
1) Purpose
- Defines a LootCube MonoBehaviour representing a loot pickup in the world.
- Exposes item identity (_itemId) and quantity (_quantity) for initialization and pickup logic.
- Provides a static OnLootCollected event to notify listeners when loot is collected.

```

```csharp
2) Public API
- Namespace/module
  - CHAL.Systems.Loot

- Types
  - public class LootCube : MonoBehaviour
    - Public fields/properties
      - public string _itemId { get; private set; } [SerializeField]
        - Identifies the item represented by this loot cube
      - public int _quantity { get; private set; } = 1 [SerializeField]
        - Quantity of the item represented by this loot cube
    - Public methods
      - public void Init(string itemId, int quantity = 1)
        - Sets _itemId and _quantity
        - Looks up rarity via ItemRegistry.Instance.GetRarity(itemId)
        - Sets the Renderer material color using RarityColors.Get(rarity)
      - private void OnMouseDown()
        - Detects nearby LootCube instances via OverlapSphere(radius = 0.3)
        - For each LootCube hit, invokes OnLootCollected with that cube's itemId and this cube's quantity
        - Destroys the hit LootCube's GameObject
    - Public static event
      - public static event System.Action<string, int> OnLootCollected
        - Invoked when a loot cube is collected via OnMouseDown
    - Unity physics/scene behavior
      - private void OnCollisionEnter(Collision collision)
        - If collision object has tag "Ground":
          - Removes Rigidbody component if present
          - Sets Collider.isTrigger = true to freeze in place (scene behavior)

```

```csharp
3) Key Behavior & Side Effects
- Init(itemId, quantity)
  - Stores itemId and quantity
  - Retrieves rarity from ItemRegistry and applies color via RarityColors
- OnMouseDown
  - Scans nearby area (0.3f radius) for LootCube objects
  - For each hit LootCube:
    - Emits OnLootCollected(lc._itemId, _quantity)
    - Destroys the LootCube game object
- OnCollisionEnter
  - If collided with an object tagged "Ground":
    - Destroys any attached Rigidbody
    - Sets Collider.isTrigger = true to stop physical interactions

```

```csharp
4) Constraints & Failure Modes
- Init assumes a Renderer component exists; no null check may cause NullReferenceException if missing.
- Init relies on ItemRegistry and RarityColors being available; may fail if those systems are not initialized.
- OnLootCollected is invoked with lc._itemId and this._quantity; relies on lc being a LootCube (protected by null check).
- OnMouseDown uses a fixed pickup radius (0.3f); behavior depends on scene physics and overlapping colliders.
- OnCollisionEnter uses Destroy(rb) pattern; if Rigidbody not present, safe due to null check; otherwise, component removal is performed.
- Serialized fields on properties are unusual for Unity; behavior depends on Unity's serialization handling for properties with private setters.

```

```csharp
5) Example
// Subscribe to loot collection and initialize a LootCube instance
// using this file's public API
 LootCube.OnLootCollected += (itemId, qty) =>
 {
     // Handle collected loot (e.g., add to inventory)
     Debug.Log($"Collected {itemId} x{qty}");
 };

 // Assume 'loot' is a LootCube component reference obtained from a spawned prefab
 loot.Init("item_sword_iron", 2);

```

```text
6) Unknowns
- Exact item ID formats and validity rules (e.g., "item_sword_iron").
- Behavior of ItemRegistry.GetRarity and what RarityColors.Get returns (types/colors).
- Whether the project properly ensures a Renderer is always present on LootCube.
- How multiple LootCube instances interact when overlapped within the 0.3f radius during OnMouseDown.
- Any external listeners or side effects of OnLootCollected beyond this file. 
```
