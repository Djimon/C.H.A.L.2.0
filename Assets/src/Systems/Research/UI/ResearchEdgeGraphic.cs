using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CHAL.Core; // DebugManager

namespace CHAL.Systems.Research
{
    // Einfache UI-Linie (orthogonal mit kleinem Bezier-Knick), generiert als Mesh.
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class ResearchEdgeGraphic : MaskableGraphic
    {
        public Vector2 start;
        public Vector2 end;
        [Min(0.5f)] public float thickness = 2f;
        [Range(0f, 1f)] public float cornerRadius = 0.25f; // 0..1 Anteil des kürzeren Segments
        public bool useCompletedColor;
        public Color completedColor = new Color(0.6f, 1f, 0.6f, 1f);

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (color.a <= 0.001f) return;

            // Orthogonale Route: horizontal von start.x -> end.x auf start.y, dann vertikal zu end.y
            var p0 = start;
            var p1 = new Vector2(end.x, start.y);
            var p2 = end;

            Color col = useCompletedColor ? completedColor : color;

            // Kleiner Radius an der Ecke p1
            float hLen = Mathf.Abs(p1.x - p0.x);
            float vLen = Mathf.Abs(p2.y - p1.y);
            float cr = Mathf.Min(hLen, vLen) * cornerRadius;

            // Segment 1 (p0 -> p1), gekürzt
            Vector2 dirH = (p1 - p0).normalized;
            Vector2 a1 = p0;
            Vector2 b1 = p1 - dirH * cr;

            // Segment 2 (p1 -> p2), gekürzt
            Vector2 dirV = (p2 - p1).normalized;
            Vector2 a2 = p1 + dirV * cr;
            Vector2 b2 = p2;

            // Quad für Segment 1
            AddQuad(vh, a1, b1, thickness, col);
            // Quad für Segment 2
            AddQuad(vh, a2, b2, thickness, col);

            // Runde Ecke als kleines „Knie“ (approximiert mit 4 Quads)
            AddCorner(vh, b1, a2, thickness, col);
        }

        private static void AddQuad(VertexHelper vh, Vector2 a, Vector2 b, float thick, Color c)
        {
            if ((b - a).sqrMagnitude < 0.0001f) return;
            Vector2 n = Vector2.Perpendicular((b - a).normalized) * (thick * 0.5f);

            int idx = vh.currentVertCount;
            vh.AddVert(a - n, c, Vector2.zero);
            vh.AddVert(a + n, c, Vector2.zero);
            vh.AddVert(b + n, c, Vector2.zero);
            vh.AddVert(b - n, c, Vector2.zero);
            vh.AddTriangle(idx + 0, idx + 1, idx + 2);
            vh.AddTriangle(idx + 0, idx + 2, idx + 3);
        }

        private static void AddCorner(VertexHelper vh, Vector2 from, Vector2 to, float thick, Color c)
        {
            // vier kurze Quads entlang eines 90°-Bogensegments
            Vector2 dir = (to - from);
            float len = dir.magnitude;
            if (len < 0.001f) return;

            int steps = 4;
            Vector2 prev = from;
            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                // kleiner Bezier von 'from' zu 'to' über Ecke
                Vector2 p = Vector2.Lerp(from, to, t);
                AddQuad(vh, prev, p, thick, c);
                prev = p;
            }
        }
    }
}
