using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "ResearchTreeDef", menuName = "Research/Tree")]
    public sealed class CodexTreeDef : ScriptableObject
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

        [Header("Initial unlocks")]
        public List<string> alwaysUnlockedIds = new List<string>();

        [Header("Actual research Tree")]
        public List<ResearchTreeLane>   researchTreeLanes = new List<ResearchTreeLane>();

        // Helper für UI: Lane-Name & -Farbe aus Index holen
/// <summary>
/// Gets the name of the specified lane.
/// </summary>
/// <param name="lane">The index of the lane.</param>
/// <returns>The name of the lane, or "unknown lane" if the index is out of range.</returns>
        public string GetLaneName(int lane)
        {
            return (lane >= 0 && lane < researchLanes.Count)
                ? researchLanes[lane].laneName
                : "unknown lane";
        }

/// <summary>
/// Gets the color of the specified lane.
/// </summary>
/// <param name="lane">The index of the lane.</param>
/// <returns>The color of the lane, or black if the index is out of range.</returns>
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

    [Serializable]
    public sealed class ResearchTreeLane
    {
        public string laneName;
        public Color laneColor;

        public List<ResearchTreeStage> stages = new List<ResearchTreeStage>();
    }

    [Serializable]
    public sealed class ResearchTreeStage
    {
        [Tooltip("Nodes in dieser Stage (gleiche Y-Ebene).")]
        public List<ResearchTreeNodeRef> nodes = new List<ResearchTreeNodeRef>();
    }

    [Serializable]
    public sealed class ResearchTreeNodeRef
    {
        public CodexNodeDef node;
        [Tooltip("Parents als Node-Refs; wird zu IDs kompiliert.")]
        public List<CodexNodeDef> parentRefs = new List<CodexNodeDef>();
    }
}
