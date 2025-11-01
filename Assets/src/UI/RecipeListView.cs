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

        public void SetData(IEnumerable<RecipeDef> recipes, IDictionary<RecipeDef, bool> craftableMap)
        {
            _scroll?.Clear();
            if (_scroll == null || recipes == null) return;

            var groups = recipes
                .GroupBy(r => (r != null ? r.slotType.ToString() : "Misc"))
                .OrderBy(g => g.Key);

            foreach (var g in groups)
            {
                var fold = new Foldout { text = g.Key, value = true };
                fold.AddToClassList("group-foldout");
                _scroll.Add(fold);

                foreach (var r in g)
                {
                    if (r == null) continue;

                    var row = new VisualElement();
                    row.AddToClassList("recipe-row");

                    var btn = new Button(() => OnSelect?.Invoke(r))
                    {
                        text = string.IsNullOrEmpty(r.displayKey) ? r.name : r.displayKey
                    };
                    btn.AddToClassList("recipe-btn");
                    btn.style.unityTextAlign = TextAnchor.MiddleLeft;

                    // Klassen aus Map setzen (fehlt Eintrag => missing)
                    var craftable = false;
                    if (craftableMap != null) craftableMap.TryGetValue(r, out craftable);
                    btn.EnableInClassList("is-craftable", craftable);
                    btn.EnableInClassList("is-missing", !craftable);

                    row.RegisterCallback<ClickEvent>(_ => OnSelect?.Invoke(r));
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
