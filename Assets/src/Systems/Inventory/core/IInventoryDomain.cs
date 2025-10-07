using System;

namespace CHAL.Systems.Inventory
{
    public interface IInventoryDomain
    {
        bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result);
        bool TryMove(in MoveRequest req, out TransactionResult result);
        bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result);

        event Action<string, int, ItemStack?> OnSlotChanged; // (instanceId, slot, newStack)

        ItemStack? Peek(string instanceId, int slotIndex);
        int SlotCount(string instanceId);
    }
}