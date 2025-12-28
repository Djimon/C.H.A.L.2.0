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
        Seismic,
        Venomous,
        Aetheric, //magic, Arcane
        Infernal, //daemonic/dark
        Radiant,
        Nullified, // void
        Cthonic, //abyssal

    }
}
