using CHAL.Systems.Research;
using UnityEngine;

/// <summary>
/// Persistenter Progress-Zustand pro Deed (Source of Truth).
/// progress01 ist UI-freundlich, aber darf aus counters abgeleitet/aktualisiert werden.
/// </summary>
public struct DeedProgressState
{
    public float progress01;
    public bool completed;
    public bool claimed;

    public DeedProgress counters;
}

public struct ActiveFocusSlotState
{
    public string deedId;
    public bool locked;
}

public struct DeedGateState
{
    public bool isVisible;
    public bool isAvailable;

    public string blockedByDeedId;
    public float blockedByRequProgress01;

    public string blockedbyGroupId;
    public float blockedByRequGroupProgress01;

}

public struct GroupGateState
{
    public bool isVisible;

    public float completion01;         // claimedCount/total
    public float requiredCompletion01; // visibleAfterCompletion01

    public int dependsOnGroupId; 
}
