using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{
    public class CraftingUI : IngameUI
    {
        private Button _btnExit;

        protected override void Awake()
        {
            base.Awake(); // setzt root & versteckt das UI
            var doc = GetComponent<UIDocument>();
            root = doc.rootVisualElement;

            _btnExit = root.Q<Button>("exit"); // optionaler Close-Button in deiner UXML
            if (_btnExit != null)
                _btnExit.clicked += () => Show(false);
        }
    }
}