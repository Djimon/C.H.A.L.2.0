using System;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "ResearchUITheme", menuName = "Research/UI Theme")]
    public sealed class ResearchUIThemeDef : ScriptableObject
    {
        [Header("Map Background")]
        public Sprite mapBackground;
        [Tooltip("Füllfarbe unter dem Map-Background (falls Sprite transparent).")]
        public Color mapBackgroundTint = Color.white;

        [Header("Node Look")]
        public Sprite nodeBackground;
        public Sprite nodeIconDefault;
        [Tooltip("Farbe für Node-Text/Icon im Normalzustand.")]
        public Color nodeForegroundColor = Color.white;
        [Tooltip("Farbe für inaktive/gesperrte Nodes.")]
        public Color nodeDisabledColor = new Color(1f, 1f, 1f, 0.35f);
        [Tooltip("Farbe für abgeschlossene Nodes.")]
        public Color nodeCompletedColor = new Color(0.7f, 1f, 0.7f, 1f);

        [Header("Edges / Links")]
        public Color edgeColor = Color.white;
        [Min(0.5f)] public float edgeThickness = 2f;
        [Tooltip("Farbe für Edges von bereits erfüllten Parent-Links.")]
        public Color edgeCompletedColor = new Color(0.6f, 1f, 0.6f, 1f);

        [Header("Highlight (Auswahl/Aktiv)")]
        public Color highlightColor = new Color(1f, .85f, .2f, 1f);
        [Range(0f, 2f)] public float highlightIntensity = 1.0f;
        [Tooltip("Optionaler Glow-Multiplikator für aktive Node.")]
        [Range(0f, 2f)] public float activeGlow = 0.6f;

        [Header("Zoom (diskrete Stufen)")]
        [Tooltip("Diskrete Zoomstufen (Scale-Faktoren).")]
        public float[] zoomSteps = new float[] { 0.75f, 1.0f, 1.25f, 1.5f };
        [Tooltip("Default-Index in zoomSteps beim Öffnen.")]
        public int defaultZoomIndex = 1;

        [Header("Layout Hooks (später vom Tree genutzt)")]
        [Min(1)] public int nodeWidth = 240;
        [Min(1)] public int nodeHeight = 120;
        [Min(1)] public int stageStepY = 180;
        public int topMarginY = 120;
        [Tooltip("X-Basis pro Lane (muss zur Lane-Anzahl des Trees passen).")]
        public System.Collections.Generic.List<int> laneBaseX = new System.Collections.Generic.List<int> { 300, 700, 1100, 1500 };

        private void OnValidate()
        {
            // Zoom sanity
            if (zoomSteps == null || zoomSteps.Length == 0)
            {
                zoomSteps = new float[] { 1f };
                DebugManager.Log("ResearchUITheme: zoomSteps war leer – auf [1.0] gesetzt.",
                    DebugManager.EDebugLevel.Dev, "ResearchUI", LogType.Warning);
            }
            for (int i = 0; i < zoomSteps.Length; i++)
                if (zoomSteps[i] < 0.1f) zoomSteps[i] = 0.1f;

            if (defaultZoomIndex < 0 || defaultZoomIndex >= zoomSteps.Length)
            {
                defaultZoomIndex = Mathf.Clamp(defaultZoomIndex, 0, Mathf.Max(0, zoomSteps.Length - 1));
            }

            if (edgeThickness < 0.5f) edgeThickness = 0.5f;
            if (nodeWidth < 1) nodeWidth = 1;
            if (nodeHeight < 1) nodeHeight = 1;
            if (stageStepY < 1) stageStepY = 1;

            if (laneBaseX == null || laneBaseX.Count == 0)
            {
                laneBaseX = new System.Collections.Generic.List<int> { 300, 700, 1100, 1500 };
                DebugManager.Log("ResearchUITheme: laneBaseX war leer – Standardwerte gesetzt.",
                    DebugManager.EDebugLevel.Dev, "ResearchUI", LogType.Warning);
            }
        }
    }
}
