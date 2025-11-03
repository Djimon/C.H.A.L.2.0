using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Enemy;
using CHAL.Systems.Loot;
using CHAL.Systems.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Wave
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Setup")]
        public WaveDef waveDef;
        public List<Transform> EnemySpawnPoints;

        public List<Transform> HeroSpawns;

        [Header("Debug Input")]
        [SerializeField] private MapDef debugMap;
        [SerializeField] private int debugWaveIndex = 1;
        public GameObject enemyFallbackPrefab;
        public GameObject lootPrefab;

        public WaveRewards waveRewards; // Sammelbehälter für diese Wave
        private LootRulesService _rules;
        private LootRoller _roller;
        private UnluckyProtection _unlucky;
        private List<EnemyController> _aliveEnemies = new();
        private WaveLootContext _waveCtx;

        private MapManager _MapMangerRef;

        // --- SubWave-Plan (zur Laufzeit berechnet) ---
        private struct SubWaveSlice
        {
            public int spawn;
            public int normal;
            public int magic;
            public int elite;
            public int boss;
            public int champion;

            public int Total => spawn + normal + magic + elite + boss + champion;
        }
        private List<SubWaveSlice> _subWavePlan; // Index 0..S-1

        private bool _allSubWavesSpawned = false;

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

        // ------------------ PUBLIC API ------------------

        public void StartWave(MapDef mapDef, int waveIndex, MapManager _ref)
        {
            _MapMangerRef = _ref;
            DebugManager.Log($"Starting Wave {waveIndex}/{mapDef.maxWaves}", DebugManager.EDebugLevel.Test, "Wave");

            waveRewards = new WaveRewards();

            // Guard
            if (mapDef == null || waveIndex < 1 || waveIndex > mapDef.waveDefs.Count)
            {
                DebugManager.Error("Invalid wave index!", "Wave");
                return;
            }

            // Hole WaveDef & bau Composition (für Loot-Kontext/Stats)
            var wDef = mapDef.waveDefs[waveIndex - 1];
            var wave = BuildWaveComposition(mapDef, wDef); // behält deine existierende Logik
            _waveCtx = new WaveLootContext(wave);
            _aliveEnemies.Clear();
            _allSubWavesSpawned = false;

            // --- SubWave-Plan vorbereiten (feste Größe pro SubWave + Backloading) ---
            PrepareSubWaveDistribution(mapDef, wDef, out _subWavePlan);

            // --- Run ---
            StopAllCoroutines();
            StartCoroutine(RunWaveRoutine(mapDef, wDef));
        }

        // ------------------ CORE: SUBWAVE LAUF ------------------

        private IEnumerator RunWaveRoutine(MapDef mapDef, WaveDef wDef)
        {
            int S = Mathf.Max(1, mapDef.subWaveCount); // Anzahl SubWaves
            float interDelay = Mathf.Max(0f, mapDef.interSubWaveDelay); 
            int? cap = mapDef.maxConCurrentEnemies > 0 ? mapDef.maxConCurrentEnemies : (int?)null;

            for (int k = 0; k < S; k++)
            {
                // Cap beachten (falls gesetzt)
                if (cap.HasValue)
                {
                    while (_aliveEnemies.Count >= cap.Value)
                        yield return null;
                }

                DebugManager.Log($"SubWave {k + 1}/{S} spawning...", DebugManager.EDebugLevel.Test, "Wave");

                // SubWave k spawnen (Round-Robin nach Rängen, mit Mini-Delay 0.2s wie bisher)
                yield return StartCoroutine(RunSubWaveRoutine(mapDef, wDef, _subWavePlan[k]));

                // Warte zwischen SubWaves
                if (k < S - 1 && interDelay > 0f)
                    yield return new WaitForSeconds(interDelay);
            }

            // Danach: Wave endet wie gehabt, sobald alle Gegner tot sind (HandleEnemyKilled → EndWave)
            DebugManager.Log("All SubWaves spawned. Waiting for cleanup...", DebugManager.EDebugLevel.Test, "Wave");

            _allSubWavesSpawned = true;

            TryEndWave();
        }

        private IEnumerator RunSubWaveRoutine(MapDef mapDef, WaveDef wDef, SubWaveSlice slice)
        {
            // Wir spawnen in Round-Robin-Runden über die Ränge mit Resten,
            // und behalten dein bestehendes Mini-Delay (0.2 s) bei.
            // Reihenfolge: Spawn -> Normal -> Magic -> Elite -> Boss -> Champion (wie in BuildWaveComposition)
            int remainSpawn = slice.spawn;
            int remainNormal = slice.normal;
            int remainMagic = slice.magic;
            int remainElite = slice.elite;
            int remainBoss = slice.boss;
            int remainChampion = slice.champion;

            // Wir wählen pro Rank passende EnemyDefs & bauen pro Spawn einen EnemyStruct (Count=1), wie bisher.
            while (remainSpawn + remainNormal + remainMagic + remainElite + remainBoss + remainChampion > 0)
            {
                if (remainSpawn > 0) { SpawnOne(mapDef, EnemyRank.Spawn); remainSpawn--; yield return new WaitForSeconds(0.2f); }
                if (remainNormal > 0) { SpawnOne(mapDef, EnemyRank.Normal); remainNormal--; yield return new WaitForSeconds(0.2f); }
                if (remainMagic > 0) { SpawnOne(mapDef, EnemyRank.Magic); remainMagic--; yield return new WaitForSeconds(0.2f); }
                if (remainElite > 0) { SpawnOne(mapDef, EnemyRank.Elite); remainElite--; yield return new WaitForSeconds(0.2f); }
                if (remainBoss > 0) { SpawnOne(mapDef, EnemyRank.Boss); remainBoss--; yield return new WaitForSeconds(0.2f); }
                if (remainChampion > 0) { SpawnOne(mapDef, EnemyRank.Champion); remainChampion--; yield return new WaitForSeconds(0.2f); }
            }
        }

        private void SpawnOne(MapDef mapDef, EnemyRank rank)
        {
            var candidates = GetCandidatesForRank(mapDef, rank);
            if (candidates == null || candidates.Count == 0)
            {
                DebugManager.Warning($"No candidates for rank {rank} on map {mapDef.name}. Skipping spawn.", "Wave");
                return;
            }

            var baseDef = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            var instance = UpgradeRank(baseDef, rank, mapDef);

            GameObject prefab = GetEnemyPrefab(baseDef);

            var go = Instantiate(prefab, SelectSpawnpoint(EnemySpawnPoints), Quaternion.identity);
            var ec = go.GetComponent<EnemyController>();
            ec.Init(instance);
            _aliveEnemies.Add(ec);
        }

        private GameObject GetEnemyPrefab(EnemyDef baseDef)
        {
            // TODO: über Registry/Lookup aus EnemyDef ziehen?
            GameObject go = baseDef.prefab;
            if (go == null)
                return enemyFallbackPrefab;
            else
                return go;
        }

        // ------------------ SUBWAVE PLANUNG ------------------

        private void PrepareSubWaveDistribution(MapDef mapDef, WaveDef wDef, out List<SubWaveSlice> plan)
        {
            plan = new List<SubWaveSlice>();

            int S = Mathf.Max(1, mapDef.subWaveCount);
            // Totals je Rank aus WaveDef
            int totSpawn = Mathf.Max(0, wDef.spawnCount);
            int totNormal = Mathf.Max(0, wDef.normalCount);
            int totMagic = Mathf.Max(0, wDef.magicCount);
            int totElite = Mathf.Max(0, wDef.eliteCount);
            int totBoss = Mathf.Max(0, wDef.bossCount);
            int totChampion = Mathf.Max(0, wDef.championCount);

            int total = totSpawn + totNormal + totMagic + totElite + totBoss + totChampion;

            // Feste Zielgröße pro SubWave (gleichmäßig, Rest in die ersten Runden)
            int baseT = total / S;
            int restT = total % S; // die ersten restT SubWaves bekommen +1

            // Vorbereiten der CDF-Inkremente (Delta je SubWave/Rang) mit Backloading
            int[] dSpawn = BuildBackloadedDeltas(totSpawn, S, wDef.backload.GetSpawnDelayAlpha(EnemyRank.Spawn));
            int[] dNormal = BuildBackloadedDeltas(totNormal, S, wDef.backload.GetSpawnDelayAlpha(EnemyRank.Normal));
            int[] dMagic = BuildBackloadedDeltas(totMagic, S, wDef.backload.GetSpawnDelayAlpha(EnemyRank.Magic));
            int[] dElite = BuildBackloadedDeltas(totElite, S, wDef.backload.GetSpawnDelayAlpha(EnemyRank.Elite));
            int[] dBoss = BuildBackloadedDeltas(totBoss, S, wDef.backload.GetSpawnDelayAlpha(EnemyRank.Boss));
            int[] dChampion = BuildBackloadedDeltas(totChampion, S, wDef.backload.GetSpawnDelayAlpha(EnemyRank.Champion));

            // Jetzt pro SubWave die Deltas auf die Zielgröße T_k balancieren:
            // - Unterfüllung: zuerst Normal, dann Magic auffüllen (falls deren Reste übrig sind)
            // - Überfüllung: zuerst Normal, dann Magic reduzieren (nie Boss/Champion/Elite kürzen)
            for (int k = 0; k < S; k++)
            {
                int Tk = baseT + (k < restT ? 1 : 0);

                int s = dSpawn[k];
                int n = dNormal[k];
                int m = dMagic[k];
                int e = dElite[k];
                int b = dBoss[k];
                int c = dChampion[k];

                int sum = s + n + m + e + b + c;

                if (sum < Tk)
                {
                    int need = Tk - sum;
                    // Auffüllen in Reihenfolge Normal -> Magic
                    int addN = Mathf.Min(need, RemainingForRank(k, dNormal, totNormal));
                    n += addN; need -= addN;

                    if (need > 0)
                    {
                        int addM = Mathf.Min(need, RemainingForRank(k, dMagic, totMagic));
                        m += addM; need -= addM;
                    }

                    // Falls immer noch Bedarf (extremer Randfall): fülle Spawn
                    if (need > 0)
                    {
                        int addS = Mathf.Min(need, RemainingForRank(k, dSpawn, totSpawn));
                        s += addS; need -= addS;
                    }
                }
                else if (sum > Tk)
                {
                    int over = sum - Tk;
                    // Kürzen in Reihenfolge Normal -> Magic (niemals Boss/Elite/Champion kürzen, um Backloading zu erhalten)
                    int cutN = Mathf.Min(over, n);
                    n -= cutN; over -= cutN;

                    if (over > 0)
                    {
                        int cutM = Mathf.Min(over, m);
                        m -= cutM; over -= cutM;
                    }

                    if (over > 0)
                    {
                        int cutS = Mathf.Min(over, s); // als allerletztes Spawn kürzen
                        s -= cutS; over -= cutS;
                    }
                }

                plan.Add(new SubWaveSlice
                {
                    spawn = s,
                    normal = n,
                    magic = m,
                    elite = e,
                    boss = b,
                    champion = c
                });
            }

            // Safety: Summen prüfen (sollten exakt passen)
            int chkS = 0, chkN = 0, chkM = 0, chkE = 0, chkB = 0, chkC = 0;
            for (int k = 0; k < S; k++)
            {
                chkS += plan[k].spawn;
                chkN += plan[k].normal;
                chkM += plan[k].magic;
                chkE += plan[k].elite;
                chkB += plan[k].boss;
                chkC += plan[k].champion;
            }

            if (chkS != totSpawn || chkN != totNormal || chkM != totMagic || chkE != totElite || chkB != totBoss || chkC != totChampion)
            {
                DebugManager.Warning($"SubWave plan mismatch (S:{chkS}/{totSpawn}, N:{chkN}/{totNormal}, M:{chkM}/{totMagic}, E:{chkE}/{totElite}, B:{chkB}/{totBoss}, C:{chkC}/{totChampion})", "Wave");
            }
        }

        // Baut die CDF-basierten Inkremente (Delta je SubWave) mit Backloading-Exponent alpha (0..5).
        private static int[] BuildBackloadedDeltas(int total, int S, float alpha)
        {
            int[] result = new int[S];
            if (total <= 0 || S <= 0) return result;

            // Gewichte w_k = (k/S)^alpha, k=1..S
            double[] w = new double[S];
            double sumW = 0.0;
            for (int k = 1; k <= S; k++)
            {
                double wk = Mathf.Pow(k / S, alpha);
                w[k - 1] = wk;
                sumW += wk;
            }

            // CDF Ziele kumulativ runden und zu Deltas differenzieren
            int prev = 0;
            double cum = 0.0;
            for (int k = 0; k < S; k++)
            {
                cum += w[k] / sumW;
                int targetCum = (int)System.Math.Round(total * cum, System.MidpointRounding.AwayFromZero);
                int delta = targetCum - prev;
                result[k] = Mathf.Max(0, delta);
                prev = targetCum;
            }

            return result;
        }

        // Wie viele Einheiten dieses Ranges sind ab (inkl.) SubWave k noch „frei“ (für Auffüllen), gemessen an Total?
        private static int RemainingForRank(int k, int[] deltas, int total)
        {
            int used = 0;
            for (int i = 0; i < k; i++) used += deltas[i];
            return Mathf.Max(0, total - used - deltas[k]);
        }

        // ------------------ EXISTIERENDE LOGIK: Loot/Ende ------------------

        private void HandleEnemyKilled(EnemyController ec, EnemyDef def, EnemyStruct instance, Vector3 pos)
        {
            _aliveEnemies.Remove(ec);

            waveRewards.AddCurrency("gold", _roller.RollGoldForMonster(instance, _MapMangerRef.CurrentMap.baseLevel));
            waveRewards.AddXP(_roller.RollXPForMonster(instance, _MapMangerRef.CurrentMap.baseLevel, _MapMangerRef.CurrentMap.difficulty, _MapMangerRef.CurrentWave));

            var drops = _roller.RollLootForMonster(def, instance, _waveCtx);
            foreach (var d in drops)
            {
                Vector3 spawnPos = pos + Vector3.up * 2f
                                   + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0, UnityEngine.Random.Range(-0.2f, 0.2f));
                var lootObj = Instantiate(lootPrefab, spawnPos, Quaternion.identity);
                var lc = lootObj.GetComponent<LootCube>();
                lc.Init(d.ItemId, d.quantity);
            }

                TryEndWave();
        }

        private void TryEndWave()
        {
            if (_allSubWavesSpawned && _aliveEnemies.Count == 0)
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
                CollectRemainingLoot();
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
            waveRewards = new WaveRewards(); // reset
        }

        public void CollectRemainingLoot()
        {
            int lootLayer = LayerMask.NameToLayer("Loot");
            if (lootLayer < 0)
            {
                DebugManager.Warning("Loot layer not found!", "Loot");
                return;
            }

            var lootObjects = FindObjectsByType<LootCube>(FindObjectsSortMode.None);
            foreach (var loot in lootObjects)
            {
                if (loot.gameObject.layer != lootLayer) continue;

                DebugManager.Log($"Auto-collecting remaining loot: {loot._itemId} x{loot._quantity}",
                    DebugManager.EDebugLevel.Test, "Loot");

                CollectLoot(loot._itemId, loot._quantity);
                GameObject.Destroy(loot.gameObject);
            }
        }

        private void TransferRewardsToProfile(WaveRewards rewards)
        {
            var gm = GameManager.Instance;
            var profile = gm.Profile;
            var domain = gm.Inventory;

            if (profile == null || domain == null || rewards?.Items == null) return;       

            int applied = 0, unknown = 0;

            // 1) Items in die DOMAIN buchen
            foreach (var kv in rewards.Items)
            {
                // robust gegen Whitespaces/unsichtbare Zeichen
                var itemId = kv.Key?.Trim();
                int count = kv.Value;
                if (string.IsNullOrEmpty(itemId)) continue;

                // ZENTRAL: enum-basierter Resolver aus dem GameManager
                if (!gm.TryResolveByItemId(itemId, out var invType, out var instanceId))
                {
                    DebugManager.Log(
                        $"Unknown inventory prefix for itemId='{itemId}'",
                        DebugManager.EDebugLevel.Test, "Inventory", LogType.Warning);
                    unknown++;
                    continue;
                }

                // Instanz sicherstellen (falls noch nicht vorhanden)
                gm.EnsureInstance(instanceId, invType);

                // In die DOMAIN buchen
                var ok = domain.TryAdd(instanceId, new ItemStack(itemId, count), out var tx);
                if (!ok)
                {
                    DebugManager.Log($"TryAdd failed for {itemId} x{count} → {tx.reason}",
                        DebugManager.EDebugLevel.Dev, "Inventory", LogType.Warning);
                }
                else
                {
                    applied++;
                }
            }

            foreach (var kv in rewards.Currencies)
                profile.AddCurrency(kv.Key, kv.Value);

            if (rewards.XP > 0)
                profile.AddXP(rewards.XP);

            if (_MapMangerRef.CurrentMap != null && _MapMangerRef.CurrentWave == _MapMangerRef.MaxWaves)
            {
                int mapId = _MapMangerRef.CurrentMap.mapId;
                int difficulty = (int)_MapMangerRef.CurrentMap.difficulty;

                if (!profile.MapProgress.ContainsKey(mapId))
                    profile.MapProgress[mapId] = difficulty;
                else if (profile.MapProgress[mapId] < difficulty)
                    profile.MapProgress[mapId] = difficulty;

                DebugManager.Log($"Map {mapId} progress updated to Difficulty {difficulty}",
                        DebugManager.EDebugLevel.Test, "System");
            }

            gm.SaveGame();
        }

        public void CollectLoot(string itemId, int quantity)
        {
            waveRewards.AddItem(itemId, quantity);
            DebugManager.Log($"Collected {itemId} ({quantity}x). Inventory now: {waveRewards.Items[itemId]}", DebugManager.EDebugLevel.Debug, "Loot");
        }

        // ------------------ DEIN BESTEHENDER AUFBAU (leicht angepasst genutzt) ------------------

        private WaveComposition BuildWaveComposition(MapDef mapDef, WaveDef waveDef)
        {
            var wave = new WaveComposition
            {
                Level = mapDef.baseLevel,
                Difficulty = mapDef.difficulty,
                Monsters = new List<EnemyStruct>()
            };

            // Reihenfolge wie bisher (nur für Loot-Kontext/Stats; Spawns selber passieren aus dem SubWave-Plan)
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

            if (rank == EnemyRank.Magic)
            {
                var magicPool = BalanceManager.Instance.Config.enemies.magicTagPool;
                if (magicPool != null && magicPool.Count > 0)
                    inst.bonusTags.Add(magicPool[UnityEngine.Random.Range(0, magicPool.Count)]);
            }
            else if (rank == EnemyRank.Elite)
            {
                var minEliteTags = Mathf.Max(BalanceManager.Instance.Config.enemies.minEliteTags, 4);
                var mods = mapDef.allowedModifiers ?? new List<string>();
                if (mods.Count > 0)
                {
                    inst.bonusTags.Add(mods[UnityEngine.Random.Range(0, mods.Count)]);
                    while (inst.bonusTags.Count < minEliteTags)
                        inst.bonusTags.Add(mods[UnityEngine.Random.Range(0, mods.Count)]);
                }
                else
                {
                    DebugManager.Warning("No allowedModifiers on map for Elite promotion.", "Wave");
                }
            }
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
                var baseDef = candidates[UnityEngine.Random.Range(0, candidates.Count)];
                var instance = UpgradeRank(baseDef, rank, mapDef);
                wave.Monsters.Add(instance);
            }
        }

        public static Vector3 SelectSpawnpoint(List<Transform> spawnPoints)
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
            {
                DebugManager.Warning("Keine Spawnpunkte vorhanden!");
                return Vector3.zero;
            }

            int index = UnityEngine.Random.Range(0, spawnPoints.Count);
            return spawnPoints[index].position;
        }

        [ContextMenu("Debug/Start Wave (from Inspector)")]
        private void DebugStartWave()
        {
            if (debugMap != null)
            {
                StartWave(debugMap, debugWaveIndex, _MapMangerRef);
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

        public void SimulateWaveStats(MapDef mapDef, int waveIndex)
        {
            if (mapDef == null || waveIndex < 1 || waveIndex > mapDef.waveDefs.Count)
            {
                DebugManager.Warning("SimulateWaveStats: invalid map/wave index.", "Wave");
                return;
            }

            var wDef = mapDef.waveDefs[waveIndex - 1];
            var wave = BuildWaveComposition(mapDef, wDef); // unverändert

            // UnluckyProtection hier absichtlich frisch, damit Runs unabhängig sind
            var roller = new LootRoller(_rules, new UnluckyProtection());
            //WaveSimRunner.RunStats(roller, wave, mapDef.baseLevel, mapDef.difficulty, runs: 100);
        }

    }

    public class WaveRewards
    {
        public Dictionary<string, int> Items = new();          // itemId → count
        public Dictionary<string, int> Currencies = new();     // "gold" → amount
        public int XP;

        public void AddItem(string itemId, int count = 1)
        {
            if (!Items.ContainsKey(itemId)) Items[itemId] = 0;
            Items[itemId] += count;
        }

        public void AddCurrency(string currencyId, int amount)
        {
            if (!Currencies.ContainsKey(currencyId)) Currencies[currencyId] = 0;
            Currencies[currencyId] += amount;
        }

        public void AddXP(int amount)
        {
            XP += amount;
            DebugManager.Log($"gained {amount} XP", DebugManager.EDebugLevel.Dev, "Combat");
        }
    }
}
