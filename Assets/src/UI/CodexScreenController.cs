using CHAL.Core;
using CHAL.Systems.Codex;
using CHAL.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.Systems.UI
{
    /// <summary>
    /// UI placeholder wiring for CodexScreen. No game logic here:
    /// - pulls VMs from CodexService
    /// - routes button actions to CodexService
    /// - rebuilds UI on OnCodexChanged
    /// </summary>
    public sealed class CodexScreenController : IngameUI
    {
        // Services
        private GameManager _gm;
        private CodexService _codex;

        // UI refs (from CodexScreen.uxml)
        private Label _headerActiveDeedTitle;
        private Button _btnClaimHeader;

        private VisualElement _chaptersContainer;
        private VisualElement _stageGroupsContainer;

        private Label _detailsDeedName;
        private Label _detailsDeedDesc;
        private VisualElement _detailsProgressFill;
        private Label _detailsProgressText;

        private VisualElement _requirementsContainer;
        private VisualElement _rewardsContainer;

        private Button _btnActivate;

        // UI state
        private string _selectedChapterId;
        private string _selectedDeedId;

        // Cache for lookups (rebuilt on refresh)
        private readonly Dictionary<string, DeedVM> _deedById = new Dictionary<string, DeedVM>(StringComparer.Ordinal);

        protected override void Awake()
        {
            base.Awake();

            if (root == null)
            {
                DebugManager.Error("[CodexScreenController] root is null (IngameUI failed to init UIDocument).");
                enabled = false;
                return;
            }

            _gm = GameManager.Instance;
            if (_gm == null)
            {
                DebugManager.Error("[CodexScreenController] GameManager.Instance is null.");
                enabled = false;
                return;
            }

            _codex = _gm.codexService;
            if (_codex == null)
            {
                DebugManager.Error("[CodexScreenController] GameManager.codexService is null. (InitCodex not called?)");
                enabled = false;
                return;
            }

            BindUI();
            HookEvents();

            // Initial build
            RefreshAll();
        }

        private void OnDestroy()
        {
            if (_codex != null)
                _codex.OnCodexChanged -= OnCodexChanged;
        }

        private void BindUI()
        {
            // Header
            _headerActiveDeedTitle = root.Q<Label>("active-deed-title");
            _btnClaimHeader = root.Q<Button>("btn-claim-reward");

            // Lists
            _chaptersContainer = root.Q<VisualElement>("chapters-container");
            _stageGroupsContainer = root.Q<VisualElement>("stage-groups-container");

            // Details
            _detailsDeedName = root.Q<Label>("details-deed-name");
            _detailsDeedDesc = root.Q<Label>("details-deed-desc");
            _detailsProgressFill = root.Q<VisualElement>("details-progress-fill");
            _detailsProgressText = root.Q<Label>("details-progress-text");

            _requirementsContainer = root.Q<VisualElement>("requirements-container");
            _rewardsContainer = root.Q<VisualElement>("rewards-container");

            _btnActivate = root.Q<Button>("btn-activate");
        }

        private void HookEvents()
        {
            _codex.OnCodexChanged -= OnCodexChanged;
            _codex.OnCodexChanged += OnCodexChanged;

            if (_btnActivate != null)
            {
                _btnActivate.clicked -= OnActivateClicked;
                _btnActivate.clicked += OnActivateClicked;
            }

            if (_btnClaimHeader != null)
            {
                _btnClaimHeader.clicked -= OnClaimClicked;
                _btnClaimHeader.clicked += OnClaimClicked;
            }
        }

        private void OnCodexChanged()
        {
            RefreshAll();
        }

        private void RefreshAll()
        {
            // Pull everything
            var chapters = _codex.GetChaptersVM();

            // Pick chapter if missing
            if (string.IsNullOrWhiteSpace(_selectedChapterId))
            {
                if (chapters != null && chapters.Count > 0)
                    _selectedChapterId = chapters[0].chapterId;
            }

            BuildChaptersList(chapters);

            var chapterVm = !string.IsNullOrWhiteSpace(_selectedChapterId)
                ? _codex.GetChapterVM(_selectedChapterId)
                : null;

            BuildGroupsAndDeeds(chapterVm);

            // Update selection validity
            if (!string.IsNullOrWhiteSpace(_selectedDeedId) && !_deedById.ContainsKey(_selectedDeedId))
                _selectedDeedId = null;

            // Auto-select: active deed in slot0 if nothing selected
            if (string.IsNullOrWhiteSpace(_selectedDeedId))
            {
                var active0 = _codex.GetActiveDeedId(0);
                if (!string.IsNullOrWhiteSpace(active0) && _deedById.ContainsKey(active0))
                    _selectedDeedId = active0;
            }

            UpdateHeaderActiveDeed();
            UpdateDetails();
        }

        private void BuildChaptersList(IReadOnlyList<ChapterVM> chapters)
        {
            if (_chaptersContainer == null) return;

            _chaptersContainer.Clear();

            if (chapters == null) return;

            for (int i = 0; i < chapters.Count; i++)
            {
                var ch = chapters[i];
                if (ch == null || string.IsNullOrWhiteSpace(ch.chapterId)) continue;

                var btn = new Button();
                btn.text = ch.chapterId;

                // simple selected styling hook via class
                btn.AddToClassList("chapter-entry");
                if (string.Equals(ch.chapterId, _selectedChapterId, StringComparison.Ordinal))
                    btn.AddToClassList("selected");

                string capturedId = ch.chapterId;
                btn.clicked += () =>
                {
                    _selectedChapterId = capturedId;
                    _selectedDeedId = null; // reset selection on chapter switch
                    RefreshAll();
                };

                _chaptersContainer.Add(btn);
            }
        }

        private void BuildGroupsAndDeeds(ChapterVM chapterVm)
        {
            _deedById.Clear();

            if (_stageGroupsContainer == null) return;
            _stageGroupsContainer.Clear();

            if (chapterVm == null || chapterVm.groups == null) return;

            for (int gi = 0; gi < chapterVm.groups.Count; gi++)
            {
                var g = chapterVm.groups[gi];
                if (g == null) continue;

                // Group visibility gate
                if (!g.gate.isVisible)
                    continue;

                var foldout = new Foldout();
                foldout.text = BuildGroupTitle(g);
                foldout.value = true;
                foldout.AddToClassList("group-foldout");

                if (g.deeds != null)
                {
                    for (int di = 0; di < g.deeds.Count; di++)
                    {
                        var d = g.deeds[di];
                        if (d == null || string.IsNullOrWhiteSpace(d.deedId)) continue;

                        // Deed visibility gate
                        if (!d.gate.isVisible)
                            continue;

                        _deedById[d.deedId] = d;

                        var row = BuildDeedRow(d);
                        foldout.Add(row);
                    }
                }

                _stageGroupsContainer.Add(foldout);
            }
        }

        private string BuildGroupTitle(GroupVM g)
        {
            // Minimal: groupId + completion percent
            float pct = Mathf.RoundToInt(Mathf.Clamp01(g.gate.completion01) * 100f);
            return $"{g.groupId} ({pct}%)";
        }

        private VisualElement BuildDeedRow(DeedVM d)
        {
            var row = new VisualElement();
            row.AddToClassList("deed-row");

            // Button area (select)
            var btn = new Button();
            btn.text = BuildDeedRowText(d);
            btn.AddToClassList("deed-btn");

            if (string.Equals(d.deedId, _selectedDeedId, StringComparison.Ordinal))
                btn.AddToClassList("selected");

            // Availability = interactable
            btn.SetEnabled(d.gate.isAvailable);

            string capturedId = d.deedId;
            btn.clicked += () =>
            {
                _selectedDeedId = capturedId;
                UpdateDetails();
                // refresh selection highlight
                RefreshAll();
            };

            row.Add(btn);

            // Optional small "active" chip
            if (d.isActive)
            {
                var chip = new Label($"ACTIVE S{d.activeSlotIndex}");
                chip.AddToClassList("chip");
                row.Add(chip);
            }

            // Optional locked indicator (slot locked until claim)
            if (d.isSlotLocked)
            {
                var lockLabel = new Label("LOCKED");
                lockLabel.AddToClassList("chip");
                row.Add(lockLabel);
            }

            return row;
        }

        private string BuildDeedRowText(DeedVM d)
        {
            // Minimal, but informative
            if (d.claimed) return $"{d.title} ✓ (claimed)";
            if (d.completed || d.progress01 >= 0.9999f) return $"{d.title} ✓ (claimable)";
            int pct = Mathf.RoundToInt(Mathf.Clamp01(d.progress01) * 100f);
            return $"{d.title} ({pct}%)";
        }

        private void UpdateHeaderActiveDeed()
        {
            if (_headerActiveDeedTitle == null) return;

            var active0 = _codex.GetActiveDeedId(0);
            if (string.IsNullOrWhiteSpace(active0))
            {
                _headerActiveDeedTitle.text = "None";
                return;
            }

            if (_deedById.TryGetValue(active0, out var d))
                _headerActiveDeedTitle.text = d.title;
            else
                _headerActiveDeedTitle.text = active0;
        }

        private void UpdateDetails()
        {
            // Clear visuals if nothing selected
            if (string.IsNullOrWhiteSpace(_selectedDeedId) || !_deedById.TryGetValue(_selectedDeedId, out var d))
            {
                if (_detailsDeedName != null) _detailsDeedName.text = "-";
                if (_detailsDeedDesc != null) _detailsDeedDesc.text = "";
                SetProgress(0f, "");
                ClearContainers();
                UpdateDetailsButtons(null);
                return;
            }

            if (_detailsDeedName != null) _detailsDeedName.text = d.title;

            // Desc: currently not exposed through VM. Keep placeholder.
            if (_detailsDeedDesc != null)
            {
                if (!d.gate.isAvailable && d.gate.isVisible)
                {
                    // show why blocked if provided
                    string why = BuildBlockedText(d.gate);
                    _detailsDeedDesc.text = string.IsNullOrWhiteSpace(why) ? "Locked." : why;
                }
                else
                {
                    _detailsDeedDesc.text = d.claimed ? "Claimed." : "Select ACTIVATE to track progress.";
                }
            }

            int pct = Mathf.RoundToInt(Mathf.Clamp01(d.progress01) * 100f);
            SetProgress(d.progress01, $"{pct}%");

            // Requirements/Rewards are not part of VM yet -> placeholder clear
            ClearContainers();

            UpdateDetailsButtons(d);
        }

        private void UpdateDetailsButtons(DeedVM d)
        {
            // Activate button
            if (_btnActivate != null)
            {
                bool canActivate = d != null && d.gate.isAvailable && !d.claimed;

                // Slot lock rule: if slot0 currently locked (claimable), you must claim before switching.
                // We keep it simple: always activate to slot0 for now.
                if (canActivate && _codex.IsSlotLocked(0))
                    canActivate = false;

                _btnActivate.SetEnabled(canActivate);
            }

            // Header claim button
            if (_btnClaimHeader != null)
            {
                // Claim allowed only if deed is active in any slot AND claimable (your CodexService enforces active-slot claim)
                bool claimable = d != null && _codex.IsClaimable(d.deedId);
                bool isActive = d != null && d.isActive;
                _btnClaimHeader.SetEnabled(claimable && isActive);
            }
        }

        private void SetProgress(float progress01, string text)
        {
            progress01 = Mathf.Clamp01(progress01);

            if (_detailsProgressFill != null)
                _detailsProgressFill.style.width = Length.Percent(progress01 * 100f);

            if (_detailsProgressText != null)
                _detailsProgressText.text = string.IsNullOrWhiteSpace(text) ? "" : text;
        }

        private void ClearContainers()
        {
            if (_requirementsContainer != null) _requirementsContainer.Clear();
            if (_rewardsContainer != null) _rewardsContainer.Clear();
        }

        private string BuildBlockedText(DeedGateState gate)
        {
            // Keep it short: show first blocker
            if (!string.IsNullOrWhiteSpace(gate.blockedByDeedId))
            {
                int req = Mathf.RoundToInt(Mathf.Clamp01(gate.blockedByRequProgress01) * 100f);
                return $"Blocked by deed: {gate.blockedByDeedId} ({req}%).";
            }

            if (!string.IsNullOrWhiteSpace(gate.blockedByGroupId))
            {
                int req = Mathf.RoundToInt(Mathf.Clamp01(gate.blockedByRequGroupProgress01) * 100f);
                return $"Blocked by group: {gate.blockedByGroupId} ({req}%).";
            }

            return null;
        }

        private void OnActivateClicked()
        {
            if (string.IsNullOrWhiteSpace(_selectedDeedId))
                return;

            // For now: always slot 0 (UI has no slot picker yet)
            if (!_codex.TrySetActiveFocus(0, _selectedDeedId, out var reason))
            {
                if (!string.IsNullOrWhiteSpace(reason))
                    DebugManager.Log($"Codex Activate failed: {reason}", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
                return;
            }

            RefreshAll();
        }

        private void OnClaimClicked()
        {
            // Prefer selected deed (if active+claimable), otherwise active slot0
            string toClaim = null;

            if (!string.IsNullOrWhiteSpace(_selectedDeedId) &&
                _deedById.TryGetValue(_selectedDeedId, out var selected) &&
                selected.isActive &&
                _codex.IsClaimable(selected.deedId))
            {
                toClaim = selected.deedId;
            }
            else
            {
                var active0 = _codex.GetActiveDeedId(0);
                if (!string.IsNullOrWhiteSpace(active0) && _codex.IsClaimable(active0))
                    toClaim = active0;
            }

            if (string.IsNullOrWhiteSpace(toClaim))
                return;

            if (!_codex.TryClaim(toClaim, out var reason))
            {
                if (!string.IsNullOrWhiteSpace(reason))
                    DebugManager.Log($"Codex Claim failed: {reason}", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
                return;
            }

            RefreshAll();
        }
    }
}
