using CHAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Research
{
    /// <summary>
    /// Deterministische Gate Engine (Phase 2).
    /// - Keine UI-Logik.
    /// - Visible vs Available getrennt.
    /// - Completion-Metrik: CLAIMED (nicht completed).
    /// </summary>
    public sealed class CodexGateEngine
    {
        public readonly struct Config
        {
            /// <summary>
            /// Optional: innerhalb einer Gruppe nur die nächsten N "future" Deeds sichtbar machen.
            /// Default: false (dein Plan).
            /// </summary>
            public readonly bool chainVisibilityClampEnabled;

            /// <summary>
            /// Wenn Clamp aktiv: wie viele "future" locked Deeds max sichtbar sind.
            /// </summary>
            public readonly int maxFutureDeedsVisible;

            public Config(bool chainVisibilityClampEnabled = false, int maxFutureDeedsVisible = 1)
            {
                this.chainVisibilityClampEnabled = chainVisibilityClampEnabled;
                this.maxFutureDeedsVisible = Mathf.Max(0, maxFutureDeedsVisible);
            }
        }

        private readonly CodexDef _def;
        private readonly CodexState _state;
        private readonly Config _cfg;

        // Indizes für deterministische Lookups (Def-Order ist die Wahrheit, UI darf sortieren).
        private readonly Dictionary<string, ChapterIndex> _chapterById = new(StringComparer.Ordinal);
        private readonly Dictionary<string, GroupIndex> _groupById = new(StringComparer.Ordinal);
        private readonly Dictionary<string, DeedIndex> _deedById = new(StringComparer.Ordinal);

        public CodexGateEngine(CodexDef def, CodexState state, Config cfg = default)
        {
            _def = def ? def : throw new ArgumentNullException(nameof(def));
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _cfg = cfg;

            BuildIndices();
        }

        // -------------------------
        // Public API (Phase 2)
        // -------------------------

        public GroupGateState ComputeGroupGate(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId) || !_groupById.TryGetValue(groupId, out var g))
            {
                return new GroupGateState
                {
                    isVisible = false,
                    completion01 = 0f,
                    requiredCompletion01 = 1f,
                    dependsOnGroupId = null,
                };
            }

            var dependsOn = ResolveDependsOnGroupId(g);
            var required = GetGroupVisibleAfterCompletion01(g);

            // Group completion: CLAIMED / TOTAL
            var completion = ComputeGroupCompletion01(g.groupId);

            bool isVisible;
            if (string.IsNullOrEmpty(dependsOn))
            {
                // Group 0 / or no dependency => visible
                isVisible = true;
            }
            else
            {
                var depCompletion = ComputeGroupCompletion01(dependsOn);
                var depRequired = GetGroupVisibleAfterCompletion01(dependsOn);

                // "visibleAfterCompletion01" gehört semantisch zur Gruppe, die sichtbar werden soll,
                // aber du hast es als property der target-group definiert. Wir folgen dem.
                // => Sichtbarkeit von g hängt ab von completion(dependsOn) >= required(target).
                isVisible = depCompletion >= required - 0.00001f;
            }

            return new GroupGateState
            {
                isVisible = isVisible,
                completion01 = completion,
                requiredCompletion01 = required,
                dependsOnGroupId = dependsOn,
            };
        }

        public DeedGateState ComputeDeedGate(string deedId)
        {
            // Not found => unsichtbar
            if (string.IsNullOrWhiteSpace(deedId) || !_deedById.TryGetValue(deedId, out var d))
            {
                return new DeedGateState
                {
                    isVisible = false,
                    isAvailable = false,
                    blockedByDeedId = null,
                    blockedByRequProgress01 = 1f,
                    blockedByGroupId = null,
                    blockedByRequGroupProgress01 = 1f,
                };
            }

            // 1) Group Gate
            var groupGate = ComputeGroupGate(d.groupId);
            if (!groupGate.isVisible)
            {
                // Deed unsichtbar, weil Group unsichtbar.
                return new DeedGateState
                {
                    isVisible = false,
                    isAvailable = false,
                    blockedByDeedId = null,
                    blockedByRequProgress01 = 1f,
                    blockedByGroupId = groupGate.dependsOnGroupId,
                    blockedByRequGroupProgress01 = groupGate.requiredCompletion01,
                };
            }

            // 2) Base visibility: group visible
            bool isVisible = true;

            // Optional: Chain Clamp innerhalb der Gruppe
            if (_cfg.chainVisibilityClampEnabled)
            {
                isVisible = IsVisibleUnderChainClamp(d);
                if (!isVisible)
                {
                    // Clamp ist eine Policy: absichtlich keine Block-Details (UI zeigt’s einfach nicht).
                    return new DeedGateState
                    {
                        isVisible = false,
                        isAvailable = false,
                        blockedByDeedId = null,
                        blockedByRequProgress01 = 1f,
                        blockedByGroupId = null,
                        blockedByRequGroupProgress01 = 1f,
                    };
                }
            }

            // 3) Availability (Gate a): unlockAfterDeedId >= unlockAfterProgress01
            var unlockAfterDeedId = GetUnlockAfterDeedId(d);
            var unlockAfterProgress01 = GetUnlockAfterProgress01(d);

            bool isAvailable = true;
            string blockedByDeedId = null;
            float blockedByRequiredProgress01 = 1f;

            if (!string.IsNullOrWhiteSpace(unlockAfterDeedId))
            {
                float prereqProgress = GetProgress01(unlockAfterDeedId);
                if (prereqProgress + 0.00001f < unlockAfterProgress01)
                {
                    isAvailable = false;
                    blockedByDeedId = unlockAfterDeedId;
                    blockedByRequiredProgress01 = unlockAfterProgress01;
                }
            }

            return new DeedGateState
            {
                isVisible = true,
                isAvailable = isAvailable,
                blockedByDeedId = blockedByDeedId,
                blockedByRequProgress01 = blockedByRequiredProgress01,
                blockedByGroupId = null,
                blockedByRequGroupProgress01 = 1f,
            };
        }

        // -------------------------
        // Internals
        // -------------------------

        private void BuildIndices()
        {
            _chapterById.Clear();
            _groupById.Clear();
            _deedById.Clear();

            if (_def.codexChapters == null) return;

            foreach (var ch in _def.codexChapters)
            {
                if (ch == null) continue;

                var chapterId = GetChapterId(ch);
                if (string.IsNullOrWhiteSpace(chapterId))
                    continue;

                if (!_chapterById.ContainsKey(chapterId))
                    _chapterById.Add(chapterId, new ChapterIndex(chapterId, ch));

                if (ch.stages == null) continue;

                for (int gi = 0; gi < ch.stages.Count; gi++)
                {
                    var g = ch.stages[gi];
                    if (g == null) continue;

                    var groupId = GetGroupId(g, chapterId, gi);
                    if (string.IsNullOrWhiteSpace(groupId))
                        continue;

                    if (!_groupById.ContainsKey(groupId))
                        _groupById.Add(groupId, new GroupIndex(groupId, chapterId, gi, g));

                    if (g.deedSlots == null) continue;

                    for (int si = 0; si < g.deedSlots.Count; si++)
                    {
                        var slot = g.deedSlots[si];
                        if (slot == null) continue;

                        var deedId = GetDeedId(slot);
                        if (string.IsNullOrWhiteSpace(deedId))
                            continue;

                        // DeedId muss global eindeutig sein (dein System geht davon aus).
                        if (!_deedById.ContainsKey(deedId))
                            _deedById.Add(deedId, new DeedIndex(deedId, chapterId, groupId, gi, si, slot));
                    }
                }
            }
        }

        private float ComputeGroupCompletion01(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId) || !_groupById.TryGetValue(groupId, out var g))
                return 0f;

            int total = 0;
            int claimed = 0;

            if (g.group.deedSlots != null)
            {
                foreach (var slot in g.group.deedSlots)
                {
                    var deedId = GetDeedId(slot);
                    if (string.IsNullOrWhiteSpace(deedId)) continue;

                    total++;
                    if (IsClaimed(deedId)) claimed++;
                }
            }

            if (total <= 0) return 1f;
            return Mathf.Clamp01((float)claimed / total);
        }

        private bool IsClaimed(string deedId)
        {
            if (string.IsNullOrWhiteSpace(deedId)) return false;
            if (!_state.deedProgress.TryGetValue(deedId, out var s)) return false;
            return s.claimed;
        }

        private float GetProgress01(string deedId)
        {
            if (string.IsNullOrWhiteSpace(deedId)) return 0f;
            if (!_state.deedProgress.TryGetValue(deedId, out var s)) return 0f;
            return Mathf.Clamp01(s.progress01);
        }

        private string ResolveDependsOnGroupId(GroupIndex g)
        {
            // 1) Wenn Def-Feld existiert (dependsOnGroupId), nimm das.
            // 2) Sonst default: previous group in DEF-order.
            var explicitId = GetDependsOnGroupId(g.group);
            if (!string.IsNullOrWhiteSpace(explicitId))
                return explicitId;

            // Default = previous group (nur wenn gi > 0)
            if (g.groupOrderIndex <= 0) return null;

            // Previous group Id ermitteln über chapter+order
            var prev = FindGroupIdByChapterAndOrder(g.chapterId, g.groupOrderIndex - 1);
            return prev;
        }

        private string FindGroupIdByChapterAndOrder(string chapterId, int groupOrderIndex)
        {
            foreach (var kv in _groupById)
            {
                var g = kv.Value;
                if (g.chapterId == chapterId && g.groupOrderIndex == groupOrderIndex)
                    return g.groupId;
            }
            return null;
        }

        private float GetGroupVisibleAfterCompletion01(GroupIndex g)
        {
            return GetGroupVisibleAfterCompletion01(g.group);
        }

        private float GetGroupVisibleAfterCompletion01(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId) || !_groupById.TryGetValue(groupId, out var g))
                return 1f;
            return GetGroupVisibleAfterCompletion01(g.group);
        }

        private float GetGroupVisibleAfterCompletion01(CodexChapterGroup g)
        {
            // Unterstützt beide Varianten:
            // - visibleAfterCompletion01 (neu)
            // - visibleAfterProgress (alt)
            var t = g.GetType();
            var f1 = t.GetField("visibleAfterCompletion01");
            if (f1 != null)
                return Mathf.Clamp01((float)f1.GetValue(g));

            var f2 = t.GetField("visibleAfterProgress");
            if (f2 != null)
                return Mathf.Clamp01((float)f2.GetValue(g));

            return 1f;
        }

        private string GetDependsOnGroupId(CodexChapterGroup g)
        {
            // Unterstützt:
            // - dependsOnGroupId (neu)
            // - visibleAfterGroupIndex (alt, aber als string – missbraucht) -> akzeptieren wir als groupId.
            var t = g.GetType();
            var f1 = t.GetField("dependsOnGroupId");
            if (f1 != null)
                return (string)f1.GetValue(g);

            var f2 = t.GetField("visibleAfterGroupIndex");
            if (f2 != null)
                return (string)f2.GetValue(g);

            return null;
        }

        private string GetUnlockAfterDeedId(DeedIndex d)
        {
            var t = d.slot.GetType();
            var f = t.GetField("unlockAfterDeedId");
            if (f != null) return (string)f.GetValue(d.slot);
            return null;
        }

        private float GetUnlockAfterProgress01(DeedIndex d)
        {
            var t = d.slot.GetType();

            // neu: unlockAfterProgress01
            var f1 = t.GetField("unlockAfterProgress01");
            if (f1 != null) return Mathf.Clamp01((float)f1.GetValue(d.slot));

            // alt: unlockAfterProgress
            var f2 = t.GetField("unlockAfterProgress");
            if (f2 != null) return Mathf.Clamp01((float)f2.GetValue(d.slot));

            return 1f;
        }

        private string GetChapterId(CodexChapter ch)
        {
            // Du kannst später explizit chapterId einführen.
            // Bis dahin: chapterName ist die stabile ID, sofern du sie stabil hältst.
            return ch.chapterId;
        }

        private string GetGroupId(CodexChapterGroup g, string chapterId, int groupOrderIndex)
        {
            if (!string.IsNullOrWhiteSpace(g.groupid))
                return g.groupid;

            //fallback:
            return $"{chapterId}:GrouIdx:{groupOrderIndex}";
        }

        private string GetDeedId(DeedSlot slot)
        {
            // Neu: deedId
            var t = slot.GetType();
            var f = t.GetField("deedId");
            if (f != null)
            {
                var v = (string)f.GetValue(slot);
                if (!string.IsNullOrWhiteSpace(v)) return v;
            }

            // Alt: deed ref -> deed.id
            if (slot.deed != null && !string.IsNullOrWhiteSpace(slot.deed.id))
                return slot.deed.id;

            return null;
        }

        private bool IsVisibleUnderChainClamp(DeedIndex deed)
        {
            // Policy: in einer Gruppe maximal N "future" (nicht-available) Deeds sichtbar.
            // Wir interpretieren "future" = Deed ist visible (group visible) aber NOT available.

            if (!_groupById.TryGetValue(deed.groupId, out var g)) return true;
            if (g.group.deedSlots == null) return true;

            int futureVisibleBudget = _cfg.maxFutureDeedsVisible;

            for (int i = 0; i < g.group.deedSlots.Count; i++)
            {
                var slot = g.group.deedSlots[i];
                var id = GetDeedId(slot);
                if (string.IsNullOrWhiteSpace(id)) continue;

                // Available?
                bool available = ComputeDeedGate(id).isAvailable;

                if (!available)
                {
                    // Das ist ein "future" Kandidat.
                    if (id == deed.deedId)
                    {
                        // Ist dieses Deed innerhalb des Budgets?
                        return futureVisibleBudget > 0;
                    }
                    futureVisibleBudget--;
                    if (futureVisibleBudget < 0) futureVisibleBudget = 0;
                }

                // Sobald wir unseren Target getroffen haben, kann man abbrechen – aber wir haben das oben schon.
            }

            return true;
        }

        // -------------------------
        // Small index structs
        // -------------------------

        private readonly struct ChapterIndex
        {
            public readonly string chapterId;
            public readonly CodexChapter chapter;
            public ChapterIndex(string chapterId, CodexChapter chapter)
            {
                this.chapterId = chapterId;
                this.chapter = chapter;
            }
        }

        private readonly struct GroupIndex
        {
            public readonly string groupId;
            public readonly string chapterId;
            public readonly int groupOrderIndex;
            public readonly CodexChapterGroup group;

            public GroupIndex(string groupId, string chapterId, int groupOrderIndex, CodexChapterGroup group)
            {
                this.groupId = groupId;
                this.chapterId = chapterId;
                this.groupOrderIndex = groupOrderIndex;
                this.group = group;
            }
        }

        private readonly struct DeedIndex
        {
            public readonly string deedId;
            public readonly string chapterId;
            public readonly string groupId;
            public readonly int groupOrderIndex;
            public readonly int slotOrderIndex;
            public readonly DeedSlot slot;

            public DeedIndex(string deedId, string chapterId, string groupId, int groupOrderIndex, int slotOrderIndex, DeedSlot slot)
            {
                this.deedId = deedId;
                this.chapterId = chapterId;
                this.groupId = groupId;
                this.groupOrderIndex = groupOrderIndex;
                this.slotOrderIndex = slotOrderIndex;
                this.slot = slot;
            }
        }
    }
}
