using System;

[Serializable]
public class HeroProgressData
{
    public string HeroId;           // referenziert HeroDef
    public int Level;
    public int CurrentXP;
    public int TotalXP;             // optional
    public int TotalOrbitPoints;
    public int UnspentOrbitPoints;
    public int UnlockedSockets;

    // Später evtl.:
    // public GearLoadoutDTO Gear;
    // public bool IsUnlocked;
    // public string Nickname;
}
