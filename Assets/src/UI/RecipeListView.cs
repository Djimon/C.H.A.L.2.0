using CHAL.Systems.Crafting;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{
    public sealed class RecipeListView : MonoBehaviour
    {
        public event Action<RecipeDef> OnSelect;

        [Header("UXML refs")]
        [SerializeField] private UIDocument doc;

        private ScrollView _scroll;

        private void Awake()
        {
            if (doc == null) doc = GetComponent<UIDocument>();
            _scroll = doc.rootVisualElement.Q<ScrollView>("list-scroll");
        }

        public void SetData(IEnumerable<RecipeDef> recipes)
        {
            _scroll?.Clear();
            if (recipes == null) return;

            // Gruppierung nach slotType (GearType) – falls null, als "Misc" einsortieren
            var groups = recipes
                .GroupBy(r => r.slotType.ToString())
                .OrderBy(g => g.Key);

            foreach (var g in groups)
            {
                var header = new Label(g.Key) { pickingMode = PickingMode.Ignore };
                header.AddToClassList("group-header");
                _scroll.Add(header);

                foreach (var r in g)
                {
                    var row = MakeRow(r);
                    _scroll.Add(row);
                }
            }
        }

        private VisualElement MakeRow(RecipeDef r)
        {
            var row = new VisualElement();
            row.AddToClassList("recipe-row");

            var btn = new Button(() => OnSelect?.Invoke(r))
            {
                text = string.IsNullOrEmpty(r.displayKey) ? r.name : r.displayKey
            };
            btn.style.unityTextAlign = TextAnchor.MiddleLeft;

            row.Add(btn);
            return row;
        }
    }
}
