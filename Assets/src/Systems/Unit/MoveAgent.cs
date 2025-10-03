using UnityEngine;

public class MoveAgent
{

    public float baseSpeed;
    public float currentSpeed;

    public float stoppingDistance;

    private Vector3 Destination;

    public void SetDestination(Vector3 dest)
    { 
        Destination = dest;
    }

    public void StopOrHold()
    { 

    }

    public bool IsInStoppingRange(Vector3 targetPos)
    {
        return false;
    }


    
}
