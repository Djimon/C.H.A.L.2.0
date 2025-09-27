using CHAL.Data;
using CHAL.Systems.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootRollerDebug : MonoBehaviour
{
    private CHAL.Systems.Loot.LootRoller_old roller;

    void Start()
    {
        var rules = new CHAL.Systems.Loot.LootRulesService();
        rules.LoadAll();

        var unlucky = new CHAL.Systems.Loot.UnluckyProtection();
        var roller = new CHAL.Systems.Loot.LootRoller_old(rules, unlucky);

        // Wave definieren
        var wave = new CHAL.Data.WaveComposition
        {
            Level = 3,
            Difficulty = MapDifficulty.Stable,
            Monsters = new List<CHAL.Data.EnemyStruct>
    {
        new CHAL.Data.EnemyStruct
        {
            EnemyId = "Monster1",
            Count = 10,
            bonusTags = new List<string>{ "insect", "swarm" }
        },
        new CHAL.Data.EnemyStruct
        {
            EnemyId = "Monster2",
            Count = 3,
            bonusTags = new List<string>{ "beast", "tank" }
        },
        new CHAL.Data.EnemyStruct
        {
            EnemyId = "Monster3",
            Count = 1,
            bonusTags = new List<string>{ "insect", "boss" }
        }
    }
        };

        // Loot würfeln
        var loot = roller.RollLoot(wave);

        //var grouped = loot.GroupBy(id => id).OrderBy(g => g.Key); // alphabetisch sortieren für Übersicht

        foreach (var entry in loot)
        {
            Debug.Log($" - {entry.ItemId} from {entry.EnemyId} via {entry.PickedTag}");
        }
    }
}
