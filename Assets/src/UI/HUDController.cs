using CHAL.Core;
using CHAL.Systems.Codex;
using CHAL.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.Systems.UI
{
    /// <summary>
    /// HUD button that toggles the already-present CodexUI (child UIDocument).
    /// HUD is placed in scenes manually; CodexUI is a child of HUD in the prefab.
    /// </summary>
    public sealed class HudCodexController : IngameUI
    {
        [Header("References (optional)")]
        [SerializeField] private UIDocument codexDocument; // assign CodexUI UIDocument or leave null (auto-find)

        private GameManager _gm;
        private CodexService _codex;

        private Button _btnCodex;
        private Label _badge;

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

            root.pickingMode = PickingMode.Ignore; //Root blockiert nicht andere UIs

            BindHudUI();
            ResolveCodexDocument();
            HookEvents();

            Show(true);
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

            if (_btnCodex == null)
                DebugManager.Error("[HudCodexController] Missing Button with name 'btn-codex' in HUD UXML.");
            else
                _btnCodex.pickingMode = PickingMode.Position; //nur echte Buttons sind klickbar
        }

        private void ResolveCodexDocument()
        {
            if (codexDocument != null)
                return;

            // Find a child UIDocument (not our own)
            var docs = GetComponentsInChildren<UIDocument>(true);
            for (int i = 0; i < docs.Length; i++)
            {
                if (docs[i] == null) continue;
                if (docs[i] == GetComponent<UIDocument>()) continue;
                codexDocument = docs[i];
                break;
            }

            if (codexDocument == null)
                DebugManager.Error("[HudCodexController] Could not find child UIDocument for CodexUI. Assign 'codexDocument' in inspector.");
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

            if (codexDocument == null)
                return;

            var codexRoot = codexDocument.rootVisualElement;
            if (codexRoot == null)
                return;

            codexRoot.style.display = open ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnCodexChanged()
        {
            UpdateCodexBadge();
        }

        private void UpdateCodexBadge()
        {
            if (_badge == null || _codex == null)
                return;

            // Minimal: show progress% of active deed in slot 0.
            var deedId = _codex.GetActiveDeedId(0);
            if (string.IsNullOrWhiteSpace(deedId))
            {
                _badge.text = "";
                return;
            }

            // Find deed progress via VMs (cheap, counts small).
            float p = 0f;
            bool found = false;

            var chapters = _codex.GetChaptersVM();
            if (chapters != null)
            {
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
