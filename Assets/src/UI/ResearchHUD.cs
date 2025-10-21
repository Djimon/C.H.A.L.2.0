using CHAL.Data;
using CHAL.Systems.Research;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class ResearchHUD : MonoBehaviour
{
    // Bindings
    private UIDocument _doc;
    private VisualElement _root, _activeBox, _activeIcon, _detailPanel;
    private Label _activeName, _activePercent, _detailTitle, _detailFlavor, _detailUnlocks, _detailCosts;
    private Button _runButton;

    // External refs (vom Bootstrap oder MapView zuweisen)
    public ResearchService Service { get; private set; }
    public ResearchUIThemeDef Theme { get; private set; }

    // Intern
    private string _selectedNodeId;

    void Awake()
    {
        _doc = GetComponent<UIDocument>();
        var ve = _doc.rootVisualElement;

        _root = ve.Q<VisualElement>("root");
        _activeBox = ve.Q<VisualElement>("activeBox");
        _activeIcon = ve.Q<VisualElement>("activeIcon");
        _activeName = ve.Q<Label>("activeName");
        _activePercent = ve.Q<Label>("activePercent");

        _detailPanel = ve.Q<VisualElement>("detailPanel");
        _detailTitle = ve.Q<Label>("detailTitle");
        _detailFlavor = ve.Q<Label>("detailFlavor");
        _detailUnlocks = ve.Q<Label>("detailUnlocks");
        _detailCosts = ve.Q<Label>("detailCosts");
        _runButton = ve.Q<Button>("runButton");

        HideDetails();
    }

    public void Init(ResearchService service, ResearchUIThemeDef theme)
    {
        Service = service;
        Theme = theme;
        RefreshActive();
    }

    public void RefreshActive()
    {
        if (Service == null) return;

        var id = Service.GetActiveNodeId();
        if (string.IsNullOrEmpty(id))
        {
            _activeName.text = "Keine aktive Forschung";
            _activePercent.text = "0%";
            return;
        }

        var def = Service.GetNodeDef(id);
        if (def == null)
        {
            DebugManager.Log($"HUD_UITK: Active node '{id}' not found.", DebugManager.EDebugLevel.Dev, "ResearchUI", LogType.Warning);
            return;
        }

        _activeName.text = string.IsNullOrEmpty(def.title) ? id : def.title;

        float p01 = Service.GetNodeProgress01(id); // 0..1
        _activePercent.text = Mathf.RoundToInt(p01 * 100f) + "%";

        // Optional: Theme-Icon als Background-Image der activeIcon setzen:
        if (_activeIcon != null && Theme?.nodeIconDefault != null)
        {
            var tex = Theme.nodeIconDefault.texture;
            _activeIcon.style.backgroundImage = new StyleBackground(tex);
        }
    }

    public void ShowDetails(string nodeId)
    {
        _selectedNodeId = nodeId;
        if (Service == null || string.IsNullOrEmpty(nodeId)) { HideDetails(); return; }

        var def = Service.GetNodeDef(nodeId);
        if (def == null) { HideDetails(); return; }

        if (_detailPanel != null) _detailPanel.SetEnabled(true);

        _detailTitle.text = string.IsNullOrEmpty(def.title) ? nodeId : def.title;
        _detailFlavor.text = string.IsNullOrEmpty(def.desc) ? "" : def.desc;
        _detailUnlocks.text = ResearchUIFormat.FormatUnlocks(def);
        _detailCosts.text = ResearchUIFormat.FormatRequirements(def);

        if (_runButton != null)
        {
            bool canRun = Service.IsNodeAvailable(nodeId) && !Service.IsCompleted(nodeId) && Service.GetActiveNodeId() != nodeId;
            _runButton.SetEnabled(canRun);
            _runButton.clicked -= OnRunClicked; // sicherheitshalber abmelden
            _runButton.clicked += OnRunClicked;
        }

        if (_detailPanel != null) _detailPanel.style.display = DisplayStyle.Flex;
    }

    public void HideDetails()
    {
        if (_detailPanel != null) _detailPanel.style.display = DisplayStyle.None;
        _selectedNodeId = null;
    }

    private void OnRunClicked()
    {
        if (Service == null || string.IsNullOrEmpty(_selectedNodeId)) return;
        if (Service.SetActive(_selectedNodeId))
        {
            RefreshActive();
            ShowDetails(_selectedNodeId); // Refresh (Button disabled, etc.)
        }
    }

    public bool IsPointerOverUI(Vector2 screenPos)
    {
        if (_doc == null || _doc.rootVisualElement == null) return false;
        var panel = _doc.rootVisualElement.panel;
        if (panel == null) return false;

        // Screen → Panel-Koordinaten
        Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(panel, screenPos);

        // 1) Detailpanel (nur wenn sichtbar)
        if (_detailPanel != null && _detailPanel.resolvedStyle.display == DisplayStyle.Flex)
        {
            if (_detailPanel.worldBound.Contains(panelPos))
                return true;
        }

        // 2) Active-Box
        if (_activeBox != null && _activeBox.worldBound.Contains(panelPos))
            return true;

        return false;
    }
}

public static class ResearchUIFormat
{
    public static string FormatUnlocks(ResearchNodeDef def)
    {
        if (def == null || def.unlocks == null || def.unlocks.Count == 0) return "—";
        var sb = new StringBuilder();
        foreach (var u in def.unlocks)
            sb.AppendLine($"• {u.unlockType}: {u.targetId}");
        return sb.ToString();
    }

    public static string FormatRequirements(ResearchNodeDef def)
    {
        if (def == null || def.requirements == null) return "—";
        var r = def.requirements;
        var sb = new StringBuilder();

        if (r.waves > 0) sb.AppendLine($"• Wellen: {r.waves}");
        if (r.maps > 0) sb.AppendLine($"• Karten: {r.maps}");

        if (r.mapRequirements != null)
            foreach (var mr in r.mapRequirements)
                sb.AppendLine($"• Karten ({mr.difficulty}): {mr.amount}");

        if (r.killsGeneral > 0)
            sb.AppendLine($"• Kills (gesamt, gewichtet): {r.killsGeneral}");

        if (r.killsByTag != null)
            foreach (var kc in r.killsByTag)
                if (kc != null && !string.IsNullOrEmpty(kc.enemyTag))
                    sb.AppendLine($"• Kills [{kc.enemyTag}]: {kc.count}");

        if (r.eliteCount > 0) sb.AppendLine($"• Elites: {r.eliteCount}");
        if (r.bossCount > 0) sb.AppendLine($"• Bosse:  {r.bossCount}");

        return sb.Length == 0 ? "—" : sb.ToString();
    }
}
