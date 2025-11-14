using CHAL.Data;             // ResearchNodeDef, ResearchRequirement, ResearchUnlockTypes, ResearchTreeDef
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace CHAL.Systems.Research
{
    public sealed class ResearchService
    {
        // ------------------ Konfiguration / Tabellen ------------------
        private readonly Dictionary<string, ResearchNodeDef> _nodesById = new Dictionary<string, ResearchNodeDef>(StringComparer.Ordinal);
        private readonly Dictionary<(int lane, int stage), List<string>> _idsByLaneStage = new Dictionary<(int, int), List<string>>();

        private ResearchTreeDef _treeDef; // nur fÃ¼r Layout/Meta-Daten (optional fÃ¼r UI)
        private ResearchState _state;

        private Dictionary<string, List<string>> _compiledParents;

        // EnemyRank-Gewichtungen fÃ¼r Kill-"Punkte" (kannst du spÃ¤ter in ein Config-Asset verlagern)
        private readonly Dictionary<EnemyRank, int> _rankWeights = new Dictionary<EnemyRank, int>
        {
            { EnemyRank.Spawn,   0 },  // farm-sicher
            { EnemyRank.Normal,  1 },
            { EnemyRank.Magic,   1 },
            { EnemyRank.Elite,   2 },
            { EnemyRank.Boss,    5 },
            { EnemyRank.Champion,10},
        };

        // Elites/Bosses fÃ¼r "eliteCount"/"bossCount"-Requirements
        private static bool IsEliteLike(EnemyRank r) => r == EnemyRank.Elite || r == EnemyRank.Champion || r == EnemyRank.Boss;
        private static bool IsBoss(EnemyRank r) => r == EnemyRank.Boss || r == EnemyRank.Champion;

        private static bool IsChamp(EnemyRank r) => r == EnemyRank.Champion;

        // Event: Wenn ein Knoten abgeschlossen wurde (fÃ¼r UnlockRegistry)
        public event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted;
        // Event: wird nach InitFromTree mit den Always-Unlocked-IDs gefeuert
        public event Action<IReadOnlyList<string>> OnAlwaysUnlockedReady;

        // ------------------ Init ------------------

/// <summary>
/// Initializes the object using data from the specified research tree definition and state.
/// </summary>
/// <param name="treeDef">The research tree definition to initialize from.</param>
/// <param name="state">The research state to use; if null, a new state is created.</param>
        public void InitFromTree(ResearchTreeDef treeDef, ResearchState state)
        {
            _treeDef = treeDef;
            _state = state ?? new ResearchState();

            _nodesById.Clear();
            _idsByLaneStage.Clear();

            var compiled = ResearchTreeCompiler.Compile(treeDef);

            // nodesById + posById Ã¼bernehmen
            foreach (var kv in compiled.nodesById)
            {
                _nodesById[kv.Key] = kv.Value;
            }

            foreach (var kv in compiled.posById)
            {
                var key = (kv.Value.lane, kv.Value.stage);
                if (!_idsByLaneStage.TryGetValue(key, out var list))
                {
                    list = new List<string>();
                    _idsByLaneStage[key] = list;
                }
                list.Add(kv.Key);
            }

            // SlotIndex-Determinismus: pro (lane,stage) nach ID sortieren
            foreach (var kv in _idsByLaneStage)
                kv.Value.Sort(StringComparer.Ordinal);

            // State: Progress-EintrÃ¤ge sicherstellen
            foreach (var id in _nodesById.Keys)
                EnsureProgress(id);

            // Save: parents werden kÃ¼nftig NICHT aus NodeDef gelesen.
            _compiledParents = compiled.parentsById; // -> Feld hinzufÃ¼gen: Dictionary<string,List<string>> _compiledParents;

            DebugManager.Log($"ResearchService.InitFromTree: Nodes={_nodesById.Count}", DebugManager.EDebugLevel.Dev, "Research");

            var always = (_treeDef?.alwaysUnlockedIds ?? new List<string>())
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .Select(s => s.Trim())
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();

            if (always.Count > 0)
            {
                OnAlwaysUnlockedReady?.Invoke(always);
                DebugManager.Log($"ResearchService: AlwaysUnlocked ready ({always.Count} IDs).",
                    DebugManager.EDebugLevel.Dev, "Research");
            }
        }

        // Ã„nderung in IsNodeAvailable: NICHT mehr def.parents nutzen
/// <summary>
/// Checks if a node is available based on its ID.
/// </summary>
/// <param name="nodeId">The ID of the node to check.</param>
/// <returns>True if the node is available; otherwise, false.</returns>
        public bool IsNodeAvailable(string nodeId)
        {
            if (!_nodesById.TryGetValue(nodeId, out var def)) return false;
            if (IsCompleted(nodeId)) return false;

            if (_compiledParents != null && _compiledParents.TryGetValue(nodeId, out var parents))
            {
                foreach (var pid in parents)
                    if (!_state.completedNodeIds.Contains(pid)) return false;
            }
            return true;
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
/// <summary>
/// Retrieves the ID of the currently active node.
/// </summary>
/// <returns>The active node ID as a string.</returns>
        public string GetActiveNodeId() => _state.activeNodeId;

/// <summary>
/// Checks if the specified node is completed.
/// </summary>
/// <param name="nodeId">The ID of the node to check.</param>
/// <returns>True if the node is completed; otherwise, false.</returns>
        public bool IsCompleted(string nodeId) => _state.completedNodeIds.Contains(nodeId);

/// <summary>
/// Retrieves the progress of the specified node.
/// </summary>
/// <param name="nodeId">The ID of the node to get progress for.</param>
/// <returns>The progress of the node.</returns>
        public NodeProgress GetNodeProgress(string nodeId)
        {
            return _state.perNodeProgress.TryGetValue(nodeId, out var p) ? p : new NodeProgress();
        }

/// <summary>
/// Retrieves the ResearchNodeDef associated with the specified node ID.
/// </summary>
/// <param name="nodeID">The ID of the node to retrieve the definition for.</param>
/// <returns>The ResearchNodeDef for the given node ID, or null if not found.</returns>
        public ResearchNodeDef GetNodeDef(string nodeID)
        {
            return _nodesById.TryGetValue(nodeID, out var def) ? def: null;
        }

/// <summary>
/// Calculates the progress of a node based on its ID.
/// </summary>
/// <param name="nodeId">The ID of the node to check progress for.</param>
/// <returns>The progress of the node as a float value between 0 and 1.</returns>
        public float GetNodeProgress01(string nodeId)
        {
            if (IsCompleted(nodeId)) return 1f;
            var def = GetNodeDef(nodeId);
            if (def == null || def.requirements == null) return 0f;

            var r = def.requirements;
            var p = GetNodeProgress(nodeId);

            float have = 0f;
            float need = 0f;

            // Waves (falls vorhanden)
            if (r.waves > 0)
            {
                need += r.waves;
                have += Mathf.Clamp(p.waves, 0, r.waves);
            }

            // Maps gesamt (falls vorhanden)
            if (r.maps > 0)
            {
                need += r.maps;
                have += Mathf.Clamp(p.mapsTotal, 0, r.maps);
            }

            // Maps per Difficulty (falls vorhanden)
            if (r.mapRequirements != null)
            {
                foreach (var mr in r.mapRequirements)
                {
                    if (mr.amount <= 0) continue;
                    int cur = 0;
                    if (p.mapsByDifficulty != null && p.mapsByDifficulty.TryGetValue(mr.difficulty, out var c))
                        cur = c;
                    need += mr.amount;
                    have += Mathf.Clamp(cur, 0, mr.amount);
                }
            }

            // Kills gesamt gewichtet (falls vorhanden)
            if (r.killsGeneral > 0)
            {
                need += r.killsGeneral;
                have += Mathf.Clamp(p.killsGeneralWeighted, 0, r.killsGeneral);
            }

            // Kills nach Tag (falls vorhanden)
            if (r.killsByTag != null)
            {
                foreach (var kc in r.killsByTag)
                {
                    if (kc == null || string.IsNullOrEmpty(kc.enemyTag) || kc.count <= 0) continue;
                    int cur = 0;
                    if (p.killsByTagWeighted != null && p.killsByTagWeighted.TryGetValue(kc.enemyTag, out var v))
                        cur = v;
                    need += kc.count;
                    have += Mathf.Clamp(cur, 0, kc.count);
                }
            }

            // Elites / Bosse (falls vorhanden)
            if (r.eliteCount > 0) { need += r.eliteCount; have += Mathf.Clamp(p.eliteCount, 0, r.eliteCount); }
            if (r.bossCount > 0) { need += r.bossCount; have += Mathf.Clamp(p.bossCount, 0, r.bossCount); }

            if (need <= 0.0001f) return 0f;
            return Mathf.Clamp01(have / need);
        }

        // ------------------ Befehle ------------------
/// <summary>
/// Sets the active research node based on the provided node ID.
/// Returns true if the node is successfully set; otherwise, false.
/// </summary>
/// <param name="nodeId">The ID of the node to activate.</param>
/// <returns>True if the node is set as active; otherwise, false.</returns>
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

/// <summary>
/// Clears the currently active research node.
/// </summary>
        public void ClearActive()
        {
            _state.activeNodeId = null;
            DebugManager.Log("Active research cleared.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
        }

        // ------------------ Event-Inputs (Bridge ruft diese auf) ------------------
/// <summary>
/// Marks the completion of the wave process.
/// </summary>
        public void ApplyWaveCompleted(MapDifficulty difficulty)
        {
            var id = _state.activeNodeId;
            if (string.IsNullOrEmpty(id) || !_nodesById.TryGetValue(id, out var def)) return;

            var p = EnsureProgress(id);
            p.waves += 1;

            TryComplete(def, p);
        }

        /// <summary>
        /// Applies the completion of a map based on its difficulty.
        /// </summary>
        /// <param name="difficulty">The difficulty level of the map.</param>
        public void ApplyMapCompleted(MapDifficulty difficulty)
        {
            var id = _state.activeNodeId;
            if (string.IsNullOrEmpty(id) || !_nodesById.TryGetValue(id, out var def)) return;

            var p = EnsureProgress(id);
            p.mapsTotal += 1;

            var key = difficulty;
            if (!p.mapsByDifficulty.TryGetValue(key, out var cnt))
                p.mapsByDifficulty[key] = 1;
            else
                p.mapsByDifficulty[key] = cnt + 1;

            TryComplete(def, p);
        }

/// <summary>
/// Applies the effects of an enemy being killed based on its tags and rank.
/// </summary>
/// <param name="enemyTags">A list of tags associated with the enemy.</param>
/// <param name="rank">The rank of the enemy that was killed.</param>
        public void ApplyEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)
        {
            var id = _state.activeNodeId;
            if (string.IsNullOrEmpty(id) || !_nodesById.TryGetValue(id, out var def)) return;

            var p = EnsureProgress(id);

            // Rarity-ZÃ¤hler (ungewichtet)
            if (IsBoss(rank)) p.bossCount += 1;
            else if (IsEliteLike(rank)) p.eliteCount += 1;
            else if (IsChamp(rank)) p.champCount += 1;

                int weight = _rankWeights.TryGetValue(rank, out var w) ? w : 1;

            // Zielkills (byTag) priorisieren: nur wenn Requirement Tags fordert
            bool anyTagMatched = false;
            var req = def.requirements;
            if (req != null && req.killsByTag != null && req.killsByTag.Count > 0 && enemyTags != null)
            {
                // Schnittmenge der geforderten Tags und der Enemy-Tags
                // (Wenn mehrere geforderte Tags matchen, zÃ¤hlen alle â€“ das ist i. d. R. okay; bei Bedarf auf "nur erster Match" umstellen.)
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

            // Wenn kein Tag-Bucket getroffen wurde â†’ General hochzÃ¤hlen
            if (!anyTagMatched)
            {
                p.killsGeneralWeighted += weight;
            }

            TryComplete(def, p);
        }

        public void OnWaveCompleted(int mapId, int waveIndex, MapDifficulty difficulty)
        {
            // Aktuell ignorieren wir mapId/waveIndex/difficulty für Research.
            // Wenn du später per-Map oder per-Tier Research machen willst, kannst du das hier auswerten.
            ApplyWaveCompleted(difficulty);
        }

        public void OnMapCompleted(int mapId, MapDifficulty difficultyId)
        {
            ApplyMapCompleted(difficultyId);
        }

        public void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> basetags, List<string> bonustags)
        {

            var allTags = new List<string>();
            if (basetags != null) allTags.AddRange(basetags);
            if (bonustags != null) allTags.AddRange(bonustags);

            ApplyEnemyKilled(allTags, rank);
        }

        public void OnCraftExecuted(string recipeId)
        {
            /* TODO
             * - Wenn du später Research-Requirements für Crafting (z.B. "X mal gear crafted")
             *   ergänzen willst, kannst du hier NodeProgress erweitern.
             * - Aktuell: keine Wirkung, Event kommt aber sauber an.
             * END TODO */
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
            if (req == null) return true; // leere Anforderungen â†’ sofort fertig (V1 erlaubt)

            // Waves
            if (req.waves > 0 && p.waves < req.waves) return false;

            // Maps (gesamt)
            if (req.maps > 0 && p.mapsTotal < req.maps) return false;

            // Maps per Difficulty
            if (req.mapRequirements != null && req.mapRequirements.Count > 0)
            {
                foreach (var mr in req.mapRequirements)
                {
                    var key = mr.difficulty;
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
