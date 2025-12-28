namespace CHAL.Data
{
    public enum DamageType
    {
        Physical,
        Fire,
        Cold,
        Lightning,
        Earth,
        Poison,
        Arcane,
        Daemonic, //cap with Attunement to "Diabolic"
        Holy,     //cap with Attunement to "Seraphic"
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