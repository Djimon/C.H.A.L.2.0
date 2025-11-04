using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{
/// <summary>
/// Manages the crafting user interface in the game.
/// Inherits from IngameUI to provide additional functionality.
/// </summary>
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
