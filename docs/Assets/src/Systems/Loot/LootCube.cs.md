# Assets/src/Systems/Loot/LootCube.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `LootCube` class that represents collectible loot in the game.

## Public API
- Namespace/module: None specified.
- Types
  - public class LootCube : MonoBehaviour
    - Public fields/properties:
      - string _itemId: Identifier for the item.
      - int _quantity: Quantity of the item (default is 1).
    - Public methods:
      - void Init(string itemId, int quantity=1): Initializes the loot cube with an item ID and quantity.
      - void OnMouseDown(): Handles mouse click events to collect loot.
      - void OnCollisionEnter(Collision collision): Handles collision events to freeze the loot cube upon landing.

## Key Behavior & Side Effects
- `Init`: Sets the item ID and quantity, changes the color of the loot cube based on item rarity.
- `OnMouseDown`: Collects loot from nearby `LootCube` instances and invokes `OnLootCollected` event.
- `OnCollisionEnter`: Freezes the loot cube in place when it collides with an object tagged "Ground".

## Constraints & Failure Modes
- `OnMouseDown`: Only collects loot from other `LootCube` instances within a 0.3f radius.
- `OnCollisionEnter`: Requires the ground object to be tagged as "Ground" for proper functionality.

## Example
```csharp
LootCube lootCube = new LootCube();
lootCube.Init("item123", 5);
```

## Unknowns
- The implementation details of `ItemRegistry` and `RarityColors` are not provided.
- The behavior of the game when the `OnLootCollected` event is invoked is not specified.
```
