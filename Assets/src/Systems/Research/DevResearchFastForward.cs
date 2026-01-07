#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHAL.Core;               // DebugManager
using CHAL.Data;               // ResearchTreeDef, MapDifficulty, EnemyRank
using CHAL.Systems.Research;
using Unity.VisualScripting;   // ResearchMapView, ResearchService, ResearchEventBridge, ResearchTreeCompiler


public sealed class DevResearchFastForward : MonoBehaviour
{
    [Header("Wiring (optional)")]
    public ResearchMapView mapView;
    public CodexTreeDef treeDef;

    [Header("Modus bei Play (alle optional)")]
    public bool completeAllOnPlay = false;
    public int completeUpToStage = -1;
    public List<string> extraNodeIds = new List<string>();

    [Header("Event-Simulation (Heuristik)")]
    public MapDifficulty fallbackDifficulty = MapDifficulty.Stable;
    public EnemyRank fallbackKillRank = EnemyRank.Normal;
    public List<string> fallbackKillTags = new List<string>();

    [Header("Nach dem Anwenden")]
    public bool rebuildMapAfterApply = true;

    // --- intern ---
    private CodexService _service;
    private CodexTreeDef _tree;

    private void Start()
    {
        StartCoroutine(WaitAndMaybeApply());
    }

    private IEnumerator WaitAndMaybeApply()
    {
        // Service/Tree/Bridge finden (ein paar Frames warten, falls Bootstrap/GM später init.)
        for (int i = 0; i < 60; i++)
        {
            if (TryResolve()) break;
            yield return null;
        }

        if (_service == null )
        {
            DebugManager.Log("DevResearchFastForward: Service or bridge not found – aborted.",
                DebugManager.EDebugLevel.Dev, "Research", LogType.Warning);
            yield break;
        }

        if (!completeAllOnPlay && completeUpToStage < 0 && (extraNodeIds == null || extraNodeIds.Count == 0))
            yield break; // nichts zu tun

        ApplyCheats();

        if (rebuildMapAfterApply && mapView != null)
        {
            mapView.BuildMap();
            mapView.CenterOnActiveOrFirst();
        }
    }

    private bool TryResolve()
    {
        if (mapView == null)
            mapView = FindFirstObjectByType<ResearchMapView>();

        if (mapView != null)
        {
            _service = mapView.serviceRef;
            if (treeDef == null) treeDef = mapView.treeDef;
        }

        _tree = treeDef;
        return _service != null;
    }

    private void ApplyCheats()
    {
        int totalOps = 0;

        if (completeAllOnPlay && _tree != null)
            totalOps += CompleteAll();

        if (completeUpToStage >= 0 && _tree != null)
            totalOps += CompleteUpToStage(completeUpToStage);

        if (extraNodeIds != null && extraNodeIds.Count > 0)
            totalOps += CompleteIds(extraNodeIds);

        DebugManager.Log($"DevResearchFastForward: angewandt – {totalOps} Operation(en).",
            DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
    }

    // ---------- öffentliche Context-Menüs für den Playmode ----------

    [ContextMenu("DEV/Complete ALL Now")]
    private void Ctx_CompleteAll() { if (TryResolve()) { var n = CompleteAll(); Post(n); } }

    [ContextMenu("DEV/Complete Up To Stage Now")]
    private void Ctx_CompleteUpToStage()
    {
        if (completeUpToStage < 0) { DebugManager.Log("completeUpToStage < 0 (aus).", DebugManager.EDebugLevel.Dev, "Research", LogType.Warning); return; }
        if (!TryResolve()) return;
        var n = CompleteUpToStage(completeUpToStage);
        Post(n);
    }

    [ContextMenu("DEV/Complete Extra IDs Now")]
    private void Ctx_CompleteExtra()
    {
        if (!TryResolve()) return;
        var n = CompleteIds(extraNodeIds);
        Post(n);
    }

    [ContextMenu("DEV/Save cheated progress")]
    private void SaveCheatedResearchProgress()
    {
        var Profile = GameManager.Instance.Profile;
        SaveSystem.SaveResearch(Profile.profileId, Profile.BuildResearchSnapshotFrom(Profile.ResearchRuntime));
    }

    private void Post(int ops)
    {
        DebugManager.Log($"DevResearchFastForward Context: {ops} Operation(en).",
            DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
        if (rebuildMapAfterApply && mapView != null)
        {
            mapView.BuildMap();
            mapView.CenterOnActiveOrFirst();
        }
    }

    // ---------- Kernlogik: Events simulieren ----------

    private int CompleteAll()
    {
        int ops = 0;
        var compiled = CodexTreeCompiler.Compile(_tree);
        // Elternabhängigkeiten werden durch natürliche Reihenfolge nicht garantiert,
        // aber SetActive prüft IsNodeAvailable. Wir lassen es den Service entscheiden.
        foreach (var id in compiled.nodesById.Keys)
            ops += CompleteNode(id);
        return ops;
    }

    private int CompleteUpToStage(int stage)
    {
        int ops = 0;
        var compiled = CodexTreeCompiler.Compile(_tree);
        foreach (var kv in compiled.posById)
        {
            var id = kv.Key;
            var st = kv.Value.stage;
            if (st <= stage) ops += CompleteNode(id);
        }
        return ops;
    }

    private int CompleteIds(List<string> ids)
    {
        if (ids == null) return 0;
        int ops = 0;
        foreach (var id in ids)
            ops += CompleteNode(id);
        return ops;
    }


    private int CompleteNode(string nodeId)
    {
        if (string.IsNullOrWhiteSpace(nodeId)) return 0;
        if (_service.IsCompleted(nodeId)) return 0;

        // Versuche den Knoten aktiv zu setzen (Elternabhängigkeiten werden hier geprüft).
        if (!_service.SetActive(nodeId))
            return 0;

        var def = _service.GetNodeDef(nodeId);
        if (def == null || def.requirements == null) return 0;

        var req = def.requirements;
        var prog = _service.GetNodeProgress(nodeId);

        // 1) Waves
        if (req.waves > 0)
        {
            int missing = Mathf.Max(0, req.waves - prog.waves);
            for (int i = 0; i < missing && !_service.IsCompleted(nodeId); i++)
                _service.OnWaveCompleted(1,i,MapDifficulty.Stable);
        }

        // 2) Maps per Difficulty zuerst (spezifisch)
        if (req.mapRequirements != null)
        {
            foreach (var mr in req.mapRequirements)
            {
                if (mr.amount <= 0) continue;
                int cur = 0;
                if (prog.mapsByDifficulty != null && prog.mapsByDifficulty.TryGetValue(mr.difficulty, out var c))
                    cur = c;

                int missing = Mathf.Max(0, mr.amount - cur);
                for (int i = 0; i < missing && !_service.IsCompleted(nodeId); i++)
                    _service.OnMapCompleted(i,mr.difficulty);
            }
        }

        // 3) Maps total (Restbedarf)
        if (req.maps > 0 && !_service.IsCompleted(nodeId))
        {
            int missing = Mathf.Max(0, req.maps - _service.GetNodeProgress(nodeId).mapsTotal);
            for (int i = 0; i < missing && !_service.IsCompleted(nodeId); i++)
                _service.OnMapCompleted(i,MapDifficulty.Stable);
        }

        // 4) Elites/Bosses (ungewichtet)
        if (req.eliteCount > 0 && !_service.IsCompleted(nodeId))
        {
            int missing = Mathf.Max(0, req.eliteCount - _service.GetNodeProgress(nodeId).eliteCount);
            for (int i = 0; i < missing && !_service.IsCompleted(nodeId); i++)
                _service.OnEnemyKilled("test", EnemyRank.Elite, fallbackKillTags, fallbackKillTags);
        }
        if (req.bossCount > 0 && !_service.IsCompleted(nodeId))
        {
            int missing = Mathf.Max(0, req.bossCount - _service.GetNodeProgress(nodeId).bossCount);
            for (int i = 0; i < missing && !_service.IsCompleted(nodeId); i++)
                _service.OnEnemyKilled("test", EnemyRank.Boss, fallbackKillTags, fallbackKillTags);
        }

        // 5) Kills by Tag (gewichtet im Service)
        if (req.killsByTag != null && !_service.IsCompleted(nodeId))
        {
            foreach (var kc in req.killsByTag)
            {
                if (kc == null || string.IsNullOrEmpty(kc.enemyTag) || kc.count <= 0) continue;

                int cur = 0;
                var curDict = _service.GetNodeProgress(nodeId).killsByTagWeighted;
                if (curDict != null) curDict.TryGetValue(kc.enemyTag, out cur);

                int missing = Mathf.Max(0, kc.count - cur);
                for (int i = 0; i < missing && !_service.IsCompleted(nodeId); i++)
                {
                    // Tag setzen: Service wertet Gewicht nach Rank; Normal ist meist 1 – passt für Dev-FastForward
                    var tags = new List<string>(fallbackKillTags);
                    if (!tags.Contains(kc.enemyTag)) tags.Add(kc.enemyTag);
                    _service.OnEnemyKilled("test",EnemyRank.Normal,tags,fallbackKillTags);
                }
                if (_service.IsCompleted(nodeId)) break;
            }
        }

        // 6) Kills general (gewichtet)
        if (req.killsGeneral > 0 && !_service.IsCompleted(nodeId))
        {
            int cur = _service.GetNodeProgress(nodeId).killsGeneralWeighted;
            int missing = Mathf.Max(0, req.killsGeneral - cur);
            for (int i = 0; i < missing && !_service.IsCompleted(nodeId); i++)
                _service.OnEnemyKilled("test", EnemyRank.Normal, fallbackKillTags, fallbackKillTags);
        }

        // Service markiert den Knoten selbst als abgeschlossen, sobald Requirements erfüllt sind.
        // Falls Requirements 0 waren oder etwas knapp darunter blieb, ist der Knoten evtl. noch aktiv:
        // Das ist für Dev-Zwecke ok – bei nächster Operation wird er aktualisiert.

        return 1; // "eine Operation" gezählt (ein Knoten wurde bearbeitet)
    }
}
#endif
