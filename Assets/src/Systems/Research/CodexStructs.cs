using UnityEngine;

public struct DeedProgressState
{
    public float progress;
    public bool complieted;
    public bool claimed;   
}

public struct ActiveFocusSlotState
{
    string deedId;
    bool locked;
}

public struct DeedGateState
{
    public bool isVisivle;
    public bool isAvailable;

    public string blockedByDeedId;
    public float blockedByRequProgress;

    public string blockedbyGroupId;
    public float blockedByRequGroupProgress;

}

public struct GroupGateState
{
    public bool isVisivle;
    public bool isAvailable;

    public string blockedByDeedId;
    public float blockedByRequProgress;

    public string blockedbyGroupId;
    public float blockedByRequGroupProgress;

}
