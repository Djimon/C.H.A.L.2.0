using CHAL.Data;
using CHAL.Systems.Enemy;
using CHAL.Systems.Unit;
using UnityEngine;

public class DebugEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;   // Prefab mit EnemyController
    public Transform spawnPoint;
    public string enemyId = "debug_rat";

    private void Start()
    {
        var def = UnitRegistry.Instance.GetEnemyByID(enemyId);
        if (def == null)
        {
            DebugManager.Error($"EnemyDef {enemyId} not found!");
            return;
        }

        // Minimaler Struct fürs Debugging
        EnemyStruct data = new EnemyStruct
        {
            EnemyId = enemyId,
            Count = 10,
            bonusTags = new System.Collections.Generic.List<string> { "swarm" },
            Rank = EnemyRank.Normal
        };

        var go = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        var ctrl = go.GetComponent<EnemyController>();
        ctrl.Init(data);
    }
}
