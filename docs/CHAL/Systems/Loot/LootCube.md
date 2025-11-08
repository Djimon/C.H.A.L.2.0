# CHAL.Systems.Loot.LootCube

_Automatically generated/updated from `Assets/src/Systems/Loot/LootCube.cs`._

# Purpose
- Defines a `LootCube` class that represents a loot cube containing items.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - public class `LootCube` : `MonoBehaviour`
    - Public fields/properties:
      - `string _itemId`: The unique identifier for the item.
      - `int _quantity`: The quantity of the item (default is 1).
    - Public methods:
      - `void Init(string itemId, int quantity = 1)`: Initializes the item with the specified ID and quantity.
      - `static event System.Action<string, int> OnLootCollected`: Invoked when loot is collected.

# Key Behavior & Side Effects
- `Init`: Sets the item ID and quantity, retrieves rarity, and changes the cube's color based on rarity.
- `OnMouseDown`: Detects mouse clicks, checks for nearby `LootCube` objects, invokes `OnLootCollected`, and destroys the collected loot cube.
- `OnCollisionEnter`: Freezes the loot cube in place upon collision with an object tagged "Ground" by removing its `Rigidbody` and setting its `Collider` to be a trigger.

# Constraints & Failure Modes
- The `OnMouseDown` method uses a fixed `pickupRadius` of 0.3f for detecting loot cubes.
- The `OnCollisionEnter` method assumes that the ground object has the "Ground" tag.
- If the `Rigidbody` or `Collider` components are missing, the corresponding actions will not occur.

# Example
```csharp
LootCube lootCube = new LootCube();
lootCube.Init("item123", 5);
```

# Unknowns
- None.
