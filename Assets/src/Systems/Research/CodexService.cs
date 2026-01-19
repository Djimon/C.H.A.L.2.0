using CHAL.Core;
using CHAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Codex
{
    public sealed class CodexService
    {
        private readonly Dictionary<string, CodexDeedDef> _nodesById = new Dictionary<string, CodexDeedDef>(StringComparer.Ordinal);
        private readonly Dictionary<(int lane, int stage), List<string>> _idsByLaneStage = new Dictionary<(int, int), List<string>>();

        private CodexDef _treeDef;
        private CodexState _state;

        private Dictionary<string, List<string>> _compiledParents;
        private CodexGateEngine _gate;

        private readonly Dictionary<EnemyRank, int> _rankWeights = new Dictionary<EnemyRank, int>
        {
            { EnemyRank.Spawn,    0 },
            { EnemyRank.Normal,   1 },
            { EnemyRank.Magic,    1 },
            { EnemyRank.Elite,    2 },
            { EnemyRank.Boss,     5 },
            { EnemyRank.Champion, 10 },
        };

        private static bool IsEliteLike(EnemyRank r) => r == EnemyRank.Elite || r == EnemyRank.Champion || r == EnemyRank.Boss;
        private static bool IsBoss(EnemyRank r) => r == EnemyRank.Boss || r == EnemyRank.Champion;
        private static bool IsChamp(EnemyRank r) => r == EnemyRank.Champion;


        //EVENTS
        public event Action<string, IReadOnlyList<CodexUnlock>> OnNodeCompleted;
        public event Action<IReadOnlyList<string>> OnAlwaysUnlockedReady;
        public event Action OnCodexChanged;

        public void InitFromDef(CodexDef treeDef, CodexState state)
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

            // Gate engine
            _gate = new CodexGateEngine(_treeDef, _state, new CodexGateEngine.Config(
                chainVisibilityClampEnabled: false,
                maxFutureDeedsVisible: 1
            ));

            // Focus Slots: mindestens 1 Slot
            if (_state.activeFocusSlots == null) _state.activeFocusSlots = new List<ActiveFocusSlotState>();
            if (_state.activeFocusSlots.Count == 0)
                _state.activeFocusSlots.Add(new ActiveFocusSlotState { deedId = null, locked = false });

            // Slot-Lock Status nachladen/rekonstruieren
            SyncAllSlotLocks();

            DebugManager.Log($"CodexService.InitFromTree: Nodes={_nodesById.Count}, FocusSlots={_state.activeFocusSlots.Count}",
                DebugManager.EDebugLevel.Dev, "Research");

            var always = (_treeDef?.alwaysUnlockedIds ?? new List<string>())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (always.Count > 0)
            {
                EnsureFocusSlotCount(1);

                OnAlwaysUnlockedReady?.Invoke(always);
                DebugManager.Log($"CodexService: AlwaysUnlocked ready ({always.Count} IDs).",
                    DebugManager.EDebugLevel.Dev, "Research");
            }

            RaiseCodexChanged();
        }

        public void EnsureFocusSlotCount(int requiredCount)
        {
            if (requiredCount < 1)
                requiredCount = 1;

            if (_state.activeFocusSlots == null)
                _state.activeFocusSlots = new List<ActiveFocusSlotState>();

            while (_state.activeFocusSlots.Count < requiredCount)
            {
                _state.activeFocusSlots.Add(new ActiveFocusSlotState
                {
                    deedId = null,
                    locked = false
                });
            }
        }



        // ------------------------------------
        // Active Focus API (neue Wahrheit)
        // ------------------------------------
        public int GetFocusSlotCount()
        {
            if (_state == null) return 0;
            if (_state.activeFocusSlots == null) return 0;
            return _state.activeFocusSlots.Count;
        }

        public string GetActiveDeedId(int slotIndex)
        {
            if (_state.activeFocusSlots == null) return null;
            if (slotIndex < 0 || slotIndex >= _state.activeFocusSlots.Count) return null;
            return _state.activeFocusSlots[slotIndex].deedId;
        }

        public bool TrySetActiveFocus(int slotIndex, string deedId, out string reason)
        {
            reason = null;

            if (_state.activeFocusSlots == null) _state.activeFocusSlots = new List<ActiveFocusSlotState>();
            if (slotIndex < 0 || slotIndex >= _state.activeFocusSlots.Count)
            {
                reason = "Invalid slot index.";
                return false;
            }

            // Slot lock: sobald claimable => erst claimen
            if (IsSlotLocked(slotIndex))
            {
                reason = "Slot is locked until claim.";
                return false;
            }

            // Clearing allowed (deedId null/empty) – aber nur wenn nicht locked (oben schon geprüft)
            if (string.IsNullOrWhiteSpace(deedId))
            {
                var s0 = _state.activeFocusSlots[slotIndex];
                s0.deedId = null;
                s0.locked = false;
                _state.activeFocusSlots[slotIndex] = s0;

                RaiseCodexChanged();
                return true;
            }

            deedId = deedId.Trim();

            // Existenz
            if (!_nodesById.ContainsKey(deedId))
            {
                reason = $"Unknown deedId '{deedId}'.";
                return false;
            }

            // Kein Deed darf in mehreren Slots sein
            if (TryFindSlotOfDeed(deedId, out var otherSlot) && otherSlot != slotIndex)
            {
                reason = $"Deed is already active in slot {otherSlot}.";
                return false;
            }

            // Claim ist echter Abschluss => claimed Deeds nicht mehr aktivierbar
            if (IsClaimed(deedId))
            {
                reason = "Deed already claimed.";
                return false;
            }

            // Gate: nur available aktivierbar (empfohlen, sonst parken)
            var gate = _gate != null ? _gate.ComputeDeedGate(deedId) : default;
            if (_gate != null && !gate.isAvailable)
            {
                reason = "Deed is not available yet.";
                return false;
            }

            // Set
            var slot = _state.activeFocusSlots[slotIndex];
            slot.deedId = deedId;
            slot.locked = IsClaimable(deedId); // direkt synchronisieren
            _state.activeFocusSlots[slotIndex] = slot;

            RaiseCodexChanged();

            DebugManager.Log($"Codex: Focus set slot={slotIndex} deed={deedId}", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
            return true;
        }

        public bool TryClaim(string deedId, out string reason)
        {
            reason = null;

            if (string.IsNullOrWhiteSpace(deedId))
            {
                reason = "Invalid deedId.";
                return false;
            }

            deedId = deedId.Trim();

            if (!_nodesById.TryGetValue(deedId, out var def) || def == null)
            {
                reason = "Unknown deedId.";
                return false;
            }

            EnsureProgress(deedId);

            var st = _state.deedProgress[deedId];

            if (st.claimed)
            {
                reason = "Already claimed.";
                return false;
            }

            // Claimable = completed && !claimed
            if (!st.completed && st.progress01 + 0.00001f < 1f)
            {
                reason = "Not completed yet.";
                return false;
            }

            // Stringent (dein Slot-Lock Konzept): Claim nur wenn Deed aktiv in einem Slot
            if (!TryFindSlotOfDeed(deedId, out var slotIndex))
            {
                reason = "Deed is not active in any focus slot.";
                return false;
            }

            // Claim durchführen
            st.completed = true;
            st.progress01 = 1f;
            st.claimed = true;
            _state.deedProgress[deedId] = st;

            // Slot lock updaten -> nach Claim ist es nicht mehr claimable => unlock
            SyncSlotLock(slotIndex);

            // Unlocks feuern (Claim = echter Abschluss)
            if (def.unlocks != null && def.unlocks.Count > 0)
                OnNodeCompleted?.Invoke(deedId, def.unlocks);

            RaiseCodexChanged();

            DebugManager.Log($"Codex: Claimed deed={deedId} (slot={slotIndex})", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
            return true;
        }

        public bool TryUnlockNextFocusSlot( out string reason)
        {
            reason = null;
            int maxSlots = GameManager.Instance.BalanceConfig.codexSettings.codexMaxFocusSlots;

            if (maxSlots < 1) maxSlots = 1;
            EnsureFocusSlotCount(1);

            int current = _state.activeFocusSlots.Count;
            if (current >= maxSlots)
            {
                reason = $"Already at max focus slots ({maxSlots}).";
                return false;
            }

            EnsureFocusSlotCount(current + 1);
            RaiseCodexChanged();
            return true;
        }

        // ------------------------------------
        // Query helpers
        // ------------------------------------

        public bool IsClaimed(string deedId)
        {
            if (string.IsNullOrWhiteSpace(deedId)) return false;
            if (!_state.deedProgress.TryGetValue(deedId, out var s)) return false;
            return s.claimed;
        }

        public bool IsClaimable(string deedId)
        {
            if (string.IsNullOrWhiteSpace(deedId)) return false;
            if (!_state.deedProgress.TryGetValue(deedId, out var s)) return false;
            return (s.completed || s.progress01 >= 1f) && !s.claimed;
        }

        public bool IsSlotLocked(int slotIndex)
        {
            if (_state.activeFocusSlots == null) return false;
            if (slotIndex < 0 || slotIndex >= _state.activeFocusSlots.Count) return false;

            // lock ist ableitbar: deed claimable
            var deedId = _state.activeFocusSlots[slotIndex].deedId;
            return !string.IsNullOrWhiteSpace(deedId) && IsClaimable(deedId);
        }

        private bool TryFindSlotOfDeed(string deedId, out int slotIndex)
        {
            slotIndex = -1;
            if (_state.activeFocusSlots == null) return false;
            for (int i = 0; i < _state.activeFocusSlots.Count; i++)
            {
                if (string.Equals(_state.activeFocusSlots[i].deedId, deedId, StringComparison.Ordinal))
                {
                    slotIndex = i;
                    return true;
                }
            }
            return false;
        }

        private void SyncAllSlotLocks()
        {
            if (_state.activeFocusSlots == null) return;
            for (int i = 0; i < _state.activeFocusSlots.Count; i++)
                SyncSlotLock(i);
        }

        private void SyncSlotLock(int slotIndex)
        {
            if (_state.activeFocusSlots == null) return;
            if (slotIndex < 0 || slotIndex >= _state.activeFocusSlots.Count) return;

            var slot = _state.activeFocusSlots[slotIndex];
            slot.locked = !string.IsNullOrWhiteSpace(slot.deedId) && IsClaimable(slot.deedId);
            _state.activeFocusSlots[slotIndex] = slot;
        }

        // ------------------------------------
        // Legacy helper (noch da, aber korrekt)
        // ------------------------------------

        public bool IsNodeAvailable(string nodeId)
        {
            // Legacy/Compiler-Pfade noch drin – aber Completion ist claimed.
            if (string.IsNullOrWhiteSpace(nodeId)) return false;
            if (!_nodesById.TryGetValue(nodeId, out _)) return false;
            if (IsClaimed(nodeId)) return false;

            if (_compiledParents != null && _compiledParents.TryGetValue(nodeId, out var parents))
            {
                foreach (var pid in parents)
                {
                    if (!IsClaimed(pid)) return false;
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
                if (s.counters == null)
                {
                    s.counters = new DeedProgress();
                    _state.deedProgress[deedId] = s;
                }
            }

            return _state.deedProgress[deedId].counters;
        }

        public DeedProgress GetNodeProgress(string deedId)
        {
            if (string.IsNullOrWhiteSpace(deedId)) return null;
            EnsureProgress(deedId);
            return _state.deedProgress[deedId].counters;
        }

        public CodexDeedDef GetNodeDef(string deedId)
            => _nodesById.TryGetValue(deedId, out var def) ? def : null;

        // ------------------------------------
        // Progress: nur über ActiveFocus Slots
        // ------------------------------------

        public void OnWaveCompleted(int waveIndex, int waveCount, MapDifficulty difficulty)
            => ApplyToActiveDeeds((deedId, def, prog) =>
            {
                prog.waves++;
                // optional: wave difficulty zählt bei maps, nicht bei waves
                return true;
            });

        public void OnMapCompleted(int mapId, MapDifficulty difficulty)
            => ApplyToActiveDeeds((deedId, def, prog) =>
            {
                prog.mapsTotal++;
                if (prog.mapsByDifficulty == null) prog.mapsByDifficulty = new Dictionary<MapDifficulty, int>();
                prog.mapsByDifficulty.TryGetValue(difficulty, out var cur);
                prog.mapsByDifficulty[difficulty] = cur + 1;
                return true;
            });

        public void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> tagsWeighted, List<string> tagsRaw)
            => ApplyToActiveDeeds((deedId, def, prog) =>
            {
                // killsGeneralWeighted: Gewichtung über Rank
                _rankWeights.TryGetValue(rank, out var w);
                if (w < 0) w = 0;
                prog.killsGeneralWeighted += w;

                // killsByTagWeighted: wir zählen hier tagsWeighted als "already weighted" (1 Eintrag = 1 Punkt)
                if (tagsWeighted != null)
                {
                    if (prog.killsByTagWeighted == null)
                        prog.killsByTagWeighted = new Dictionary<string, int>(StringComparer.Ordinal);

                    foreach (var t in tagsWeighted)
                    {
                        if (string.IsNullOrWhiteSpace(t)) continue;
                        prog.killsByTagWeighted.TryGetValue(t, out var cur);
                        prog.killsByTagWeighted[t] = cur + 1;
                    }
                }

                // Rarity counts
                if (IsEliteLike(rank)) prog.eliteCount++;
                if (IsBoss(rank)) prog.bossCount++;
                if (IsChamp(rank)) prog.champCount++;

                return true;
            });

        // ---- Optional: Craft Hook bleibt erstmal noop ----
        public void OnCraftExecuted(string obj)
        {
            // später, falls es DeedRequirements für crafting gibt
        }

        private delegate bool MutateProgressFn(string deedId, CodexDeedDef def, DeedProgress progress);

        private void ApplyToActiveDeeds(MutateProgressFn fn)
        {
            if (_state.activeFocusSlots == null || _state.activeFocusSlots.Count == 0) return;

            bool anyChanged = false;

            for (int slotIndex = 0; slotIndex < _state.activeFocusSlots.Count; slotIndex++)
            {
                var slot = _state.activeFocusSlots[slotIndex];
                var deedId = slot.deedId;

                if (string.IsNullOrWhiteSpace(deedId))
                    continue;

                // Slot lock = claimable => MUSS erst claimen, daher kein weiteres Progress
                if (IsSlotLocked(slotIndex))
                    continue;

                // Existence
                if (!_nodesById.TryGetValue(deedId, out var def) || def == null)
                    continue;

                EnsureProgress(deedId);

                // claimed => fertig
                if (IsClaimed(deedId))
                    continue;

                // Gate: Progress zählt nur wenn Deed available (und natürlich active)
                if (_gate != null)
                {
                    var gate = _gate.ComputeDeedGate(deedId);
                    if (!gate.isAvailable)
                        continue;
                }

                var progress = _state.deedProgress[deedId].counters;
                if (progress == null) continue;

                bool mutated = fn(deedId, def, progress);
                if (!mutated) continue;

                anyChanged = true;

                // Recompute progress01 / completed
                RecomputeAndStoreProgress(deedId);

                // Slot lock sync (kann jetzt claimable geworden sein)
                SyncSlotLock(slotIndex);
            }

            if (anyChanged)
                RaiseCodexChanged();
        }

        private void RecomputeAndStoreProgress(string deedId)
        {
            if (string.IsNullOrWhiteSpace(deedId)) return;
            if (!_nodesById.TryGetValue(deedId, out var def) || def == null) return;

            EnsureProgress(deedId);

            var st = _state.deedProgress[deedId];
            if (st.claimed)
            {
                st.progress01 = 1f;
                st.completed = true;
                _state.deedProgress[deedId] = st;
                return;
            }

            float p01 = ComputeProgress01(def, st.counters);
            st.progress01 = p01;
            st.completed = p01 >= 1f - 0.00001f;
            _state.deedProgress[deedId] = st;
        }

        private static float ComputeProgress01(CodexDeedDef def, DeedProgress p)
        {
            if (def == null || def.requirements == null || p == null) return 0f;

            var r = def.requirements;

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

            if (r.killsGeneral > 0)
            {
                need += r.killsGeneral;
                have += Mathf.Clamp(p.killsGeneralWeighted, 0, r.killsGeneral);
            }

            if (r.killsByTag != null)
            {
                foreach (var kc in r.killsByTag)
                {
                    if (kc == null || string.IsNullOrWhiteSpace(kc.enemyTag) || kc.count <= 0) continue;

                    int cur = 0;
                    if (p.killsByTagWeighted != null && p.killsByTagWeighted.TryGetValue(kc.enemyTag, out var v))
                        cur = v;

                    need += kc.count;
                    have += Mathf.Clamp(cur, 0, kc.count);
                }
            }

            if (r.eliteCount > 0) { need += r.eliteCount; have += Mathf.Clamp(p.eliteCount, 0, r.eliteCount); }
            if (r.bossCount > 0) { need += r.bossCount; have += Mathf.Clamp(p.bossCount, 0, r.bossCount); }
            if (r.championCount > 0) { need += r.championCount; have += Mathf.Clamp(p.champCount, 0, r.championCount); }

            if (need <= 0.0001f) return 0f;
            return Mathf.Clamp01(have / need);
        }

        // ============================================================
        //  Helper for UI ViewModelss (API-first)
        // ============================================================

        public IReadOnlyList<ChapterVM> GetChaptersVM()
        {
            var list = new List<ChapterVM>();
            if (_treeDef == null || _treeDef.codexChapters == null) return list;

            foreach (var ch in _treeDef.codexChapters)
            {
                if (ch == null) continue;
                list.Add(BuildChapterVM(ch));
            }

            return list;
        }

        public ChapterVM GetChapterVM(string chapterId)
        {
            if (string.IsNullOrWhiteSpace(chapterId)) return null;
            if (_treeDef == null || _treeDef.codexChapters == null) return null;

            foreach (var ch in _treeDef.codexChapters)
            {
                if (ch == null) continue;
                if (string.Equals(GetChapterId(ch), chapterId, StringComparison.Ordinal))
                    return BuildChapterVM(ch);
            }

            return null;
        }

        private ChapterVM BuildChapterVM(CodexChapter ch)
        {
            var vm = new ChapterVM
            {
                chapterId = GetChapterId(ch)
            };

            if (ch.stages == null) return vm;

            for (int groupIndex = 0; groupIndex < ch.stages.Count; groupIndex++)
            {
                var g = ch.stages[groupIndex];
                if (g == null) continue;

                var groupId = GetGroupId(g, vm.chapterId, groupIndex);

                // Group gate via engine (wenn engine groupId kennt)
                GroupGateState groupGate;
                if (_gate != null)
                    groupGate = _gate.ComputeGroupGate(groupId);
                else
                    groupGate = default;

                var gvm = new GroupVM
                {
                    groupId = groupId,
                    gate = groupGate,
                };

                if (g.deedSlots != null)
                {
                    for (int slotIdx = 0; slotIdx < g.deedSlots.Count; slotIdx++)
                    {
                        var slot = g.deedSlots[slotIdx];
                        if (slot == null) continue;

                        var deedId = GetDeedId(slot);
                        if (string.IsNullOrWhiteSpace(deedId)) continue;

                        EnsureProgressSafe(deedId);

                        var st = _state.deedProgress.TryGetValue(deedId, out var s) ? s : default;

                        var deedGate = _gate != null ? _gate.ComputeDeedGate(deedId) : default;

                        var (isActive, activeSlotIndex) = FindActiveSlot(deedId);

                        // Slot-Lock ist bei dir: claimable => locked
                        bool claimable = (st.completed || st.progress01 >= 1f - 0.00001f) && !st.claimed;
                        bool isSlotLocked = isActive && claimable;

                        var def = GetNodeDef(deedId);

                        gvm.deeds.Add(new DeedVM
                        {
                            deedId = deedId,
                            title = def != null ? def.title : deedId,

                            gate = deedGate,

                            progress01 = st.progress01,
                            completed = st.completed,
                            claimed = st.claimed,

                            isActive = isActive,
                            activeSlotIndex = activeSlotIndex,
                            isSlotLocked = isSlotLocked,
                        });
                    }
                }

                vm.groups.Add(gvm);
            }

            return vm;
        }

        private (bool isActive, int slotIndex) FindActiveSlot(string deedId)
        {
            if (string.IsNullOrWhiteSpace(deedId)) return (false, -1);
            if (_state == null || _state.activeFocusSlots == null) return (false, -1);

            for (int i = 0; i < _state.activeFocusSlots.Count; i++)
            {
                if (string.Equals(_state.activeFocusSlots[i].deedId, deedId, StringComparison.Ordinal))
                    return (true, i);
            }

            return (false, -1);
        }

        private void EnsureProgressSafe(string deedId)
        {
            // Nur defensiv: falls deine EnsureProgress Methode anders heißt oder private ist,
            // kannst du hier einfach deinen bestehenden EnsureProgress-Aufruf einsetzen.
            if (_state == null) return;
            if (_state.deedProgress == null) return;

            if (!_state.deedProgress.TryGetValue(deedId, out var s))
            {
                // minimaler Default – dein echtes EnsureProgress macht ggf. mehr
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
                if (s.counters == null)
                {
                    s.counters = new DeedProgress();
                    _state.deedProgress[deedId] = s;
                }
            }
        }

        private static string GetChapterId(CodexChapter ch)
        {
            // Du nutzt chapterName als Id – passt zu deinem aktuellen Stand.
            return ch != null ? ch.chapterId : null;
        }

        private static string GetGroupId(CodexChapterGroup g, string chapterId, int groupOrderIndex)
        {
            // Dein aktueller Fix: g.groupid ist stable ID.
            // Wir machen das robust: wenn field/property nicht existiert, fallback.
            if (g == null) return $"{chapterId}:GroupIdx:{groupOrderIndex}";

            // field: groupid
            var t = g.GetType();
            var f = t.GetField("groupid");
            if (f != null)
            {
                var v = f.GetValue(g) as string;
                if (!string.IsNullOrWhiteSpace(v))
                    return v;
            }

            // property: groupid
            var p = t.GetProperty("groupid");
            if (p != null)
            {
                var v = p.GetValue(g) as string;
                if (!string.IsNullOrWhiteSpace(v))
                    return v;
            }

            return $"{chapterId}:GroupIdx:{groupOrderIndex}";
        }

        private static string GetDeedId(DeedSlot slot)
        {
            if (slot == null) return null;

            // optional: deedId Feld
            var t = slot.GetType();
            var f = t.GetField("deedId");
            if (f != null)
            {
                var v = f.GetValue(slot) as string;
                if (!string.IsNullOrWhiteSpace(v))
                    return v;
            }

            // fallback: ScriptableObject ref
            if (slot.deed != null && !string.IsNullOrWhiteSpace(slot.deed.id))
                return slot.deed.id;

            return null;
        }

        private void RaiseCodexChanged()
        {
            OnCodexChanged?.Invoke();
        }

    }
}
