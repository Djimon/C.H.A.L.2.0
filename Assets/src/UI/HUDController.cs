using CHAL.Core;
using CHAL.Systems.Codex;
using CHAL.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.Systems.UI
{
    /// <summary>
    /// HUD button (top-left) that toggles the Codex UI as a child overlay.
    /// Place this HUD in both Map and Hideout scenes.
    /// </summary>
    public sealed class HudCodexController : IngameUI
    {
        [Header("UXML References")]
        [SerializeField] private VisualTreeAsset codexScreenUxml; // assign CodexScreen.uxml here

        private GameManager _gm;
        private CodexService _codex;

        private Button _btnCodex;
        private Label _badge;

        private VisualElement _overlayHost;
        private VisualElement _codexRoot;
        private bool _isOpen;

        protected override void Awake()
        {
            base.Awake();

            _gm = GameManager.Instance;
            if (_gm == null)
            {
                DebugManager.Error("[HudCodexController] GameManager.Instance is null.");
                enabled = false;
                return;
            }

            _codex = _gm.codexService;
            if (_codex == null)
            {
                DebugManager.Error("[HudCodexController] GameManager.codexService is null (InitCodex not called?).");
                enabled = false;
                return;
            }

            BindHudUI();
            EnsureCodexChildCreated();
            HookEvents();

            SetOpen(false);
            UpdateCodexBadge();
        }

        private void OnDestroy()
        {
            if (_codex != null)
                _codex.OnCodexChanged -= OnCodexChanged;
        }

        private void BindHudUI()
        {
            _btnCodex = root.Q<Button>("btn-codex");
            _badge = root.Q<Label>("codex-badge");
            _overlayHost = root.Q<VisualElement>("codex-overlay-container");

            if (_btnCodex == null)
                DebugManager.Error("[HudCodexController] Missing Button 'btn-codex' in HUD UXML.");

            if (_overlayHost == null)
                DebugManager.Error("[HudCodexController] Missing VisualElement 'codex-overlay-container' in HUD UXML.");
        }

        private void EnsureCodexChildCreated()
        {
            if (_overlayHost == null) return;
            if (_codexRoot != null) return;

            if (codexScreenUxml == null)
            {
                DebugManager.Error("[HudCodexController] codexScreenUxml not assigned (CodexScreen.uxml).");
                return;
            }

            // Create codex UI as child
            _codexRoot = codexScreenUxml.CloneTree();
            _codexRoot.name = "codex-overlay-root";

            // Optional: make it overlay-style
            _codexRoot.style.position = Position.Absolute;
            _codexRoot.style.left = 0;
            _codexRoot.style.top = 0;
            _codexRoot.style.right = 0;
            _codexRoot.style.bottom = 0;

            _overlayHost.Add(_codexRoot);

            // Attach the CodexScreenController to this same GameObject (recommended),
            // or ensure it exists elsewhere and points to the same UIDocument.
            //
            // If your CodexScreenController expects its own UIDocument root, the simplest approach:
            // - Put CodexScreenController on the same GameObject as this HUD UIDocument
            // - and let it use "root" already (IngameUI) — if it uses its own root, we adapt it next.
        }

        private void HookEvents()
        {
            if (_btnCodex != null)
            {
                _btnCodex.clicked -= ToggleOpen;
                _btnCodex.clicked += ToggleOpen;
            }

            _codex.OnCodexChanged -= OnCodexChanged;
            _codex.OnCodexChanged += OnCodexChanged;
        }

        private void ToggleOpen()
        {
            SetOpen(!_isOpen);
        }

        private void SetOpen(bool open)
        {
            _isOpen = open;

            if (_overlayHost != null)
                _overlayHost.style.display = open ? DisplayStyle.Flex : DisplayStyle.None;

            // Optional: pause input behind overlay etc. (später)
        }

        private void OnCodexChanged()
        {
            UpdateCodexBadge();

            // If open, CodexScreenController will refresh itself via OnCodexChanged anyway.
            // Nothing else needed here.
        }

        private void UpdateCodexBadge()
        {
            if (_badge == null) return;

            // Minimal, useful: show progress of active deed in slot 0
            var deedId = _codex.GetActiveDeedId(0);
            if (string.IsNullOrWhiteSpace(deedId))
            {
                _badge.text = "";
                return;
            }

            // We don't have a direct API for progress-by-id in CodexService? (depends on your current service)
            // So we use VM to get progress.
            var chapters = _codex.GetChaptersVM();
            if (chapters == null || chapters.Count == 0)
            {
                _badge.text = "";
                return;
            }

            // Find deed in any chapter (cheap; counts are small)
            float p = 0f;
            bool found = false;

            for (int ci = 0; ci < chapters.Count && !found; ci++)
            {
                var ch = _codex.GetChapterVM(chapters[ci].chapterId);
                if (ch?.groups == null) continue;

                for (int gi = 0; gi < ch.groups.Count && !found; gi++)
                {
                    var g = ch.groups[gi];
                    if (g?.deeds == null) continue;

                    for (int di = 0; di < g.deeds.Count; di++)
                    {
                        var d = g.deeds[di];
                        if (d != null && d.deedId == deedId)
                        {
                            p = d.progress01;
                            found = true;
                            break;
                        }
                    }
                }
            }

            if (!found)
            {
                _badge.text = "";
                return;
            }

            int pct = Mathf.RoundToInt(Mathf.Clamp01(p) * 100f);
            _badge.text = $"{pct}%";
        }
    }
}
