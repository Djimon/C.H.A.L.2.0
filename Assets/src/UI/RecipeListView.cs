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

            // Gruppierung nach slotType; null/Default => "Misc"
            var groups = recipes
                .GroupBy(r => r.slotType.ToString())
                .OrderBy(g => g.Key);

            foreach (var g in groups)
            {
                var fold = new Foldout { text = g.Key, value = true };
                fold.AddToClassList("group-foldout");
                _scroll.Add(fold);

                foreach (var r in g)
                {
                    var row = new VisualElement();
                    row.AddToClassList("recipe-row");

                    var btn = new Button(() => {
                        OnSelect?.Invoke(r);
                        Debug.Log($"[RecipeListView] Select: {r.name}");
                    })
                    {
                        text = string.IsNullOrEmpty(r.displayKey) ? r.name : r.displayKey
                    };

                    // Row zusätzlich klickbar (falls Styles Button verdecken)
                    row.RegisterCallback<ClickEvent>(_ => {
                        OnSelect?.Invoke(r);
                        Debug.Log($"[RecipeListView] RowClick: {r.name}");
                    });

                    row.Add(btn);
                    fold.Add(row);
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
