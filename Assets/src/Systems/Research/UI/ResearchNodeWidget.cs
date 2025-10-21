using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CHAL.Core;
using CHAL.Data;
using TMPro;

namespace CHAL.Systems.Research
{
    public sealed class ResearchNodeWidget : MonoBehaviour, IPointerClickHandler
    {
        [Header("Bind")]
        public Image background;
        public Image icon;
        public TMP_Text title; // oder TMP_Text, dann Typ anpassen
        public Button button;

        [Header("Runtime")]
        public string nodeId;
        ResearchMapView _map;
        ResearchUIThemeDef _theme;

        // State-Farben
        Color _normalColor;
        Color _disabledColor;
        Color _completedColor;
        Color _highlightColor;
        float _highlightIntensity;

        public void Init(ResearchMapView map, string id, string titleText, Sprite iconSprite)
        {
            _map = map;
            _theme = map.theme;
            nodeId = id;

            _normalColor = _theme.nodeForegroundColor;
            _disabledColor = _theme.nodeDisabledColor;
            _completedColor = _theme.nodeCompletedColor;
            _highlightColor = _theme.highlightColor;
            _highlightIntensity = _theme.highlightIntensity;

            if (background) background.sprite = _theme.nodeBackground;
            if (icon) icon.sprite = iconSprite != null ? iconSprite : _theme.nodeIconDefault;
            if (title) title.text = titleText ?? id;

            ApplyState();
        }

        public void ApplyState(bool isSelected = false)
        {
            if (_map == null) return;
            bool completed = _map.service.IsCompleted(nodeId);
            bool available = _map.service.IsNodeAvailable(nodeId);
            bool isActive = _map.service.GetActiveNodeId() == nodeId;

            // Farben
            Color fg = _normalColor;
            if (!available && !completed) fg = _disabledColor;
            if (completed) fg = _completedColor;

            if (icon) icon.color = fg;
            if (title) title.color = fg;

            // leichte Highlight-Animation für aktive/ausgewählte
            if (background)
            {
                if (isActive || isSelected)
                {
                    var c = Color.Lerp(background.color, _highlightColor, _highlightIntensity);
                    c.a = 1f;
                    background.color = c;
                }
                else
                {
                    background.color = Color.white;
                }
            }

            if (button)
            {
                button.interactable = available && !completed && !isActive;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => _map.OnNodeClicked(nodeId));
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_map != null) _map.OnNodeClicked(nodeId);
        }
    }
}
