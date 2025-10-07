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


        public event Action<string, int, ItemStack?> OnSlotChanged;


        public void RegisterInstance(InventoryInstance inst)
        {
            _instances[inst.instanceID] = inst;
        }

        public ItemStack? Peek(string instanceId, int slotIndex)
        {
            if (!_instances.TryGetValue(instanceId, out var inv)) return null;
            if (slotIndex < 0 || slotIndex >= inv.slots.Length) return null;
            return inv.slots[slotIndex].stack;
        }

        public int SlotCount(string instanceId)
        {
            return _instances.TryGetValue(instanceId, out var inv) ? inv.slots.Length : 0;
        }

        private static bool PassesFilter(Slot slot, string itemId)
        {
            var f = slot.Filter;
            if (f == null) return true;

            // 1) Blocked IDs
            if (f.BlockedItemIds != null)
                for (int i = 0; i < f.BlockedItemIds.Length; i++)
                    if (f.BlockedItemIds[i] == itemId)
                    {
                        DebugManager.Log($"Failed Filter: blocked: {f.BlockedItemIds[i]}", DebugManager.EDebugLevel.Dev, "Inventory");
                        return false;
                    } 

            // 2) Allowed IDs
            if (f.AllowedItemIds != null && f.AllowedItemIds.Length > 0)
            {
                bool ok = false;
                for (int i = 0; i < f.AllowedItemIds.Length; i++)
                    if (f.AllowedItemIds[i] == itemId) { ok = true; break; }
                if (!ok)
                {
                    DebugManager.Log($"failed Filter: not in Allowed: {itemId}", DebugManager.EDebugLevel.Dev, "Inventory");
                    return false;
                }
            }

            // 3) TODO: Tags prüfen?
            

            // 4) Types prüfen
            var t = ItemTypeUtils.FromId(itemId);
            if (f.BlockedItemTypes != null)
                for (int i = 0; i < f.BlockedItemTypes.Length; i++)
                    if (f.BlockedItemTypes[i] == t)
                    {
                        DebugManager.Log($"Failed Filter: blocked ItemType: {t}", DebugManager.EDebugLevel.Dev, "Inventory");
                        return false;
                    }

            if (f.AllowedItemTypes != null && f.AllowedItemTypes.Length > 0)
            {
                bool ok = false;
                for (int i = 0; i < f.AllowedItemTypes.Length; i++)
                    if (f.AllowedItemTypes[i] == t) { ok = true; break; }
                if (!ok)
                {
                    DebugManager.Log($"Failed Filter: wrong ItemType: {t}", DebugManager.EDebugLevel.Dev, "Inventory");
                    return false;
                }
            }

            return true;
        }


        public bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result)
        {
            result = new TransactionResult();
            if (!_instances.TryGetValue(instanceId, out var inv))
            {
                
                result.reason = "InstanceNotFound";
                DebugManager.Log($"Add Item failed: {result.reason}", DebugManager.EDebugLevel.Dev, "Inventory");
                return false;
            }


            // 1) Versuche bestehende Stacks zu füllen
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
                    DebugManager.Log($"Item {stack.itemID} ({move}) in Slot {instanceId}:{i} auffüllen", DebugManager.EDebugLevel.Dev, "Inventory");
                    OnSlotChanged?.Invoke(instanceId, i, s.stack);

                }
            }


            // 2) Leere Slots befüllen
            for (int i = 0; i < inv.slots.Length && remaining > 0; i++)
            {
                var s = inv.slots[i];
                if (!s.stack.HasValue)
                {
                    if (!PassesFilter(s, stack.itemID))
                        continue;

                    int move = Math.Min(s.maxStack, remaining);
                    s.stack = new ItemStack(stack.itemID, move);
                    remaining -= move;
                    result.SlotDeltas.Add((i, s.stack));
                    DebugManager.Log($"Item {stack.itemID} ({move}) in Slot {instanceId}:{i} legen", DebugManager.EDebugLevel.Dev, "Inventory");
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

        public bool TryMove(in MoveRequest req, out TransactionResult result)
        {
            result = new TransactionResult();

            if (!_instances.TryGetValue(req.fromInventory.instanceID, out var src) ||
                !_instances.TryGetValue(req.toInventory.instanceID, out var dst))
            { result.reason = "InstanceNotFound"; return false; }

            if (req.fromInventory.slot < 0 || req.fromInventory.slot >= src.slots.Length ||
                req.toInventory.slot < 0 || req.toInventory.slot >= dst.slots.Length)
            { result.reason = "IndexOutOfRange"; return false; }

            var a = src.slots[req.fromInventory.slot];
            var b = dst.slots[req.toInventory.slot];

            if (!a.stack.HasValue) { result.reason = "SourceEmpty"; return false; }

            // Menge bestimmen
            var moving = a.stack.Value;
            int amount = req.amount ?? moving.count;
            if (req.moveMode == MoveMode.Split)
            {
                amount = Math.Max(1, moving.count / 2); // halbieren (abrunden, min 1)
            }
            if (amount <= 0) { result.reason = "InvalidAmount"; return false; }
            if (amount > moving.count) amount = moving.count;

            // TODO: SlotFilter-Checks (b.Filter gegen moving.itemID / Tags) – später
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
                            // Ziel leer → lege bis max ab
                            int put = Math.Min(b.maxStack, amount);
                            b.stack = new ItemStack(moving.itemID, put);
                            int remain = moving.count - put;
                            a.stack = (remain > 0) ? moving.WithCount(remain) : (ItemStack?)null;

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
                                a.stack = (remain > 0) ? moving.WithCount(remain) : (ItemStack?)null;

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
                        if (target.itemID != moving.itemID)
                        { result.reason = "DifferentItem"; return false; }

                        int space = b.maxStack - target.count;
                        if (space <= 0) { result.reason = "MaxStackReached"; return false; }

                        int put = Math.Min(space, amount);
                        b.stack = target.WithCount(target.count + put);
                        int remain = moving.count - put;
                        a.stack = (remain > 0) ? moving.WithCount(remain) : (ItemStack?)null;

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
            s.stack = (remain > 0) ? cur.WithCount(remain) : (ItemStack?)null;

            result.SlotDeltas.Add((slotIndex, s.stack));
            OnSlotChanged?.Invoke(instanceId, slotIndex, s.stack);

            result.success = true;
            return true;
        }
    }
}