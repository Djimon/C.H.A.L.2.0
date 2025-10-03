using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UnitLocator
{
    public List<TargetHandle> activeEnemies;
    public List<TargetHandle> activeHeroes;

    public EffectReceiver GetNearestUnitInSight(Vector3 ownPosition, float sightRange)
    {
        return null;
    }

    public EffectReceiver GetHighestHPUnitInSight(Vector3 ownPosition, float sightRange)
    {
        return null;
    }
    
}


public struct TargetHandle
{
    public Vector3 Position;
    public EffectReceiver unit;

}
