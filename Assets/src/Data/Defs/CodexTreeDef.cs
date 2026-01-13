using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "CodexTreeDef", menuName = "Research/Codex")]
    public sealed class CodexTreeDef : ScriptableObject
    {
        [Header("Lane Labels & Colors")]
        public List<Chapter> chapters = new List<Chapter>();


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
        public List<CodexChapter>   codexChapters = new List<CodexChapter>();

        // Helper für UI: Lane-Name & -Farbe aus Index holen
/// <summary>
/// Gets the name of the specified lane.
/// </summary>
/// <param name="lane">The index of the lane.</param>
/// <returns>The name of the lane, or "unknown lane" if the index is out of range.</returns>
        public string GetChapterName(int lane)
        {
            return (lane >= 0 && lane < chapters.Count)
                ? chapters[lane].chapterName
                : "unknown lane";
        }

/// <summary>
/// Gets the color of the specified lane.
/// </summary>
/// <param name="lane">The index of the lane.</param>
/// <returns>The color of the lane, or black if the index is out of range.</returns>
        public Color GetChapterColor(int lane)
        {
            return (lane >= 0 && lane < chapters.Count)
                ? chapters[lane].chapterColor
                : Color.black;
        }
    }

    [Serializable]
    public struct Chapter
    { 
        public string chapterName;
        public Color chapterColor;
    }

    [Serializable]
    public sealed class CodexChapter
    {
        public string chapterName;
        public Color chapterColor;

        public List<CodexStageGroup> stages = new List<CodexStageGroup>();
    }

    [Serializable]
    public sealed class CodexStageGroup
    {
        public string groupName;
        public List<CodexDeedRef> deeds = new List<CodexDeedRef>();
    }

    [Serializable]
    public sealed class CodexDeedRef
    {
        public CodexDeedDef node;
        [Tooltip("Parents als Node-Refs; wird zu IDs kompiliert.")]
        public List<CodexDeedDef> parentRefs = new List<CodexDeedDef>();
    }
}
