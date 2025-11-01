# CHAL.Systems.Inventory.InventoryItem

_Automatically generated/updated from `Assets/src/Systems/Inventory/Inventory.cs`._

1) Purpose
- Defines Inventory and InventoryItem types for managing items with stacking and slot limits, keyed by a prefix (invID).
- Provides add/remove/query operations and simple (de)serialization helpers to/from a dictionary.
- Integrates with external helpers (InventoryRules, DebugManager) for capacity rules and logging.

2) Public API
- Namespace: CHAL.Systems.Inventory
- Types
  - public class Inventory
    - public readonly string invID
      - Inventory identifier/prefix used for validation and capacity rules
    - private List<InventoryItem> _items
    - public Inventory(string prefix)
      - Constructor; sets invID = prefix
    - public bool AddItem(string itemId, int amount = 1)
      - Adds items to the inventory with stacking and slot checks
      - Returns true on success, false on failure (guard fail, full slot/stack, leftovers)
    - public bool RemoveItem(string itemId, int amount = 1)
      - Decreases count for the given item; removes entry if count <= 0
      - Returns true on success, false if item not found or insufficient amount
    - public int GetItemCount(string itemId)
      - Returns the total count for the given itemId (0 if not present)
    - public List<InventoryItem> GetAllItems()
      - Returns the internal list of InventoryItem
    - public Dictionary<string, int> ToDictionary()
      - Converts items to a dictionary mapping ItemId -> Count
    - public void FromDictionary(Dictionary<string, int> dict)
      - Rebuilds internal _items from a dictionary of ItemId -> Count
  - public class InventoryItem
    - public string ItemId
    - public int Count

3) Key Behavior & Side Effects
- AddItem flow
  - Validates itemId starts with invID; if not, returns false
  - Reads maxStack = InventoryRules.GetMaxStack(invID) and maxSlots = InventoryRules.GetMaxSlots(invID)
  - If an existing InventoryItem with matching ItemId exists:
    - Compute spaceLeft = maxStack - entry.Count
    - If spaceLeft <= 0, log and return false
    - toAdd = min(amount, spaceLeft); entry.Count += toAdd; amount -= toAdd
    - If amount > 0 after stacking, log and return false
    - Log success and return true
  - If itemId is new:
    - If _items.Count < maxSlots:
      - toAdd = min(amount, maxStack); _items.Add(new InventoryItem { ItemId = itemId, Count = toAdd })
      - amount -= toAdd
      - If amount > 0, log and return false
      - Log success and return true
    - If no slot available, log and return false
- RemoveItem flow
  - Finds entry by ItemId; if not found or entry.Count < amount, return false
  - Decrease entry.Count by amount; if entry.Count <= 0, remove entry
  - Return true
- GetItemCount/GetAllItems/ToDictionary/FromDictionary are direct surface operations; FromDictionary replaces internal _items
- Logging
  - Uses DebugManager.Log/DebugLog with various messages during add/remove and capacity events
- Side effects
  - Mutates internal _items state
  - Can expose internal list via GetAllItems (no defensive copy)

4) Constraints & Failure Modes
- Item validation
  - AddItem requires itemId.StartsWith(invID); otherwise returns false
- Capacity constraints
  - Respects InventoryRules.GetMaxStack(invID) for per-item stack size
  - Respects InventoryRules.GetMaxSlots(invID) for total slots
  - Partial additions can fail if leftovers cannot be accommodated (return false)
- State assumptions
  - GetAllItems returns the internal list (not a copy); external code can mutate
  - FromDictionary completely overwrites _items with a new list constructed from the provided dictionary
- Null/invalid inputs
  - No explicit null checks for itemId or dict; potential exceptions if null
- Threading/async
  - No concurrency controls; not thread-safe

5) Example
- Not provided in file; usage would involve creating Inventory and calling AddItem/RemoveItem as needed, bearing in mind itemId must start with the inventory prefix.

6) Unknowns
- Implementations of InventoryRules.GetMaxStack and InventoryRules.GetMaxSlots (external to this file)
- Behavior of DebugManager.Log/DebugLog (logging details, levels beyond what’s shown)
- Whether GetAllItems should be exposed as a live reference or a defensive copy (current code exposes the internal list)
- How null itemIds or null dictionaries should be handled (not explicit)
