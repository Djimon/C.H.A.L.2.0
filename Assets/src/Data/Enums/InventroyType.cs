using System;
using UnityEngine;

namespace CHAL.Data
{
    [Serializable]
    public enum PlayerInventoryType
    {
        all,
        Remains,
        Part,
        Rune,
        Module,
        Gear,
        Core
    }
}