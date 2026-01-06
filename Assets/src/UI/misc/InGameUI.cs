using UnityEngine;
using UnityEngine.UIElements;


namespace CHAL.UI
{

    public abstract class IngameUI : MonoBehaviour
    {
        protected VisualElement root;
        public string requiredFeatureID = "none";

        protected virtual void Awake()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            root.style.display = DisplayStyle.None; // start hidden
        }

/// <summary>
/// Shows or hides the UI element based on the specified flag.
/// </summary>
/// <param name="show">True to show the element; false to hide it.</param>
        public virtual void Show(bool show)
        {
            root.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public virtual void ToggleUI()
        {
            Show(!this.IsVisible);
        }

        public bool IsVisible => root.style.display == DisplayStyle.Flex;
    }
}
