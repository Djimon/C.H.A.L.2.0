using CHAL.Data;
using CHAL.Systems.Items;
using System;
using System.Collections.Generic;

namespace CHAL.Systems.Inventory
{
    public sealed class InventoryDomain : IInventoryDomain
    {
        private readonly Dictionary<string, InventoryInstance> _instances = new();

        // Optionaler Hook: Tags/Item-Infos. In Unity lieferst du hier Adapter auf ItemRegistry.
        public Func<string, bool> ItemExists;
        public Func<string, string, bool> ItemHasTag; // (itemId, tag) => true/false


        public event Action<string, int, ItemStackRef?> OnSlotChanged;


        /// <summary>
        /// Checks if an instance exists by its ID.
        /// </summary>
        /// <param name="instanceId">The ID of the instance to check.</param>
        /// <returns>True if the instance exists; otherwise, false.</returns>
        public bool HasInstance(string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId)) return false;
            return _instances.ContainsKey(instanceId);
        }


/// <summary>
/// Retrieves an inventory instance by its ID.
/// </summary>
/// <param name="instanceId">The ID of the inventory instance to retrieve.</param>
/// <returns>The inventory instance if found; otherwise, null.</returns>
        public InventoryInstance GetInstance(string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId)) return null;
            _instances.TryGetValue(instanceId, out var inst);
            return inst;
        }


/// <summary>
/// Registers an instance of an inventory.
/// </summary>
/// <param name="inst">The inventory instance to register.</param>
        public void RegisterInstance(InventoryInstance inst)
        {
            _instances[inst.instanceID] = inst;
        }

        public ItemStackRef? Peek(string instanceId, int slotIndex)
        {
            if (!_instances.TryGetValue(instanceId, out var inv)) return null;
            if (slotIndex < 0 || slotIndex >= inv.slots.Length) return null;
            return inv.slots[slotIndex].stack;
        }

/// <summary>
/// Gets the number of slots for the specified instance.
/// </summary>
/// <param name="instanceId">The ID of the instance.</param>
/// <returns>The number of slots for the instance.</returns>
        public int SlotCount(string instanceId)
        {
            return _instances.TryGetValue(instanceId, out var inv) ? inv.slots.Length : 0;
        }

/// <summary>
/// Clears all slots for the specified instance.
/// </summary>
/// <param name="instanceId">The ID of the instance whose slots will be cleared.</param>
        public void ClearAllSlots(string instanceId)
        {
            if (!_instances.TryGetValue(instanceId, out var inv) || inv?.slots == null)
                return;

            for (int i = 0; i < inv.slots.Length; i++)
            {
                inv.slots[i].stack = null;
                OnSlotChanged?.Invoke(instanceId, i, null);
            }
        }

        private static bool PassesFilter(Slot slot, string itemId)
        {
            var f = slot.Filter;
            if (f == null) return true;

            // 1) Blocked IDs
            if (f.BlockedItemIds != null)
                for (int i = 0; i < f.BlockedItemIds.Count ; i++)
                    if (f.BlockedItemIds[i] == itemId)
                    {
                        DebugManager.Log($"Failed Filter: blocked: {f.BlockedItemIds[i]}", DebugManager.EDebugLevel.Dev, "Inventory");
                        return false;
                    } 

            // 2) Allowed IDs
            if (f.AllowedItemIds != null && f.AllowedItemIds.Count > 0)
            {
                bool ok = false;
                for (int i = 0; i < f.AllowedItemIds.Count; i++)
                    if (f.AllowedItemIds[i] == itemId) { ok = true; break; }
                if (!ok)
                {
                    DebugManager.Log($"failed Filter: not in Allowed: {itemId}", DebugManager.EDebugLevel.Dev, "Inventory");
                    return false;
                }
            }

            // 3) TODO: Tags prÃ¼fen?
            

            // 4) Types prÃ¼fen
            var t = ItemTypeUtils.FromId(itemId);
            if (f.BlockedItemTypes != null)
                for (int i = 0; i < f.BlockedItemTypes.Count; i++)
                    if (f.BlockedItemTypes[i] == t)
                    {
                        DebugManager.Log($"Failed Filter: blocked ItemType: {t}", DebugManager.EDebugLevel.Dev, "Inventory");
                        return false;
                    }

            if (f.AllowedItemTypes != null && f.AllowedItemTypes.Count > 0)
            {
                bool ok = false;
                for (int i = 0; i < f.AllowedItemTypes.Count; i++)
                    if (f.AllowedItemTypes[i] == t) { ok = true; break; }
                if (!ok)
                {
                    DebugManager.Log($"Failed Filter: wrong ItemType: {t}", DebugManager.EDebugLevel.Dev, "Inventory");
                    return false;
                }
            }

            return true;
        }

/// <summary>
/// Attempts to set an item stack in a specified slot of an instance.
/// Returns true if the operation was successful; otherwise, false.
/// </summary>
/// <param name="instanceId">The ID of the instance to modify.</param>
/// <param name="slotIndex">The index of the slot to set.</param>
/// <param name="stack">The item stack reference to set, or null to clear the slot.</param>
/// <returns>True if the item stack was set successfully; otherwise, false.</returns>
        public bool TrySetSlot(string instanceId, int slotIndex, ItemStackRef? stack)
        {
            if (!_instances.TryGetValue(instanceId, out var inv) || inv?.slots == null)
                return false;

            if (slotIndex < 0 || slotIndex >= inv.slots.Length)
                return false;

            inv.slots[slotIndex].stack = stack;
            OnSlotChanged?.Invoke(instanceId, slotIndex, stack);
            return true;
        }


        /// <summary>
        /// Determines if the specified item stack can be accepted for the given instance.
        /// </summary>
        /// <param name="instanceId">The ID of the instance to check.</param>
        /// <param name="stack">The item stack to evaluate.</param>
        /// <returns>True if the item stack can be accepted; otherwise, false.</returns>
        public bool CanAccept(string instanceId, in ItemStackRef stack)
        {
            if (!HasInstance(instanceId) || stack.count <= 0) return false;
            var inv = GetInstance(instanceId);
            if (inv == null || inv.slots == null) return false;

            // Instanced items are unstackable -> only empty slots count.
            if (stack.IsInstanced)
            {
                // Hard rule: instanced stacks must be count == 1 (caller error otherwise).
                if (stack.count != 1) return false;

                for (int i = 0; i < inv.slots.Length; i++)
                {
                    var slot = inv.slots[i];
                    if (slot.stack.HasValue) continue;
                    if (!PassesFilter(slot, stack.itemID)) continue;
                    return true;
                }
                return false;
            }

            var need = stack.count;
            var itemId = stack.itemID;

            // 1) fill existing stacks first (same itemID)
            foreach (var slot in inv.slots)
            {
                if (slot.stack.HasValue && slot.stack.Value.itemID == itemId)
                {
                    if (!PassesFilter(slot, itemId)) continue;

                    var free = slot.maxStack - slot.stack.Value.count;
                    if (free > 0)
                    {
                        need -= free;
                        if (need <= 0) return true;
                    }
                }
            }

            // 2) then use empty slots
            foreach (var slot in inv.slots)
            {
                if (!slot.stack.HasValue && PassesFilter(slot, itemId))
                {
                    need -= slot.maxStack;
                    if (need <= 0) return true;
                }
            }

            return false;
        }

/// <summary>
/// Attempts to add an item stack to the inventory.
/// </summary>
/// <param name="instanceId">The ID of the inventory instance.</param>
/// <param name="stack">The item stack to add.</param>
/// <param name="result">The result of the transaction.</param>
/// <returns>True if the item stack was added successfully; otherwise, false.</returns>
        public bool TryAdd(string instanceId, in ItemStackRef stack, out TransactionResult result)
        {
            result = new TransactionResult();
            if (!_instances.TryGetValue(instanceId, out var inv))
            {
                
                result.reason = "InstanceNotFound";
                DebugManager.Log($"Add Item failed: {result.reason}", DebugManager.EDebugLevel.Dev, "Inventory");
                return false;
            }

            // Instanced items are unstackable -> place into one empty slot, preserve instanceId.
            if (stack.IsInstanced)
            {
                if (stack.count != 1)
                {
                    result.reason = "InstancedMustBeCount1";
                    DebugManager.Log($"Add Item failed: {result.reason} ({stack})", DebugManager.EDebugLevel.Dev, "Inventory");
                    return false;
                }

                for (int i = 0; i < inv.slots.Length; i++)
                {
                    var s = inv.slots[i];
                    if (s.stack.HasValue) 
                        continue;

                    if (!PassesFilter(s, stack.itemID)) 
                        continue;

                    s.stack = stack; // keep instanceId
                    result.SlotDeltas.Add((i, s.stack));
                    result.success = true;

                    DebugManager.Log($"Instanced item {stack.itemID} placed in Slot {instanceId}:{i}", DebugManager.EDebugLevel.Debug, "Inventory");
                    OnSlotChanged?.Invoke(instanceId, i, s.stack);
                    return true;
                }

                result.reason = "NoSpace";
                DebugManager.Log($"Add Item {stack.itemID} failed: {result.reason}", DebugManager.EDebugLevel.Dev, "Inventory");
                return false;
            }


            // 1) Versuche bestehende Stacks zu fÃ¼llen
            int remaining = stack.count;
            for (int i = 0; i < inv.slots.Length && remaining > 0; i++)
            {
                var s = inv.slots[i];
                if (s.stack.HasValue && s.stack.Value.itemID == stack.itemID)
                {
                    if (!PassesFilter(s, stack.itemID)) 
                        continue;

                    int space = s.maxStack - s.stack.Value.count;
                    if (space <= 0) 
                        continue;

                    int move = Math.Min(space, remaining);
                    s.stack = s.stack.Value.WithCount(s.stack.Value.count + move);
                    remaining -= move;
                    result.SlotDeltas.Add((i, s.stack));
                    DebugManager.Log($"Item {stack.itemID} ({move}) in Slot {instanceId}:{i} refilled", DebugManager.EDebugLevel.Debug, "Inventory");
                    OnSlotChanged?.Invoke(instanceId, i, s.stack);

                }
            }


            // 2) Leere Slots befÃ¼llen
            for (int i = 0; i < inv.slots.Length && remaining > 0; i++)
            {
                var s = inv.slots[i];
                if (!s.stack.HasValue)
                {
                    if (!PassesFilter(s, stack.itemID))
                        continue;

                    int move = Math.Min(s.maxStack, remaining);
                    s.stack = new ItemStackRef(stack.itemID, move);

                    remaining -= move;
                    result.SlotDeltas.Add((i, s.stack));
                    DebugManager.Log($"Item {stack.itemID} ({move}) in Slot {instanceId}:{i} legen", DebugManager.EDebugLevel.Debug, "Inventory");
                    OnSlotChanged?.Invoke(instanceId, i, s.stack);
                }
            }


            result.success = remaining == 0;
            if (!result.success)
            {
                result.reason = "NoSpace";
                DebugManager.Log($"Add Item {stack.itemID} (remaining: {remaining}) failed: {result.reason}", DebugManager.EDebugLevel.Dev, "Inventory");
            }

            return result.success;
        }

/// <summary>
/// Attempts to move an item from one inventory to another.
/// Returns true if the move is successful, otherwise false.
/// </summary>
/// <param name="req">The request containing details of the move operation.</param>
/// <param name="result">The result of the transaction, including any error reasons.</param>
/// <returns>True if the move was successful; otherwise, false.</returns>
        public bool TryMove(in MoveRequest req, out TransactionResult result)
        {
            result = new TransactionResult();

            if (!_instances.TryGetValue(req.fromInventory.instanceID, out var src) ||
                !_instances.TryGetValue(req.toInventory.instanceID, out var dst))
            { result.reason = "InstanceNotFound"; return false; }

            if (req.toInventory.slot < 0) //search for fitting slot
            {
                var movingstack = Peek(req.fromInventory.instanceID, req.fromInventory.slot);
                if (!movingstack.HasValue)
                {
                    result.reason = "SourceEmpty";
                    return false;
                }

                // Suche passenden Zielslot im Zielinventar
                var dstInv = _instances[req.toInventory.instanceID];
                for (int i = 0; i < dstInv.slots.Length; i++)
                {
                    var slot = dstInv.slots[i];
                    if (!PassesFilter(slot, movingstack.Value.itemID))
                        continue;

                    if (movingstack.Value.IsInstanced)
                    {
                        if (!slot.stack.HasValue)
                        {
                            var newReq = req;
                            newReq.toInventory.slot = i;
                            return TryMove(in newReq, out result);
                        }
                        continue;
                    }

                    // entweder leer ...
                    if (!slot.stack.HasValue)
                    {
                        var newReq = req;
                        newReq.toInventory.slot = i;
                        return TryMove(in newReq, out result);
                    }

                    // ... oder gleicher Stack mit Platz
                    if (slot.stack.Value.itemID == movingstack.Value.itemID && slot.stack.Value.count < slot.maxStack)
                    {
                        var newReq = req;
                        newReq.toInventory.slot = i;
                        return TryMove(in newReq, out result);
                    }
                }

                result.reason = "NoValidTargetSlot";
                return false;
            }

            var a = src.slots[req.fromInventory.slot];
            var b = dst.slots[req.toInventory.slot];

            if (!a.stack.HasValue) { result.reason = "SourceEmpty"; return false; }

            // Menge bestimmen
            var moving = a.stack.Value;
            int amount = req.amount ?? moving.count;

            // Instanced items: cannot Split or Merge; must move whole stack (count==1).
            if (moving.IsInstanced)
            {
                if (req.moveMode == MoveMode.Split || req.moveMode == MoveMode.Merge)
                {
                    result.reason = "InstancedCannotSplitOrMerge";
                    return false;
                }

                if (moving.count != 1 || amount != moving.count)
                {
                    result.reason = "InstancedMustMoveWhole";
                    return false;
                }

                // Also: cannot merge INTO an instanced target stack (same item or not).
                if (b.stack.HasValue && b.stack.Value.IsInstanced && req.moveMode != MoveMode.Swap)
                {
                    result.reason = "InstancedTargetOccupied";
                    return false;
                }
            }

            if (req.moveMode == MoveMode.Split)
            {
                amount = Math.Max(1, moving.count / 2); // halbieren (abrunden, min 1)
            }
            if (amount <= 0) { result.reason = "InvalidAmount"; return false; }
            if (amount > moving.count) amount = moving.count;

            // TODO: SlotFilter-Checks (b.Filter gegen moving.itemID / Tags) â€“ spÃ¤ter
            if (!PassesFilter(b, moving.itemID))
            { 
                result.reason = "FilterFailed"; 
                return false; 
            }

            switch (req.moveMode)
            {
                case MoveMode.Move:
                case MoveMode.Split:
                    {
                        if (!b.stack.HasValue)
                        {
                            // Ziel leer â†’ lege bis max ab
                            int put = Math.Min(b.maxStack, amount);

                            //b.stack = new ItemStackRef(moving.itemID, put);
                            b.stack = moving.WithCount(put);

                            int remain = moving.count - put;
                            a.stack = (remain > 0) ? moving.WithCount(remain) : (ItemStackRef?)null;

                            result.SlotDeltas.Add((req.fromInventory.slot, a.stack));
                            result.SlotDeltas.Add((req.toInventory.slot, b.stack));
                            OnSlotChanged?.Invoke(req.fromInventory.instanceID, req.fromInventory.slot, a.stack);
                            OnSlotChanged?.Invoke(req.toInventory.instanceID, req.toInventory.slot, b.stack);
                            result.success = true;
                            return true;
                        }
                        else
                        {
                            var target = b.stack.Value;

                            if (target.itemID == moving.itemID)
                            {
                                // Merge
                                int space = b.maxStack - target.count;
                                if (space <= 0) { result.reason = "MaxStackReached"; return false; }

                                int put = Math.Min(space, amount);
                                b.stack = target.WithCount(target.count + put);
                                int remain = moving.count - put;
                                a.stack = (remain > 0) ? moving.WithCount(remain) : (ItemStackRef?)null;

                                result.SlotDeltas.Add((req.fromInventory.slot, a.stack));
                                result.SlotDeltas.Add((req.toInventory.slot, b.stack));
                                OnSlotChanged?.Invoke(req.fromInventory.instanceID, req.fromInventory.slot, a.stack);
                                OnSlotChanged?.Invoke(req.toInventory.instanceID, req.toInventory.slot, b.stack);
                                result.success = true;
                                return true;
                            }
                            else if (req.moveMode == MoveMode.Move && amount == moving.count)
                            {
                                // Swap (nur sinnvoll, wenn komplette Quelle bewegt wird)
                                if (!PassesFilter(a, target.itemID))
                                { 
                                    result.reason = "FilterFailed"; 
                                    return false; 
                                }

                                dst.slots[req.toInventory.slot].stack = a.stack;
                                src.slots[req.fromInventory.slot].stack = target;

                                result.SlotDeltas.Add((req.fromInventory.slot, src.slots[req.fromInventory.slot].stack));
                                result.SlotDeltas.Add((req.toInventory.slot, dst.slots[req.toInventory.slot].stack));
                                OnSlotChanged?.Invoke(req.fromInventory.instanceID, req.fromInventory.slot, src.slots[req.fromInventory.slot].stack);
                                OnSlotChanged?.Invoke(req.toInventory.instanceID, req.toInventory.slot, dst.slots[req.toInventory.slot].stack);
                                result.success = true;
                                return true;
                            }
                            else
                            {
                                result.reason = "TargetOccupied";
                                return false;
                            }
                        }
                    }

                case MoveMode.Merge:
                    {
                        if (!b.stack.HasValue) { result.reason = "TargetEmpty"; return false; }
                        var target = b.stack.Value;

                        if (moving.IsInstanced || target.IsInstanced)
                        {
                            result.reason = "InstancedCannotMerge";
                            return false;
                        }

                        if (target.itemID != moving.itemID)
                        { result.reason = "DifferentItem"; return false; }

                        int space = b.maxStack - target.count;
                        if (space <= 0) { result.reason = "MaxStackReached"; return false; }

                        int put = Math.Min(space, amount);
                        b.stack = target.WithCount(target.count + put);
                        int remain = moving.count - put;
                        a.stack = (remain > 0) ? moving.WithCount(remain) : (ItemStackRef?)null;

                        result.SlotDeltas.Add((req.fromInventory.slot, a.stack));
                        result.SlotDeltas.Add((req.toInventory.slot, b.stack));
                        OnSlotChanged?.Invoke(req.fromInventory.instanceID, req.fromInventory.slot, a.stack);
                        OnSlotChanged?.Invoke(req.toInventory.instanceID, req.toInventory.slot, b.stack);
                        result.success = true;
                        return true;
                    }

                case MoveMode.Swap:
                    {
                        if (!PassesFilter(b, moving.itemID) || !PassesFilter(a, b.stack.Value.itemID))
                        {   
                            result.reason = "FilterFailed"; 
                            return false; 
                        }
                        var temp = b.stack;
                        b.stack = a.stack;
                        a.stack = temp;

                        result.SlotDeltas.Add((req.fromInventory.slot, a.stack));
                        result.SlotDeltas.Add((req.toInventory.slot, b.stack));
                        OnSlotChanged?.Invoke(req.fromInventory.instanceID, req.fromInventory.slot, a.stack);
                        OnSlotChanged?.Invoke(req.toInventory.instanceID, req.toInventory.slot, b.stack);
                        result.success = true;
                        return true;
                    }

                default:
                    result.reason = "UnknownMode";
                    return false;
            }
        }

/// <summary>
/// Attempts to remove a specified amount from a given slot of an instance.
/// Returns true if the removal is successful; otherwise, false.
/// </summary>
/// <param name="instanceId">The ID of the instance to modify.</param>
/// <param name="slotIndex">The index of the slot to remove from.</param>
/// <param name="amount">The amount to remove from the slot.</param>
/// <param name="result">The result of the transaction, including any error reasons.</param>
/// <returns>True if the removal was successful; otherwise, false.</returns>
        public bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result)
        {
            result = new TransactionResult();

            if (!_instances.TryGetValue(instanceId, out var inv))
            { result.reason = "InstanceNotFound"; return false; }

            if (slotIndex < 0 || slotIndex >= inv.slots.Length)
            { result.reason = "IndexOutOfRange"; return false; }

            var s = inv.slots[slotIndex];
            if (!s.stack.HasValue) { result.reason = "SourceEmpty"; return false; }
            if (amount <= 0) { result.reason = "InvalidAmount"; return false; }

            var cur = s.stack.Value;
            int take = Math.Min(cur.count, amount);
            int remain = cur.count - take;
            s.stack = (remain > 0) ? cur.WithCount(remain) : (ItemStackRef?)null;


            result.SlotDeltas.Add((slotIndex, s.stack));
            OnSlotChanged?.Invoke(instanceId, slotIndex, s.stack);

            result.success = true;
            return true;
        }

        internal bool TryGetInstance(string inventoryID, out InventoryInstance inst)
        {
            if (string.IsNullOrEmpty(inventoryID))
            {
                inst = null;
                return false;
            }
            return _instances.TryGetValue(inventoryID, out inst);
        }


    }
}
