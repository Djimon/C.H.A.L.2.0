using System.Collections.Generic;

namespace CHAL.Systems.Inventory
{
    public class TransactionResult
    {
        public bool success = false;
        public string reason;
        public List<(int slotIndex, ItemStack? newStack)> SlotDeltas = new();
    }

}