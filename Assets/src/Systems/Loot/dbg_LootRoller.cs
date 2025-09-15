using System.Collections.Generic;
using UnityEngine;

public class LootRollerDebug : MonoBehaviour
{
    private CHAL.Systems.Loot.LootRoller roller;

    void Start()
    {
        var rules = new CHAL.Systems.LootRulesService();
        rules.LoadAll();

        var unlucky = new CHAL.Systems.Loot.UnluckyProtection();
        roller = new CHAL.Systems.Loot.LootRoller(rules, unlucky);

        var tags = new List<string> { "insect", "swarm", "lvl3" };

        var loot = roller.RollLoot(tags, spawns: 12, normals: 4, magics: 2, elites: 0, bosses: 1, champions: 0, level: 3, difficulty: 1.0f);

        Debug.Log("Final Loot:");
        foreach (var item in loot)
        {
            Debug.Log(" - " + item);
        }
    }
}
