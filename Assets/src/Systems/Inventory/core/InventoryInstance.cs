
using System;

namespace CHAL.Systems.Inventory
{
/// <summary>
/// Represents an instance of an inventory, containing slots and associated data.
/// </summary>
    public class InventoryInstance
    {
        public string instanceID;
        public InventoryDef InvDef;
        public Slot[] slots;
        public string ownerID;
        public InventoryCapabilities Caps;


        public int SlotCount => slots?.Length ?? 0;


/// <summary>
/// Creates a new instance of InventoryInstance with the specified parameters.
/// </summary>
/// <param name="instanceId">The unique identifier for the inventory instance.</param>
/// <param name="def">The definition of the inventory.</param>
/// <param name="ownerId">The optional owner identifier for the inventory instance.</param>
/// <returns>The newly created InventoryInstance.</returns>
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

