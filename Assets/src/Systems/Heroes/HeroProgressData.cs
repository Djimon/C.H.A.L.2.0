using System;

[Serializable]
/// <summary>
/// Represents the progress data for a hero, including experience and level information.
/// </summary>
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
