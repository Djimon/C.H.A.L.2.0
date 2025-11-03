# CHAL.Systems.Inventory.Inventory

_Automatically generated/updated from `Assets/src/Systems/Inventory/Inventory.cs`._

```csharp
// Documentation for: Assets/src/Systems/Inventory/Inventory.cs

1) Purpose
- Serializable Inventory type that groups InventoryItem entries under a per-inventory identifier (invID) and maintains a private item list.
- Supports adding/removing items with stacking and slot limits enforced via InventoryRules; exposes all items and supports dictionary conversion.
- Defines InventoryItem as a simple serializable pair of ItemId and Count.

2) Public API
- Namespace: CHAL.Systems.Inventory

Types
- public class Inventory
  - public readonly string invID
  - private List<InventoryItem> _items
  - public Inventory(string prefix)
  - public bool AddItem(string itemId, int amount = 1)
  - public bool RemoveItem(string itemId, int amount = 1)
  - public int GetItemCount(string itemId)
  - public List<InventoryItem> GetAllItems()
  - public Dictionary<string, int> ToDictionary()
  - public void FromDictionary(Dictionary<string, int> dict)

- public class InventoryItem
  - public string ItemId
  - public int Count

3) Key Behavior & Side Effects
- AddItem(string itemId, int amount = 1)
  - Logs diagnostic test: itemId.StartsWith(invID)
  - If itemId does not start with invID, returns false
  - Retrieves maxStack and maxSlots via InventoryRules
  - If an existing InventoryItem with same ItemId is found:
    - Compute spaceLeft = maxStack - Count
    - If spaceLeft <= 0, logs and returns false
    - toAdd = min(amount, spaceLeft); increment entry.Count by toAdd; decrease amount by toAdd
    - If amount > 0, logs and returns false; otherwise logs success and returns true
  - If itemId is new:
    - If _items.Count < maxSlots:
      - toAdd = min(amount, maxStack); add new InventoryItem { ItemId = itemId, Count = toAdd }
      - Decrease amount by toAdd
      - If amount > 0, logs and returns false; otherwise logs success and returns true
    - If slots are full, logs and returns false
- RemoveItem(string itemId, int amount = 1)
  - Finds entry by ItemId
  - If entry not found or entry.Count < amount, returns false
  - Decrements entry.Count by amount
  - If entry.Count <= 0, removes entry from _items
  - Returns true
- GetItemCount(string itemId)
  - Returns entry.Count if found, otherwise 0
- GetAllItems()
  - Returns the internal _items list (exposes internal state)
- ToDictionary()
  - Converts _items to Dictionary<string, int> with ItemId as key and Count as value
- FromDictionary(Dictionary<string, int> dict)
  - Replaces _items by materializing InventoryItem from dict entries

4) Constraints & Failure Modes
- Guard: AddItem requires itemId to start with invID; otherwise addition is rejected
- Capacity guards depend on InventoryRules.GetMaxStack(invID) and InventoryRules.GetMaxSlots(invID)
- Adding to an existing stack fails if the stack has reached maxStack and no space remains
- Adding a new item fails if there is no free slot (maxSlots reached) or if leftover amount cannot be allocated within a single add
- Removing items may result in the entry being removed if Count <= 0
- GetAllItems returns a direct reference to the internal list (not a copy)
- FromDictionary does not perform explicit validation on dict (null handling not explicit)

5) Example
```csharp
// Minimal usage example
var inv = new CHAL.Systems.Inventory.Inventory("INV");
bool ok1 = inv.AddItem("INV_SWORD", 3);      // adds up to maxStack for new ItemId
int count = inv.GetItemCount("INV_SWORD");   // e.g., 3

bool ok2 = inv.RemoveItem("INV_SWORD", 1);  // decreases to 2 or removes if needed

var dict = inv.ToDictionary();              // convert to dictionary
var inv2 = new CHAL.Systems.Inventory.Inventory("INV");
inv2.FromDictionary(dict);                   // restore items from dictionary
```

6) Unknowns
- Exact values and behavior of InventoryRules.GetMaxStack and InventoryRules.GetMaxSlots (per invID)
- Details of DebugManager logging behavior and debug level semantics
- Thread-safety and synchronization guarantees for AddItem/RemoveItem/GetAllItems
- Whether GetAllItems should return a mutable reference or a defensive copy
- Any external interactions not visible in this file (e.g., how inventories are persisted or synchronized with other systems)
```
