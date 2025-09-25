using UnityEngine;
using UnityEngine.UIElements;

public abstract class IngameUI : MonoBehaviour
{
    protected VisualElement root;

    protected virtual void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None; // start hidden
    }

    public virtual void Show(bool show)
    {
        root.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public bool IsVisible => root.style.display == DisplayStyle.Flex;
}