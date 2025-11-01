using CHAL.Systems.Crafting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{

    public sealed class RecipeDetailPanel : MonoBehaviour
    {
        public event Action OnCraftClicked;

        [Header("UXML refs")]
        [SerializeField] private UIDocument doc;

        // Header
        private VisualElement _icon;
        private Label _name;
        private Label _baseStats;
        private VisualElement _tooltip;
        private VisualElement _tooltipList;

        // Ingredients
        private VisualElement _ingList;

        // Refinement
        private VisualElement _refinePanel;
        private Label _refineValue;
        private SliderInt _refineSlider;

        // Action
        private Label _goldLabel;
        private Button _craftBtn;
        private Label _failLabel;

        private void Awake()
        {
            if (doc == null) doc = GetComponent<UIDocument>();
            var root = doc.rootVisualElement;

            _icon = root.Q<VisualElement>("icon");
            _name = root.Q<Label>("item-name");
            _baseStats = root.Q<Label>("base-stats");
            _tooltip = root.Q<VisualElement>("implicits-tooltip");
            _tooltipList = root.Q<VisualElement>("implicits-list");

            _ingList = root.Q<VisualElement>("ingredients-list");

            _refinePanel = root.Q<VisualElement>("refine-panel");
            _refineSlider = root.Q<SliderInt>("refine-slider");
            _refineValue = root.Q<Label>("refine-value");

            _goldLabel = root.Q<Label>("gold-label");
            _craftBtn = root.Q<Button>("craft-btn");
            _failLabel = root.Q<Label>("fail-label");

            // Tooltip toggeln, wenn "(?)" geklickt wird
            var help = root.Q<Label>("implicit-help");
            if (help != null)
            {
                help.RegisterCallback<ClickEvent>(_ => ToggleTooltip());
            }

            if (_craftBtn != null)
                _craftBtn.clicked += () => OnCraftClicked?.Invoke();

            if (_refineSlider != null)
                _refineSlider.RegisterValueChangedCallback(e => _refineValue.text = e.newValue.ToString());
        }

        public void Clear()
        {
            _name.text = "";
            _baseStats.text = "";
            _icon.style.backgroundImage = StyleKeyword.None;
            _ingList?.Clear();
            _goldLabel.text = "0 G";
            _craftBtn.SetEnabled(false);
            _failLabel.text = "";
            _tooltip.style.display = DisplayStyle.None;
            _refinePanel.style.display = DisplayStyle.None;
        }

        public void ShowRecipeDetails(RecipeDef r,
                         CraftingService.RecipePreview preview,
                         int needGold, int haveGold,
                         Dictionary<string, int> haveByItemId)
        {
            _failLabel.text = "";

            _name.text = string.IsNullOrEmpty(r.displayKey) ? r.name : r.displayKey;
            _baseStats.text = "base stats"; // Platzhalter – später via GearStatsProvider befüllen

            // Icon (falls vorhanden)
            if (r.icon != null)
                _icon.style.backgroundImage = new StyleBackground(r.icon);
            else
                _icon.style.backgroundImage = StyleKeyword.None;

            // Ingredients rendern
            _ingList?.Clear();
            if (r.inputs != null)
            {
                foreach (var need in r.inputs)
                {
                    var have = (haveByItemId != null && haveByItemId.TryGetValue(need.itemId, out var h)) ? h : 0;

                    var row = new VisualElement();
                    row.AddToClassList("ing-row");

                    var icon = new VisualElement();
                    icon.AddToClassList("ing-icon");
                    row.Add(icon);

                    var label = new Label($"{have}/{Mathf.Max(1, need.qty)}") { name = $"ing-{need.itemId}" };
                    label.AddToClassList("ing-text");
                    if (have < need.qty) label.style.opacity = 0.7f;

                    row.Add(label);
                    _ingList.Add(row);
                }
            }

            // Goldzeile
            _goldLabel.text = $"{needGold} G";
            if (haveGold < needGold) _goldLabel.style.opacity = 0.7f; else _goldLabel.style.opacity = 1f;

            // Craft-Button
            _craftBtn.SetEnabled(preview.canCraft);

            // (Refinement bleibt vorerst verborgen; Controller kann sie sichtbar schalten)
        }

        public void ShowFail(string message)
        {
            _failLabel.text = message ?? "";
        }

        public void ShowSuccess()
        {
            _failLabel.text = ""; // optional kurze Erfolgsmeldung/SFX extern
        }

        private void ToggleTooltip()
        {
            _tooltip.style.display = _tooltip.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;

            // Liste könnte hier dynamisch bestückt werden (z. B. aus RollProfile), vorerst leer/Platzhalter:
            if (_tooltipList.childCount == 0)
            {
                _tooltipList.Add(new Label("• implicit A"));
                _tooltipList.Add(new Label("• implicit B"));
                _tooltipList.Add(new Label("• implicit C"));
                _tooltipList.Add(new Label("• implicit D"));
            }
        }
    }
}
