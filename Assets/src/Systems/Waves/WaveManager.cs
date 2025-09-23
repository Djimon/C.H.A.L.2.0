using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Enemy;
using CHAL.Systems.Loot;
using CHAL.Systems.Loot.Models;
using CHAL.Systems.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Wave
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Setup")]
        public WaveDef waveDef;
        public List<Transform> spawnPoints;

        [Header("Debug Input")]
        [SerializeField] private MapDef debugMap;
        [SerializeField] private int debugWaveIndex = 1;
        public GameObject enemyPrefab;
        public GameObject lootPrefab;

        public WaveRewards waveRewards; //??
        private LootRulesService _rules;
        private LootRoller _roller;
        private UnluckyProtection _unlucky;
        private List<EnemyController> _aliveEnemies = new();
        private WaveLootContext _waveCtx;

        private MapManager mapMgr;

        private void Awake()
        {
            _rules = new LootRulesService();
            _rules.LoadAll();

            _unlucky = new UnluckyProtection();
            _roller = new LootRoller(_rules, _unlucky);

            EnemyController.OnEnemyKilled += HandleEnemyKilled;
            LootCube.OnLootCollected += CollectLoot;
        }

        private void OnDestroy()
        {
            EnemyController.OnEnemyKilled -= HandleEnemyKilled;
            LootCube.OnLootCollected -= CollectLoot;
        }

        public void StartWave(MapDef mapDef, int waveIndex)
        {
            DebugManager.Log($"Starting Wave {waveIndex}/{mapDef.maxWaves}", DebugManager.EDebugLevel.Test, "Wave");

            waveRewards = new WaveRewards();

            // Hole WaveDef aus MapDef
            if (mapDef == null || waveIndex < 1 || waveIndex > mapDef.waveDefs.Count)
            {
                DebugManager.Error("Invalid wave index!", "Wave");
                return;
            }

            var waveDef = mapDef.waveDefs[waveIndex - 1];

            // Zusammensetzen (später constraint-basiert, aktuell noch fest)
            var wave = waveDef.ToComposition(mapDef.baseLevel, mapDef.difficulty);

            _waveCtx = new WaveLootContext(wave);
            _aliveEnemies.Clear();

            StartCoroutine(SpawnEnemies(wave));
        }

        private IEnumerator SpawnEnemies(WaveComposition wave)
        {
            foreach (var monster in wave.Monsters)
            {
                for (int i = 0; i < monster.Count; i++)
                {
                    var go = Instantiate(enemyPrefab, SelectSpawnpoint(spawnPoints), Quaternion.identity);
                    var ec = go.GetComponent<EnemyController>();
                    ec.Init(monster);
                    _aliveEnemies.Add(ec);

                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

        public static Vector3 SelectSpawnpoint(List<Transform> spawnPoints)
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
            {
                Debug.LogWarning("Keine Spawnpunkte vorhanden!");
                return Vector3.zero; // Fallback
            }

            int index = Random.Range(0, spawnPoints.Count);
            return spawnPoints[index].position;
        }

        private void HandleEnemyKilled(EnemyController ec, EnemyInstance instance, Vector3 pos)
        {
            _aliveEnemies.Remove(ec);
            mapMgr = MapManager.Instance;

            waveRewards.AddCurrency("gold", _roller.RollGoldForMonster(instance, mapMgr.CurrentMap.baseLevel));
            waveRewards.AddXP(_roller.RollXPForMonster(instance, mapMgr.CurrentMap.baseLevel, mapMgr.CurrentMap.difficulty, mapMgr.CurrentWave));

            // Loot berechnen
            var drops = _roller.RollLootForMonster(instance, _waveCtx);

            foreach (var d in drops)
            {
                Vector3 spawnPos = pos + Vector3.up * 1f 
                       + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
                var lootObj = Instantiate(lootPrefab, spawnPos, Quaternion.identity);
                var lc = lootObj.GetComponent<LootCube>();
                lc.Init(d.ItemId,d.quantity);
            }

            if (_aliveEnemies.Count == 0)
            {
                _roller.FinalizeWave(_waveCtx);
                DebugManager.Log("Wave Completed!", DebugManager.EDebugLevel.Test, "Wave");
                EndWave(true);
            }
        }

        private void EndWave(bool success)
        {
            if (success)
            {
                CollectRemainingLoot(); //TODO: later on Clik /Player Interaction
                TransferRewardsToProfile(waveRewards);
                DebugManager.Log("Wave Rewards transferred to PlayerProfile",
                    DebugManager.EDebugLevel.Test, "Wave");
            }
            else
            {
                DebugManager.Log("Wave lost – rewards discarded",
                    DebugManager.EDebugLevel.Test, "Wave");
            }

            MapManager.Instance.OnWaveCompleted(success);
            waveRewards = new WaveRewards(); // reset für nächste Wave
        }

        public void CollectRemainingLoot()
        {
            int lootLayer = LayerMask.NameToLayer("Loot");
            if (lootLayer < 0)
            {
                DebugManager.Warning("Loot layer not found!", "Loot");
                return;
            }

            // Alle Objekte im Loot-Layer suchen
            var lootObjects = FindObjectsByType<LootCube>(FindObjectsSortMode.None);// GameObject.FindObjectsOfType<LootCube>();

            foreach (var loot in lootObjects)
            {
                if (loot.gameObject.layer != lootLayer) continue;

                DebugManager.Log($"Auto-collecting remaining loot: {loot._itemId} x{loot._quantity}",
                    DebugManager.EDebugLevel.Test, "Loot");

                // Event feuern (wie beim Klick)
                CollectLoot(loot._itemId, loot._quantity);

                // Loot-Objekt zerstören
                GameObject.Destroy(loot.gameObject);
            }
        }


        private void TransferRewardsToProfile(WaveRewards rewards)
        {
            var profile = GameManager.Instance.Profile;

            // Items nach Typ sortieren und ins passende Inventar legen
            foreach (var kv in rewards.Items)
            {
                string itemId = kv.Key;
                int count = kv.Value;

                if (itemId.StartsWith("remain"))
                    profile.Remains.AddItem(itemId, count);
                else if (itemId.StartsWith("part"))
                    profile.Parts.AddItem(itemId, count);
                else if (itemId.StartsWith("rune"))
                    profile.Runes.AddItem(itemId, count);
                else if (itemId.StartsWith("module"))
                    profile.Modules.AddItem(itemId, count);
                else
                    DebugManager.Log($"Unknown item prefix: {itemId}",
                        DebugManager.EDebugLevel.Test, "Inventory");
            }

            // Currencies
            foreach (var kv in rewards.Currencies)
            {
                profile.AddCurrency(kv.Key, kv.Value);
            }

            // XP
            if (rewards.XP > 0)
                profile.AddXP(rewards.XP);


            //Map-Progress
            if (mapMgr?.CurrentMap != null && mapMgr.CurrentWave == mapMgr.MaxWaves)
            {
                int mapId = mapMgr.CurrentMap.mapId; 
                int difficulty = (int)mapMgr.CurrentMap.difficulty;

                if (!profile.MapProgress.ContainsKey(mapId))
                {
                    profile.MapProgress[mapId] = difficulty;
                }
                else if (profile.MapProgress[mapId] < difficulty)
                {
                    profile.MapProgress[mapId] = difficulty;
                }
                DebugManager.Log($"Map {mapId} progress updated to Difficulty {difficulty}",
                        DebugManager.EDebugLevel.Test, "System");
            }

            // Speichern
            GameManager.Instance.SaveGame();
        }

        public void CollectLoot(string itemId, int quantity)
        {
            waveRewards.AddItem(itemId,quantity);
            DebugManager.Log($"Collected {itemId} ({quantity}x). Inventory now: {waveRewards.Items[itemId]}", DebugManager.EDebugLevel.Debug, "Loot");
        }

        private WaveComposition GetFallbackWave()
        {
            return new WaveComposition
            {
                Level = 3,
                Difficulty = 2f,
                Monsters = new List<EnemyInstance>
            {
                new EnemyInstance
                {
                    EnemyId = "FallbackEnemy",
                    Count = 5,
                    Tags = new List<string> {"swarm"},
                    Rank = EnemyRank.Normal
                }
            }
            };
        }

        public void SimulateWaveStats(MapDef mapDef, int waveIndex)
        {
            var wave = waveDef != null ? waveDef.ToComposition(mapDef.baseLevel, mapDef.difficulty) : GetFallbackWave();
            var roller = new LootRoller(_rules, new UnluckyProtection());
            var mapMgr = MapManager.Instance;

            WaveSimRunner.RunStats(roller, wave, mapMgr.CurrentMap.baseLevel, mapMgr.CurrentMap.difficulty, runs: 100);
        }

        [ContextMenu("Debug/Start Wave (from Inspector)")]
        private void DebugStartWave()
        {
            if (debugMap != null)
            {
                StartWave(debugMap, debugWaveIndex);
            }
            else
            {
                DebugManager.Warning("No debug map assigned!", "Wave");
            }
        }

        [ContextMenu("Debug/Simulate Wave Stats (from Inspector)")]
        private void DebugSimulateWaveStats()
        {
            if (debugMap != null)
            {
                SimulateWaveStats(debugMap, debugWaveIndex);
            }
            else
            {
                DebugManager.Warning("No debug map assigned!", "Wave");
            }
        }


    }

    public class WaveRewards
    {
        public Dictionary<string, int> Items = new();          // itemId → count
        public Dictionary<string, int> Currencies = new();     // "gold" → amount
        public int XP;

        public void AddItem(string itemId, int count = 1)
        {
            if (!Items.ContainsKey(itemId))
                Items[itemId] = 0;
            Items[itemId] += count;
        }

        public void AddCurrency(string currencyId, int amount)
        {
            if (!Currencies.ContainsKey(currencyId))
                Currencies[currencyId] = 0;
            Currencies[currencyId] += amount;
        }

        public void AddXP(int amount)
        {
            XP += amount;
        }
    }
}
