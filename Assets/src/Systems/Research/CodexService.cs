using CHAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Research
{
    public sealed class CodexService
    {
        private readonly Dictionary<string, CodexDeedDef> _nodesById = new Dictionary<string, CodexDeedDef>(StringComparer.Ordinal);
        private readonly Dictionary<(int lane, int stage), List<string>> _idsByLaneStage = new Dictionary<(int, int), List<string>>();

        private CodexDef _treeDef;
        private CodexState _state;

        private Dictionary<string, List<string>> _compiledParents;

        private readonly Dictionary<EnemyRank, int> _rankWeights = new Dictionary<EnemyRank, int>
        {
            { EnemyRank.Spawn,   0 },
            { EnemyRank.Normal,  1 },
            { EnemyRank.Magic,   1 },
            { EnemyRank.Elite,   2 },
            { EnemyRank.Boss,    5 },
            { EnemyRank.Champion,10},
        };

        private static bool IsEliteLike(EnemyRank r) => r == EnemyRank.Elite || r == EnemyRank.Champion || r == EnemyRank.Boss;
        private static bool IsBoss(EnemyRank r) => r == EnemyRank.Boss || r == EnemyRank.Champion;
        private static bool IsChamp(EnemyRank r) => r == EnemyRank.Champion;

        public event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted;
        public event Action<IReadOnlyList<string>> OnAlwaysUnlockedReady;

        public void InitFromTree(CodexDef treeDef, CodexState state)
        {
            _treeDef = treeDef;
            _state = state ?? new CodexState();

            _nodesById.Clear();
            _idsByLaneStage.Clear();

            var compiled = CodexCompiler.Compile(treeDef);

            foreach (var kv in compiled.nodesById)
                _nodesById[kv.Key] = kv.Value;

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

            foreach (var kv in _idsByLaneStage)
                kv.Value.Sort(StringComparer.Ordinal);

            foreach (var id in _nodesById.Keys)
                EnsureProgress(id);

            _compiledParents = compiled.parentsById;

            // Sicherstellen: mind. 1 Focus Slot existiert (Adapter für altes "activeNodeId")
            if (_state.activeFocusSlots == null) _state.activeFocusSlots = new List<ActiveFocusSlotState>();
            if (_state.activeFocusSlots.Count == 0)
                _state.activeFocusSlots.Add(new ActiveFocusSlotState { deedId = null, locked = false });

            DebugManager.Log($"CodexService.InitFromTree: Nodes={_nodesById.Count}", DebugManager.EDebugLevel.Dev, "Research");

            var always = (_treeDef?.alwaysUnlockedIds ?? new List<string>())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (always.Count > 0)
            {
                OnAlwaysUnlockedReady?.Invoke(always);
                DebugManager.Log($"CodexService: AlwaysUnlocked ready ({always.Count} IDs).",
                    DebugManager.EDebugLevel.Dev, "Research");
            }
        }

        public bool IsNodeAvailable(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) return false;
            if (!_nodesById.TryGetValue(nodeId, out _)) return false;
            if (IsCompleted(nodeId)) return false;

            if (_compiledParents != null && _compiledParents.TryGetValue(nodeId, out var parents))
            {
                foreach (var pid in parents)
                {
                    if (!IsCompleted(pid)) return false;
                }
            }
            return true;
        }

        private DeedProgress EnsureProgress(string deedId)
        {
            if (!_state.deedProgress.TryGetValue(deedId, out var s))
            {
                s = new DeedProgressState
                {
                    progress01 = 0f,
                    completed = false,
                    claimed = false,
                    counters = new DeedProgress()
                };
                _state.deedProgress[deedId] = s;
            }
            else
            {
                // defensive: counters darf nie null sein
                if (s.counters == null)
                {
                    s.counters = new DeedProgress();
                    _state.deedProgress[deedId] = s;
                }
            }

            return _state.deedProgress[deedId].counters;
        }

        // Adapter: "active node" ist Slot 0
        public string GetActiveNodeId()
        {
            if (_state.activeFocusSlots == null || _state.activeFocusSlots.Count == 0) return null;
            return _state.activeFocusSlots[0].deedId;
        }

        public bool IsCompleted(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) return false;
            if (!_state.deedProgress.TryGetValue(nodeId, out var s)) return false;
            return s.claimed; // Claim ist der echte Abschluss
        }

        public DeedProgress GetNodeProgress(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) return null;
            EnsureProgress(nodeId);
            return _state.deedProgress[nodeId].counters;
        }

        public CodexDeedDef GetNodeDef(string nodeID)
        {
            return _nodesById.TryGetValue(nodeID, out var def) ? def : null;
        }

        public float GetNodeProgress01(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) return 0f;
            if (IsCompleted(nodeId)) return 1f;

            var def = GetNodeDef(nodeId);
            if (def == null || def.requirements == null) return 0f;

            var r = def.requirements;
            var p = GetNodeProgress(nodeId);

            float have = 0f;
            float need = 0f;

            if (r.waves > 0) { need += r.waves; have += Mathf.Clamp(p.waves, 0, r.waves); }
            if (r.maps > 0) { need += r.maps; have += Mathf.Clamp(p.mapsTotal, 0, r.maps); }

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

            if (r.killsGeneral > 0) { need += r.killsGeneral; have += Mathf.Clamp(p.killsGeneralWeighted, 0, r.killsGeneral); }

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

            if (r.eliteCount > 0) { need += r.eliteCount; have += Mathf.Clamp(p.eliteCount, 0, r.eliteCount); }
            if (r.bossCount > 0) { need += r.bossCount; have += Mathf.Clamp(p.bossCount, 0, r.bossCount); }

            if (need <= 0.0001f) return 0f;
            return Mathf.Clamp01(have / need);
        }

        public bool SetActive(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) return false;
            if (!_nodesById.ContainsKey(nodeId)) return false;
            if (IsCompleted(nodeId)) return false;
            if (!IsNodeAvailable(nodeId)) return false;

            if (_state.activeFocusSlots == null) _state.activeFocusSlots = new List<ActiveFocusSlotState>();
            if (_state.activeFocusSlots.Count == 0)
                _state.activeFocusSlots.Add(new ActiveFocusSlotState { deedId = null, locked = false });

            var slot = _state.activeFocusSlots[0];
            slot.deedId = nodeId;
            slot.locked = false;
            _state.activeFocusSlots[0] = slot;

            DebugManager.Log($"Active focus set (slot0): {nodeId}", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
            return true;
        }

        internal void OnEnemyKilled(string arg1, EnemyRank rank, List<string> list1, List<string> list2)
        {
            throw new NotImplementedException();
        }

        internal void OnWaveCompleted(int arg1, int arg2, MapDifficulty difficulty)
        {
            throw new NotImplementedException();
        }

        internal void OnMapCompleted(int arg1, MapDifficulty difficulty)
        {
            throw new NotImplementedException();
        }

        internal void OnCraftExecuted(string obj)
        {
            throw new NotImplementedException();
        }

        // ---- Rest der Datei bleibt erstmal unangetastet / wird in Phase 3 ersetzt ----
    }
}
