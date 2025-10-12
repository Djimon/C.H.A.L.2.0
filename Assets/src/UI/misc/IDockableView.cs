using UnityEngine.UIElements;

namespace CHAL.UI
{

    public interface IDockableView
    {
        // Pflicht: der äußere Menü-Container, den der Manager positioniert
        UIDocument doc { get;}

        VisualElement OuterContainer { get; }

        // Sichtbarkeit ausschließlich über UI Toolkit
        bool IsVisible { get; }

        // Interaktiv ja/nein (für ActiveInventories)
        bool ReadOnly { get; }

        // Dock-Settings
        DockEdge Edge { get; }
        int DockPriority { get; }
        bool AutoDock { get; }

        // Breitenvorgaben (für Left/Right)
        float BaseWidthPercent { get; }   // 0..1
        int MinWidthPx { get; }
        int MaxWidthPx { get; }

        // Identifikation/Typ
        string StableId { get; }          // für Logs/Debug
        bool IsInventoryView { get; }   // true für Inventare (ActiveInventories)
        bool IsItemCard { get; }        // true für ItemCard (zIndex=10)
    }
}
