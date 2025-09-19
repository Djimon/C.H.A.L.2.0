using CHAL.Data;
using CHAL.Systems.Enemy;
using CHAL.Systems.Loot;
using CHAL.Systems.Loot.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Wave
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Setup")]
        public WaveDef waveDef;
        public Transform spawnPoint;
        public GameObject enemyPrefab;
        public GameObject lootPrefab;

        [Header("Runtime Info")]
        public int currentWaveLevel = 1;
        public int Maplevel = 1;
        public MapDifficulty difficulty = MapDifficulty.Easy;
        public WaveRewards waveRewards;
        //public List<string> currentInventory = new();

        private LootRulesService _rules;
        private LootRoller _roller;
        private UnluckyProtection _unlucky;
        private List<EnemyController> _aliveEnemies = new();
        private WaveLootContext _waveCtx;

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

        [ContextMenu("Start Wave")]
        public void StartWave()
        {
            DebugManager.Log($"Starting Wave {currentWaveLevel}", DebugManager.EDebugLevel.Test, "Wave");

            var wave = waveDef != null ? waveDef.ToComposition() : GetFallbackWave();
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
                    var go = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                    var ec = go.GetComponent<EnemyController>();
                    ec.Init(monster);
                    _aliveEnemies.Add(ec);

                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

        private void HandleEnemyKilled(EnemyController ec, EnemyInstance instance, Vector3 pos)
        {
            _aliveEnemies.Remove(ec);

            waveRewards.AddCurrency("gold", _roller.RollGoldForMonster(instance, _waveCtx));
            waveRewards.AddXP(_roller.RollXPForMonster(instance, Maplevel, difficulty, currentWaveLevel));

            // Loot berechnen
            var drops = _roller.RollLootForMonster(instance, _waveCtx);

            foreach (var d in drops)
            {
                var lootObj = Instantiate(lootPrefab, pos + Vector3.up * 1f, Quaternion.identity);
                var lc = lootObj.GetComponent<LootCube>();
                lc.Init(d.ItemId);
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
                TransferRewardsToProfile(waveRewards);
                DebugManager.Log("Wave Rewards transferred to PlayerProfile",
                    DebugManager.EDebugLevel.Test, "Wave");
            }
            else
            {
                DebugManager.Log("Wave lost – rewards discarded",
                    DebugManager.EDebugLevel.Test, "Wave");
            }

            waveRewards = new WaveRewards(); // reset für nächste Wave
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

            // Speichern
            GameManager.Instance.SaveGame();
        }

        public void CollectLoot(string itemId)
        {
            waveRewards.AddItem(itemId);
            DebugManager.Log($"Collected {itemId}. Inventory now: {waveRewards.Items[itemId]}", DebugManager.EDebugLevel.Debug, "Loot");
        }

        private WaveComposition GetFallbackWave()
        {
            return new WaveComposition
            {
                Level = currentWaveLevel,
                Difficulty = 1f,
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
