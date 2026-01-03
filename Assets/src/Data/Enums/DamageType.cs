namespace CHAL.Data
{
    public enum DamageType
    {
        Physical,
        Fire,       // 1 elem resistance for all elements
        Cold,       // 1 elem resistance for all elements
        Lightning,  // 1 elem resistance for all elements
        Earth,      // not uses atm, 1 elem resistance for all elements
        Poison,
        Arcane,
        Daemonic, // only in endgame, resist with Attunement to "Diabolic"
        Holy,     // only in endgame, resist with Attunement to "Seraphic"
        Void,
        Abyssal,


        // … erweiterbar
    }


    public enum Attunement
    { 
        Diabolic    = -4, // ~ 70% Resist against Daemonic Dmg; -70% Vulnerable against Holy Dmg
        Infernal    = -3,
        Fallen      = -2,
        Tainted     = -1,
        Neutral     =  0,
        Blessed     =  1,
        Sanctified  =  2,
        Celestial   =  3,
        Seraphic    = 4   // ~ 70% Resist against Holy Dmg; -70% Vulnerable against Daemonic Dmg 

    }
}