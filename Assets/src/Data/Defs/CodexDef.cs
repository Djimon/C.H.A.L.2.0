using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "CodexDef", menuName = "Research/Codex")]
    public sealed class CodexDef : ScriptableObject
    {
        [Header("Chapters Labels & Colors")]
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

        [Header("Actual Codex")]
        public List<CodexChapter> codexChapters = new List<CodexChapter>();

        /// <summary>
        /// Gets the name of the specified lane.
        /// </summary>
        public string GetChapterName(int lane)
        {
            return (lane >= 0 && lane < chapters.Count)
                ? chapters[lane].chapterName
                : "unknown lane";
        }

        /// <summary>
        /// Gets the color of the specified lane.
        /// </summary>
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

        // entspricht deinem "Groups" Konzept (vormals "stages")
        public List<CodexChapterGroup> stages = new List<CodexChapterGroup>();
    }

    [Serializable]
    public sealed class CodexChapterGroup
    {
        public string groupid;
        public List<DeedSlot> deedSlots = new List<DeedSlot>();

        // Gate (b): Sichtbarkeit dieser Group abhängig von anderer Group (default: previous).
        // -1 => previous group (groupIndex - 1)
        public int dependsOnGroupId = -1;

        // completion01 basiert später auf "claimedCount/total"
        public float visibleAfterCompletion01 = 1f;
    }

    [Serializable]
    public sealed class DeedSlot
    {
        public CodexDeedDef deed;

        // Gate (a): innerhalb Group von anderem Deed abhängig
        public string unlockAfterDeedId;
        public float unlockAfterProgress01 = 1f;
    }
}
