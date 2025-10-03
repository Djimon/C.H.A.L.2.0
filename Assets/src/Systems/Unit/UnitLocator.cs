using CHAL.Systems.Enemy;
using CHAL.Systems.Hero;
using System.Collections.Generic;
using UnityEngine;

public class UnitLocator
{
    public List<EnemyController> activeEnemies;
    public List<HeroController> activeHeroes;

    public void RegisterEnemy(EnemyController e)
    { 
        //TODO get Position and add to List
    }

    public void UnregisterEnemy(EnemyController e) 
    {
        //TODO: Rmeove from List
    }

    public void RegisterHero(HeroController h)
    {
        //TODO get Position and add to List
    }

    public void UnregisterHero(HeroController h)
    {
        //TODO: Rmeove from List
    }

    public Transform GetNearestUnitInSight(Vector3 ownPosition, float sightRange)
    {
        return null;
    }

    public Transform GetHighestHPUnitInSight(Vector3 ownPosition, float sightRange)
    {
        return null;
    }
    
}


