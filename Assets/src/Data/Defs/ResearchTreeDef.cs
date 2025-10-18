using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "ResearchTreeDef", menuName = "Research/Tree")]
    public sealed class ResearchTreeDef : ScriptableObject
    {
        [Header("Lane Labels & Colors")]
        public List<ResearchLane> researchLanes = new List<ResearchLane>();


        [Header("Layout-Constants (UI)")]
        [Min(1)] public int nodeWidth = 240;
        [Min(1)] public int nodeHeight = 120;
        [Min(1)] public int stageStepY = 180;

        public List<int> laneBaseX = new List<int> { 300, 700, 1100, 1500 };

        public int topMarginY = 120;

        [Header("View (Chips/Gates)")]
        public Sprite defaultGateGlyph;

        // Helper für UI: Lane-Name & -Farbe aus Index holen
        public string GetLaneName(int lane)
        {
            return (lane >= 0 && lane < researchLanes.Count)
                ? researchLanes[lane].laneName
                : "unknown lane";
        }

        public Color GetLaneColor(int lane)
        {
            return (lane >= 0 && lane < researchLanes.Count)
                ? researchLanes[lane].laneColor
                : Color.black;
        }
    }

    [Serializable]
    public struct ResearchLane
    { 
        public string laneName;
        public Color laneColor;
    }
}
