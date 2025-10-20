using CHAL.Data;
using CHAL.Systems.Research;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public sealed class ResearchBootstrap : MonoBehaviour
{
    [Header("Definitions (Inspector)")]
    [SerializeField] private ResearchTreeDef treeDef;
    [SerializeField] private List<ResearchNodeDef> nodeDefs = new List<ResearchNodeDef>();

    [Header("Runtime (read-only)")]
    public ResearchService Service { get; private set; }
    public ResearchUnlockRegistry Registry { get; private set; }
    public ResearchEventBridge Bridge { get; private set; }
    public ResearchState State { get; private set; }

    [Header("Debug (Inspector)")]
    [SerializeField] private MapDifficulty debugMapDifficulty = MapDifficulty.Stable;
    [SerializeField] private EnemyRank debugEnemyRank = EnemyRank.Normal;
    [SerializeField] private List<string> debugEnemyTags = new List<string> { "insectoid" };

    // Falls du Script Execution Order nutzt: stelle sicher, dass dieses Mono vor Verbrauchern startet.

    private void Awake()
    {
        // 1) State erstellen/laden (Phase 5 wird hier echtes Load anschließen)
        State = new ResearchState();

        // 2) Services erzeugen
        Service = new ResearchService();
        Registry = new ResearchUnlockRegistry();
        Bridge = new ResearchEventBridge(Service);

        // Guards
        if (treeDef == null)
        {
            DebugManager.Log("ResearchBootstrap: treeDef fehlt!", DebugManager.EDebugLevel.Dev, "Research", LogType.Error);
        }
        if (nodeDefs == null || nodeDefs.Count == 0)
        {
            DebugManager.Log("ResearchBootstrap: keine Node-Defs zugewiesen.", DebugManager.EDebugLevel.Dev, "Research", LogType.Warning);
        }

        // 3) Research-Service initialisieren
        Service.InitFromTree(treeDef, State);

        // 4) ResearchUnlockRegistry: vollständigen Katalog aufbauen + aus abgeschlossenem State rekonstruieren
        //    HINWEIS: Dein aktueller ResearchUnlockRegistry-Stand besitzt Sets; falls du die Dictionary<string,bool>-Erweiterung eingebaut hast,
        //    rufe hier optional InitializeCatalog(nodeDefs) auf. RebuildFrom reicht meist, weil es intern katalogisiert.
        Registry.RebuildFrom(nodeDefs, State.completedNodeIds);

        // 5) Verkabelung: OnNodeCompleted -> Registry
        Service.OnNodeCompleted += (nodeId, unlocks) =>
        {
            Registry.ApplyNodeUnlocks(nodeId, unlocks);
        };

        DebugManager.Log(
            $"ResearchBootstrap ready. Nodes={nodeDefs?.Count ?? 0}",
            DebugManager.EDebugLevel.Dev, "Research", LogType.Log
        );
    }

    [ContextMenu("Debug/Complete Wave")]
    private void Debug_CompleteWave()
    {
        if (!Application.isPlaying)
        {
            DebugManager.Log("Debug/Complete Wave: nur im Play Mode.", DebugManager.EDebugLevel.Dev, "Research", LogType.Warning);
            return;
        }
        WaveCompleted();
        DebugManager.Log($"Debug WaveCompleted ", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
    }

    [ContextMenu("Debug/Complete Map")]
    private void Debug_CompleteMap()
    {
        if (!Application.isPlaying)
        {
                DebugManager.Log("Debug/Complete Map: nur im Play Mode.", DebugManager.EDebugLevel.Dev, "Research", LogType.Warning);
            return;
        }
        MapCompleted(debugMapDifficulty);
        DebugManager.Log($"Debug MapCompleted diff={debugMapDifficulty}", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
    }

    // --- ContextMenu: Enemy Kill ---
    [ContextMenu("Debug/Kill Enemy")]
    private void Debug_KillEnemy()
    {
        if (!Application.isPlaying)
        {
            DebugManager.Log("Debug/Kill Enemy: nur im Play Mode.", DebugManager.EDebugLevel.Dev, "Research", LogType.Warning);
            return;
        }
        EnemyKilled(debugEnemyTags, debugEnemyRank);
        DebugManager.Log($"Debug EnemyKilled tags=[{string.Join(",", debugEnemyTags)}], rank={debugEnemyRank}", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
    }

    // --------- Bequeme Forwarder (optional) ----------
    // Andere Systeme können die hier rufen, statt direkt an Bridge/Service zu hängen.

    /// <summary>Setzt den aktiven Forschungsknoten, falls verfügbar.</summary>
    public bool SetActiveResearch(string nodeId) => Service.SetActive(nodeId);

    /// <summary>Wave abgeschlossen (Map-Tier).</summary>
    public void WaveCompleted() => Bridge.OnWaveCompleted();

    /// <summary>Map abgeschlossen (Map-Tier + Difficulty).</summary>
    public void MapCompleted(MapDifficulty difficulty) => Bridge.OnMapCompleted(difficulty);

    /// <summary>Enemy kill an Forschung melden (Tags/Rank/Tier).</summary>
    public void EnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank) =>
        Bridge.OnEnemyKilled(enemyTags, rank);
}