using System;
using UnityEngine;

namespace CHAL.Data
{
    [Serializable]
    public enum PlayerInventoryType
    {
        all = 0,
        Remains,
        Part,
        Rune,
        Module,
        Gear,
        Core,

        //HeroInvetory
        HeroGear = 20,
        HeroSockets
    }
}