using CHAL.Core;
using CHAL.Data;
using UnityEngine;

namespace CHAL.Systems.Inventory
{
    [CreateAssetMenu(fileName = "Inventory Def", menuName = "Data/Inventory Def")]
    public class InventoryDef : ScriptableObject
    {
        public PlayerInventoryType TypeId;
        public string NameKey;
        [Min(1)]  public int cols;
        [Min(1)]  public int rows;
        public int defaultMaxStackPerSlot = 250;

        public SlotFilter globalSlotFilter;
    }

}