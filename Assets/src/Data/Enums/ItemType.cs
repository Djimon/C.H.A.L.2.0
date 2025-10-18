using System;

namespace CHAL.Data
{

    [Serializable]
    public enum ItemType
    {
        Unknown = 0,
        Remains, // Ressources
        Part, // materials
        Module, //=Skill
        Gear,
        Rune
    }
}
