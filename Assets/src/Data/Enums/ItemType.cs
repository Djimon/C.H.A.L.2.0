using System;

namespace CHAL.Data
{

    [Serializable]
    public enum ItemType
    {
        Unknown = 0,
        Remains, // Ressources
        Part, // materials
        Core,
        Module, //=Skill
        Gear,
        Rune
    }


    [Serializable]
    public enum CoreType
    { 
        Basic = 0, //Kinetic
        Blazing,
        Glacial,
        Static,
        Venomous,
        Aetheric,
        Infernal,
        Radiant
    }
}
