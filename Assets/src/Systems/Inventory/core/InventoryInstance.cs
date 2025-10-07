
using System;

namespace CHAL.Systems.Inventory
{
    public class InventoryInstance
    {
        public string instanceID;
        public InventoryDef InvDef;
        public Slot[] slots;
        public string ownerID;
        public InventoryCapabilities Caps;


        public int SlotCount => slots?.Length ?? 0;


        public static InventoryInstance Create(string instanceId, InventoryDef def, string ownerId = null)
        {
            var inst = new InventoryInstance
            {
                instanceID = instanceId,
                InvDef = def,
                ownerID = ownerId,
                Caps = InventoryCapabilities.None,
                slots = new Slot[def.cols * def.rows]
            };


            for (int i = 0; i < inst.slots.Length; i++)
            {
                var filter = (def.globalSlotFilter != null )
                    ? def.globalSlotFilter
                    : null;
                inst.slots[i] = new Slot(i, def.defaultMaxStackPerSlot, filter);
            }


            return inst;
        }
    }


    [Flags]
    public enum InventoryCapabilities
    {
        None = 0,
        ReadOnly = 1 << 0,
        Hidden = 1 << 1,
        Locked = 1 << 2
    }
}

