using CHAL.UI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{
    [DefaultExecutionOrder(-50)]
    public sealed class UIDockingManager : MonoBehaviour
    {
        public static UIDockingManager Instance { get; private set; }

        public int DockSpacing = 8;

        // alle registrierten Views, keine Scene-Scans
        private readonly List<IDockableView> _views = new();
        private bool _relayoutQueued;

        //ghost
        private List<UIDocument> _docs = new();
        public IReadOnlyList<UIDocument> ActiveDocs => _docs; // deine interne Liste
        public event Action<UIDocument> OnDocAdded;
        public event Action<UIDocument> OnDocRemoved;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[UIDockingManager] Es existiert bereits eine Instanz.");
                enabled = false;
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            if (_relayoutQueued)
            {
                _relayoutQueued = false;
                Relayout();
            }
        }

        // -------- Registrierung --------

        public void Register(IDockableView view)
        {
            if (view == null || _views.Contains(view)) return;
            _views.Add(view);
            if (view.doc != null && !_docs.Contains(view.doc))
            {
                _docs.Add(view.doc);
                OnDocAdded?.Invoke(view.doc);     // <-- Event feuern
            }
            EnsureAbsolutePosition(view.OuterContainer);
            QueueRelayout();
        }

        public void Unregister(IDockableView view)
        {
            if (view == null) return;
            _views.Remove(view);
            if (view.doc != null && _docs.Remove(view.doc))
                OnDocRemoved?.Invoke(view.doc);   // <-- Event feuern
            QueueRelayout();
        }

        /// Call, wenn View-Properties/Visibility geändert wurden.
        public void NotifyViewChanged(IDockableView view)
        {
            if (view == null) return;
            QueueRelayout();
        }

        public void QueueRelayout() => _relayoutQueued = true;

        // -------- Abfragen --------

        /// Sichtbare, interaktive Inventare (AutoDock egal).
        public IReadOnlyList<IDockableView> GetActiveInventories()
        {
            return _views.Where(v =>
                        v != null &&
                        v.IsVisible &&
                        v.IsInventoryView &&
                        !v.ReadOnly)
                 .OrderBy(v => v.DockPriority)
                 .ToList();
        }

        // -------- Layout --------

        private void Relayout()
        {
            // Nach Panel gruppieren (Views in verschiedenen Panels werden getrennt gelayoutet)
            var byPanel = _views
                .Where(v => v != null && v.OuterContainer != null && v.IsVisible)
                .GroupBy(v => v.OuterContainer.panel)
                .ToList();

            foreach (var panelGroup in byPanel)
            {
                var panel = panelGroup.Key;
                if (panel == null) continue;

                var panelRoot = panel.visualTree;
                float panelWidth = panelRoot?.resolvedStyle.width ?? 0f;
                if (panelWidth <= 0f) continue;

                // Links/ Rechts getrennt sammeln + sortieren (unabhängige Gruppen!)
                var left = panelGroup.Where(v => v.Edge == DockEdge.Left)
                                     .OrderBy(v => v.DockPriority)
                                     .ToList();
                var right = panelGroup.Where(v => v.Edge == DockEdge.Right)
                                      .OrderBy(v => v.DockPriority)
                                      .ToList();

                LayoutLeft(left, panelWidth);
                LayoutRight(right, panelWidth);
            }
        }

        private static void EnsureAbsolutePosition(VisualElement ve)
        {
            if (ve == null) return;
            // absolut, damit wir left/right setzen können (relativ zum passenden Container)
            if (ve.style.position != Position.Absolute)
                ve.style.position = Position.Absolute;
        }

        private void LayoutLeft(List<IDockableView> items, float panelWidth)
        {
            float offsetFromLeft = 0f;

            foreach (var v in items)
            {
                var ve = v.OuterContainer;
                if (ve == null) continue;

                // Breite anhand Panelbreite berechnen
                int widthPx = ComputeWidthPx(panelWidth, v.BaseWidthPercent, v.MinWidthPx, v.MaxWidthPx);

                if (v.AutoDock)
                {
                    // HARTE Anbindung an linke Kante + Gegenseite zurücksetzen
                    ve.style.left = offsetFromLeft;
                    ve.style.right = StyleKeyword.Auto;
                    ve.style.width = widthPx;

                    offsetFromLeft += widthPx + DockSpacing; 
                }
                // AutoDock=false → Position bleibt unberührt
            }
        }

        private void LayoutRight(List<IDockableView> items, float panelWidth)
        {
            float offsetFromRight = 0f;

            foreach (var v in items)
            {
                var ve = v.OuterContainer;
                if (ve == null) continue;

                int widthPx = ComputeWidthPx(panelWidth, v.BaseWidthPercent, v.MinWidthPx, v.MaxWidthPx);

                if (v.AutoDock)
                {
                    // HARTE Anbindung an rechte Kante + Gegenseite zurücksetzen
                    ve.style.right = offsetFromRight;
                    ve.style.left = StyleKeyword.Auto;
                    ve.style.width = widthPx;

                    offsetFromRight += widthPx + DockSpacing;
                }
            }
        }

        private static int ComputeWidthPx(float panelWidth, float basePercent, int minPx, int maxPx)
        {
            float w = Mathf.Clamp01(basePercent) * Mathf.Max(0f, panelWidth);
            int px = Mathf.Max(1, Mathf.FloorToInt(w));
            if (minPx > 0) px = Mathf.Max(px, minPx);
            if (maxPx > 0) px = Mathf.Min(px, maxPx);
            return px;
        }

        public InventoryView? GetOtherInventory(InventoryView caller)
        {
            if (caller == null) return null;

            var candidates = _views
                .Where(v =>
                    v != null &&
                    v.IsInventoryView &&
                    v.IsVisible &&
                    !v.ReadOnly &&
                    v is InventoryView iv && iv != caller)
                .Cast<InventoryView>()
                .ToList();

            return candidates.Count == 1 ? candidates[0] : null;
        }
    }
}
