using System;
using System.Collections.Generic;
using System.Linq;
using CHAL.Data;             // ResearchNodeDef, ResearchRequirement, ResearchUnlockTypes, ResearchTreeDef


namespace CHAL.Systems.Research
{
    public sealed class ResearchService
    {
        // ------------------ Konfiguration / Tabellen ------------------
        private readonly Dictionary<string, ResearchNodeDef> _nodesById = new Dictionary<string, ResearchNodeDef>(StringComparer.Ordinal);
        private readonly Dictionary<(int lane, int stage), List<string>> _idsByLaneStage = new Dictionary<(int, int), List<string>>();

        private ResearchTreeDef _treeDef; // nur für Layout/Meta-Daten (optional für UI)
        private ResearchState _state;

        // EnemyRank-Gewichtungen für Kill-"Punkte" (kannst du später in ein Config-Asset verlagern)
        private readonly Dictionary<EnemyRank, int> _rankWeights = new Dictionary<EnemyRank, int>
        {
            { EnemyRank.Spawn,   0 },  // farm-sicher
            { EnemyRank.Normal,  1 },
            { EnemyRank.Magic,   1 },
            { EnemyRank.Elite,   2 },
            { EnemyRank.Boss,    5 },
            { EnemyRank.Champion,10},
        };

        // Elites/Bosses für "eliteCount"/"bossCount"-Requirements
        private static bool IsEliteLike(EnemyRank r) => r == EnemyRank.Elite || r == EnemyRank.Champion;
        private static bool IsBoss(EnemyRank r) => r == EnemyRank.Boss;

        // Event: Wenn ein Knoten abgeschlossen wurde (für UnlockRegistry)
        public event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted;

        // ------------------ Init ------------------
        public void Init(IEnumerable<ResearchNodeDef> nodeDefs, ResearchTreeDef treeDef, ResearchState state)
        {
            _treeDef = treeDef;
            _state = state ?? new ResearchState();

            _nodesById.Clear();
            _idsByLaneStage.Clear();

            foreach (var def in nodeDefs)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.id)) continue;
                _nodesById[def.id] = def;

                var key = (def.lane, def.stage);
                if (!_idsByLaneStage.TryGetValue(key, out var list))
                {
                    list = new List<string>();
                    _idsByLaneStage[key] = list;
                }
                list.Add(def.id);
            }

            // SlotIndex-Determinismus: pro (lane,stage) nach ID sortieren (stabil)
            foreach (var kv in _idsByLaneStage)
                kv.Value.Sort(StringComparer.Ordinal);

            // State-Komplettierung: Progress-Einträge sicherstellen
            foreach (var id in _nodesById.Keys)
                EnsureProgress(id);

            DebugManager.Log($"ResearchService.Init: Nodes={_nodesById.Count}", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
        }

        private NodeProgress EnsureProgress(string nodeId)
        {
            if (!_state.perNodeProgress.TryGetValue(nodeId, out var p))
            {
                p = new NodeProgress();
                _state.perNodeProgress[nodeId] = p;
            }
            return p;
        }

        // ------------------ Query API ------------------
        public string GetActiveNodeId() => _state.activeNodeId;

        public bool IsCompleted(string nodeId) => _state.completedNodeIds.Contains(nodeId);

        public bool IsNodeAvailable(string nodeId)
        {
            if (!_nodesById.TryGetValue(nodeId, out var def)) return false;
            if (IsCompleted(nodeId)) return false;

            // Parents erfüllt?
            if (def.parents != null)
            {
                foreach (var pid in def.parents)
                {
                    if (!_state.completedNodeIds.Contains(pid)) return false;
                }
            }
            return true;
        }

        // Für UI-Layout: bestimme SlotIndex eines (lane,stage,nodeId)
        public int GetSlotIndex(string nodeId)
        {
            if (!_nodesById.TryGetValue(nodeId, out var def)) return 0;
            var key = (def.lane, def.stage);
            if (_idsByLaneStage.TryGetValue(key, out var list))
            {
                var idx = list.IndexOf(nodeId);
                return idx >= 0 ? idx : 0;
            }
            return 0;
        }

        public NodeProgress GetNodeProgress(string nodeId)
        {
            return _state.perNodeProgress.TryGetValue(nodeId, out var p) ? p : new NodeProgress();
        }

        // ------------------ Befehle ------------------
        public bool SetActive(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) return false;
            if (!_nodesById.ContainsKey(nodeId)) return false;
            if (IsCompleted(nodeId)) return false;
            if (!IsNodeAvailable(nodeId)) return false;

            _state.activeNodeId = nodeId;
            DebugManager.Log($"Active research set: {nodeId}", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
            return true;
        }

        public void ClearActive()
        {
            _state.activeNodeId = null;
            DebugManager.Log("Active research cleared.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
        }

        // ------------------ Event-Inputs (Bridge ruft diese auf) ------------------
        public void ApplyWaveCompleted()
        {
            var id = _state.activeNodeId;
            if (string.IsNullOrEmpty(id) || !_nodesById.TryGetValue(id, out var def)) return;

            var p = EnsureProgress(id);
            p.waves += 1;

            TryComplete(def, p);
        }

        public void ApplyMapCompleted(MapDifficulty difficulty)
        {
            var id = _state.activeNodeId;
            if (string.IsNullOrEmpty(id) || !_nodesById.TryGetValue(id, out var def)) return;

            var p = EnsureProgress(id);
            p.mapsTotal += 1;

            var key = (int)difficulty;
            if (!p.mapsByDifficulty.TryGetValue(key, out var cnt))
                p.mapsByDifficulty[key] = 1;
            else
                p.mapsByDifficulty[key] = cnt + 1;

            TryComplete(def, p);
        }

        public void ApplyEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)
        {
            var id = _state.activeNodeId;
            if (string.IsNullOrEmpty(id) || !_nodesById.TryGetValue(id, out var def)) return;

            var p = EnsureProgress(id);

            // Rarity-Zähler (ungewichtet)
            if (IsBoss(rank)) p.bossCount += 1;
            else if (IsEliteLike(rank)) p.eliteCount += 1;

            int weight = _rankWeights.TryGetValue(rank, out var w) ? w : 1;

            // Zielkills (byTag) priorisieren: nur wenn Requirement Tags fordert
            bool anyTagMatched = false;
            var req = def.requirements;
            if (req != null && req.killsByTag != null && req.killsByTag.Count > 0 && enemyTags != null)
            {
                // Schnittmenge der geforderten Tags und der Enemy-Tags
                // (Wenn mehrere geforderte Tags matchen, zählen alle – das ist i. d. R. okay; bei Bedarf auf "nur erster Match" umstellen.)
                foreach (var needed in req.killsByTag)
                {
                    if (needed == null || string.IsNullOrEmpty(needed.enemyTag)) continue;
                    if (enemyTags.Contains(needed.enemyTag))
                    {
                        anyTagMatched = true;
                        if (!p.killsByTagWeighted.TryGetValue(needed.enemyTag, out var cur))
                            p.killsByTagWeighted[needed.enemyTag] = weight;
                        else
                            p.killsByTagWeighted[needed.enemyTag] = cur + weight;
                    }
                }
            }

            // Wenn kein Tag-Bucket getroffen wurde → General hochzählen
            if (!anyTagMatched)
            {
                p.killsGeneralWeighted += weight;
            }

            TryComplete(def, p);
        }


        // ------------------ Completion-Check ------------------
        private void TryComplete(ResearchNodeDef def, NodeProgress p)
        {
            if (IsCompleted(def.id)) return;

            if (!MeetsRequirements(def, p)) return;

            _state.completedNodeIds.Add(def.id);

            // Optional: aktiven Slot NICHT automatisch leeren (Quality-of-Life)
            DebugManager.Log($"Research completed: {def.id}", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);

            // Unlock-Effekte melden
            OnNodeCompleted?.Invoke(def.id, def.unlocks);
        }

        private static bool MeetsRequirements(ResearchNodeDef def, NodeProgress p)
        {
            var req = def.requirements;
            if (req == null) return true; // leere Anforderungen → sofort fertig (V1 erlaubt)

            // Waves
            if (req.waves > 0 && p.waves < req.waves) return false;

            // Maps (gesamt)
            if (req.maps > 0 && p.mapsTotal < req.maps) return false;

            // Maps per Difficulty
            if (req.mapRequirements != null && req.mapRequirements.Count > 0)
            {
                foreach (var mr in req.mapRequirements)
                {
                    var key = (int)mr.difficulty;
                    int cur = p.mapsByDifficulty.TryGetValue(key, out var c) ? c : 0;
                    if (cur < mr.amount) return false;
                }
            }

            // Kills general (weighted)
            if (req.killsGeneral > 0 && p.killsGeneralWeighted < req.killsGeneral) return false;

            // Kills by tag (weighted)
            if (req.killsByTag != null && req.killsByTag.Count > 0)
            {
                foreach (var kc in req.killsByTag)
                {
                    if (kc == null || string.IsNullOrEmpty(kc.enemyTag)) continue;
                    int cur = p.killsByTagWeighted.TryGetValue(kc.enemyTag, out var c) ? c : 0;
                    if (cur < kc.count) return false;
                }
            }

            // Elites/Bosses (ungewichtet)
            if (req.eliteCount > 0 && p.eliteCount < req.eliteCount) return false;
            if (req.bossCount > 0 && p.bossCount < req.bossCount) return false;

            return true;
        }
    }
}
