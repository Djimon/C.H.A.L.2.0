using System;

namespace CHAL.Systems.Inventory
{
/// <summary>
/// Defines the interface for inventory domain operations.
/// </summary>
    public interface IInventoryDomain
    {
        bool CanAccept(string instanceId, in ItemStackRef stack);
        bool TryAdd(string instanceId, in ItemStackRef stack, out TransactionResult result);
        bool TryMove(in MoveRequest req, out TransactionResult result);
        bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result);

        event Action<string, int, ItemStackRef?> OnSlotChanged; // (instanceId, slot, newStack)

        ItemStackRef? Peek(string instanceId, int slotIndex);
        int SlotCount(string instanceId);
    }
}
