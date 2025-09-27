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
        public GameObject enemyFallbackPrefab;
        public GameObject lootPrefab;

        public WaveRewards waveRewards; //??
        private LootRulesService _rules;
        private LootRoller _roller;
        private UnluckyProtection _unlucky;
        private List<EnemyController> _aliveEnemies = new();
        private WaveLootContext _waveCtx;

        private MapManager _MapMangerRef;


        private void Awake()
        {
            _rules = new LootRulesService();
            _rules.LoadAll();

            _unlucky = GameManager.Instance.Unlucky;
            _roller = new LootRoller(_rules, _unlucky);

            EnemyController.OnEnemyKilled += HandleEnemyKilled;
            LootCube.OnLootCollected += CollectLoot;
        }

        private void OnDestroy()
        {
            EnemyController.OnEnemyKilled -= HandleEnemyKilled;
            LootCube.OnLootCollected -= CollectLoot;
        }

        public void StartWave(MapDef mapDef, int waveIndex, MapManager _ref)
        {
            _MapMangerRef = _ref;
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
            //var wave = waveDef.ToComposition(mapDef.baseLevel, mapDef.difficulty);
            var wave = BuildWaveComposition(mapDef, waveDef);

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
                    // optional: Prefab aus EnemyDef (Registry/Lookup) ziehen
                    var prefab = enemyFallbackPrefab; // fallback
                    // prefab = EnemyRegistry.Get(monster.EnemyId)?.prefab ?? enemyPrefab;

                    var go = Instantiate(prefab, SelectSpawnpoint(spawnPoints), Quaternion.identity);
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

        private void HandleEnemyKilled(EnemyController ec, EnemyStruct instance, Vector3 pos)
        {
            _aliveEnemies.Remove(ec);

            waveRewards.AddCurrency("gold", _roller.RollGoldForMonster(instance, _MapMangerRef.CurrentMap.baseLevel));
            waveRewards.AddXP(_roller.RollXPForMonster(instance, _MapMangerRef.CurrentMap.baseLevel, _MapMangerRef.CurrentMap.difficulty, _MapMangerRef .CurrentWave));

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

            _MapMangerRef.OnWaveCompleted(success, waveRewards);
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
            if (_MapMangerRef.CurrentMap != null && _MapMangerRef.CurrentWave == _MapMangerRef.MaxWaves)
            {
                int mapId = _MapMangerRef.CurrentMap.mapId; 
                int difficulty = (int)_MapMangerRef.CurrentMap.difficulty;

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

        private WaveComposition BuildWaveComposition(MapDef mapDef, WaveDef waveDef)
        {
            var wave = new WaveComposition
            {
                Level = mapDef.baseLevel,
                Difficulty = mapDef.difficulty,
                Monsters = new List<EnemyStruct>()
            };

            // Reihenfolge: Spawns → Normals → Magics → Elites → Bosses → Champions
            AddEnemies(wave, mapDef, waveDef.spawnCount, EnemyRank.Spawn);
            AddEnemies(wave, mapDef, waveDef.normalCount, EnemyRank.Normal);
            AddEnemies(wave, mapDef, waveDef.magicCount, EnemyRank.Magic);
            AddEnemies(wave, mapDef, waveDef.eliteCount, EnemyRank.Elite);
            AddEnemies(wave, mapDef, waveDef.bossCount, EnemyRank.Boss);
            AddEnemies(wave, mapDef, waveDef.championCount, EnemyRank.Champion);

            return wave;
        }

        private EnemyStruct UpgradeRank(EnemyDef def, EnemyRank rank, MapDef mapDef)
        {
            var inst = new EnemyStruct
            {
                EnemyId = def.enemyId,
                Rank = rank,
                Count = 1,
                bonusTags = new List<string>(def.baseTags)
            };

            // --- Rank-bedingte Tags ---
            if (rank == EnemyRank.Magic)
            {
                var magicPool = BalanceManager.Instance.Config.enemies.magicTagPool;
                if (magicPool != null && magicPool.Count > 0)
                    inst.bonusTags.Add(magicPool[Random.Range(0, magicPool.Count)]);
            }
            else if (rank == EnemyRank.Elite)
            {
                var minEliteTags = Mathf.Max( // Fallback 4, falls nicht konfiguriert
                    BalanceManager.Instance.Config.enemies.minEliteTags, 4);

                var mods = mapDef.allowedModifiers ?? new List<string>();
                if (mods.Count > 0)
                {
                    // mind. einen Modifier
                    inst.bonusTags.Add(mods[Random.Range(0, mods.Count)]);
                    // auffüllen bis Mindestzahl Tags
                    while (inst.bonusTags.Count < minEliteTags)
                        inst.bonusTags.Add(mods[Random.Range(0, mods.Count)]);
                }
                else
                {
                    DebugManager.Warning("No allowedModifiers on map for Elite promotion.", "Wave");
                }
            }

            // --- (optional) Scaling hier oder im EnemyController.Init() ---
            // Empfehlung: in EnemyController.Init(instance) mit
            // BalanceManager.Instance.Config.enemies.rankScaling arbeiten,
            // damit EnemyInstance schlank bleibt.

            return inst;
        }

        private List<EnemyDef> GetCandidatesForRank(MapDef map, EnemyRank rank)
        {
            var pool = map.allowedEnemies ?? new List<EnemyDef>();
            if (pool.Count == 0) return pool;

            switch (rank)
            {
                case EnemyRank.Spawn:
                case EnemyRank.Boss:
                case EnemyRank.Champion:
                    // diese Ränge sind eigene Assets
                    return pool.FindAll(e => e != null && e.BaseRank == rank);

                case EnemyRank.Normal:
                case EnemyRank.Magic:
                case EnemyRank.Elite:
                    // Promotions stammen aus Normal-Archetypen
                    return pool.FindAll(e => e != null && e.BaseRank == EnemyRank.Normal);

                default:
                    return pool;
            }
        }

        private void AddEnemies(WaveComposition wave, MapDef mapDef, int count, EnemyRank rank)
        {
            if (count <= 0) return;

            var candidates = GetCandidatesForRank(mapDef, rank);
            if (candidates == null || candidates.Count == 0)
            {
                DebugManager.Warning($"No candidates for rank {rank} on map {mapDef.name}. Skipping {count} spawns.",
                    "Wave");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                var baseDef = candidates[Random.Range(0, candidates.Count)];
                var instance = UpgradeRank(baseDef, rank, mapDef);
                wave.Monsters.Add(instance);
            }
        }

        public void SimulateWaveStats(MapDef mapDef, int waveIndex)
        {
            if (mapDef == null || waveIndex < 1 || waveIndex > mapDef.waveDefs.Count)
            {
                DebugManager.Warning("SimulateWaveStats: invalid map/wave index.", "Wave");
                return;
            }

            var wDef = mapDef.waveDefs[waveIndex - 1];
            var wave = BuildWaveComposition(mapDef, wDef); // <- statt ToComposition()

            // UnluckyProtection hier absichtlich frisch, damit Runs unabhängig sind
            var roller = new LootRoller(_rules, new UnluckyProtection());
            WaveSimRunner.RunStats(roller, wave, mapDef.baseLevel, mapDef.difficulty, runs: 100);
        }

        [ContextMenu("Debug/Start Wave (from Inspector)")]
        private void DebugStartWave()
        {
            if (debugMap != null)
            {
                StartWave(debugMap, debugWaveIndex,_MapMangerRef);
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
            DebugManager.Log($"gained {amount} XP", DebugManager.EDebugLevel.Dev, "Fight");
        }
        
    }
}
