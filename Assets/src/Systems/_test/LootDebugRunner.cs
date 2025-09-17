using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CHAL.Data;
using CHAL.Systems.Loot;
using CHAL.Systems.Loot.Models;
using CHAL.Systems.Items;
using CHAL.Systems.Wave;

public class LootDebugRunner : MonoBehaviour
{
    [Header("Test Setup")]
    public WaveDef waveDef;

    [Header("Optional: mehrfach simulieren")]
    [Min(1)] public int runs = 1;

    [Header("Fallback Wave Composition")]
    public int level = 3;
    [Range(1.5f,0.5f)]
    public float difficulty = 1.0f;
    public int spawnCount = 10;
    public int normalCount = 4;
    public int magicCount = 3;
    public int bossCount = 1;

    private UnluckyProtection _unlucky = new UnluckyProtection();

    [ContextMenu("Run Wave Once")]
    public void RunOnce()
    {
        RunInternal(1);
    }

    [ContextMenu("Run Wave (runs times)")]
    public void RunMany()
    {
        RunInternal(runs);
    }

    [ContextMenu("Run Wave From Asset")]
    public void RunFromAsset()
    {
        if (waveDef == null)
        {
            DebugManager.Warning("No WaveDef assigned!", "Loot");
            return;
        }

        var wave = waveDef.ToComposition();
        RunInternal(wave, runs);
    }

    private void RunInternal(int times)
    {
        // 1) Regeln & Roller aufsetzen
        var rules = new LootRulesService();
        rules.LoadAll(); // deine bestehende Lade-Logik (JSON/Scriptables)

        var roller = new LootRoller(rules, _unlucky);

        // 2) Test-Wave definieren (hier rein im Code)
        var wave = new WaveComposition
        {
            Level = level,
            Difficulty = difficulty,
            Monsters = new List<EnemyInstance>
            {
                new EnemyInstance {
                    EnemyId = "Monster0",
                    Count = spawnCount,
                    Tags = new List<string>{"swarm"},
                    Rank = EnemyRank.Spawn
                },
                new EnemyInstance {
                    EnemyId = "Monster1",
                    Count = normalCount,
                    Tags = new List<string>{"insect","swarm"},
                    Rank = EnemyRank.Normal
                },
                new EnemyInstance {
                    EnemyId = "Monster2",
                    Count = magicCount,
                    Tags = new List<string>{"beast","tank"},
                    Rank = EnemyRank.Magic
                },
                new EnemyInstance {
                    EnemyId = "Monster3",
                    Count = bossCount,
                    Tags = new List<string>{"insect","boss"},
                    Rank = EnemyRank.Boss
                }
            }
        };

        // 3) Kontext (Budget, Unlucky, Sammelkorb)
        var ctx = new WaveLootContext(wave);

        // 4) Simulation: alle Gegner „töten“ (Backend, ohne Visuals)
        for (int run = 0; run < times; run++)
        {
            // pro Run einen frischen Kontext, damit Budget/Unlucky zurückgesetzt sind
            ctx = new WaveLootContext(wave);

            foreach (var m in wave.Monsters)
            {
                for (int i = 0; i < m.Count; i++)
                {
                    var drops = roller.RollLootForMonster(m, ctx);
                    foreach (var d in drops)
                        DebugManager.Log($"[KillDrop] {d.ItemId} from {m.EnemyId} ({m.Rank}) via {d.PickedTag}",DebugManager.EDebugLevel.Dev,"Fight");
                }
            }

            // 5) Wellenabschluss (Failsafes/Bonis)
            roller.FinalizeWave(ctx);

            // 6) Ausgabe
            DumpWaveResult(ctx, runIndex: run);
        }
    }

    // NEU: Variante mit WaveComposition
    private void RunInternal(WaveComposition wave, int times)
    {
        // 1) Regeln & Roller aufsetzen
        var rules = new LootRulesService();
        rules.LoadAll();

        var roller = new LootRoller(rules,_unlucky);

        // 2) Simulation: alle Gegner „töten“
        for (int run = 0; run < times; run++)
        {
            // pro Run frischen Kontext erstellen
            var ctx = new WaveLootContext(wave.Clone());

            foreach (var m in wave.Monsters)
            {
                for (int i = 0; i < m.Count; i++)
                {
                    var drops = roller.RollLootForMonster(m, ctx);
                    foreach (var d in drops)
                        DebugManager.Log(
                            $"[KillDrop] {d.ItemId} from {m.EnemyId} ({m.Rank}) via {d.PickedTag}",
                            DebugManager.EDebugLevel.Dev,
                            "Fight"
                        );
                }
            }

            // 3) Wellenabschluss (Failsafes/Bonis)
            roller.FinalizeWave(ctx);

            // 4) Ausgabe
            DumpWaveResult(ctx, runIndex: run);
        }
    }

    private void DumpWaveResult(WaveLootContext ctx, int runIndex)
    {
        DebugManager.Log($"=== Wave Result (Run {runIndex + 1}) ===",DebugManager.EDebugLevel.Debug,"Loot");
        // Gruppiert nach ItemId
        foreach (var g in ctx.Drops.GroupBy(x => x.ItemId).OrderBy(g => g.Key))
        {
            var rarity = ItemRegistry.Instance.GetRarity(g.Key);
            //Debug.Log($" - {g.Count()}x {g.Key} ({rarity})");
        }

        // Kleine Rarity-Zusammenfassung
        var byRarity = ctx.Drops
            .GroupBy(x => ItemRegistry.Instance.GetRarity(x.ItemId))
            .OrderBy(g => g.Key);
        
        foreach (var r in byRarity)
            DebugManager.Log($"{r.Key}: {r.Count()}",DebugManager.EDebugLevel.Debug,"Loot");

        DebugManager.Log($"Budget used: {ctx.SpentBudget} / {ctx.TotalBudget}",DebugManager.EDebugLevel.Debug, "Loot");
    }
}
