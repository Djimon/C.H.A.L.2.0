# global.LootCube

_Automatically generated/updated from `Assets/src/Systems/Loot/LootCube.cs`._

# Purpose
- Defines the `LootCube` class for managing loot items in the game.

# Public API
- Namespace/module: None
- Types
  - public class LootCube : MonoBehaviour
    - Public fields/properties:
      - string _itemId: Identifier for the loot item.
      - int _quantity: Quantity of the loot item (default is 1).
    - Public methods:
      - void Init(string itemId, int quantity=1): Initializes the loot cube with an item ID and quantity.
      - static event System.Action<string,int> OnLootCollected: Invoked when loot is collected.

# Key Behavior & Side Effects
- `Init`: Sets the item ID and quantity, changes the color of the loot cube based on item rarity.
- `OnMouseDown`: Detects mouse clicks, checks for nearby loot cubes, invokes `OnLootCollected`, and destroys the collected loot cube.
- `OnCollisionEnter`: Freezes the loot cube in place upon collision with an object tagged "Ground".

# Constraints & Failure Modes
- `OnMouseDown`: Uses a fixed pickup radius of 0.3f for detecting loot cubes.
- `OnCollisionEnter`: Requires the ground object to have the "Ground" tag; if the Rigidbody is present, it will be destroyed.

# Example
```csharp
LootCube lootCube = new LootCube();
lootCube.Init("item123", 5);
```

# Unknowns
- None.

