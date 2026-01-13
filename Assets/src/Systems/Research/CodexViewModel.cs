using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Codex
{
    [Serializable]
    public sealed class ChapterVM
    {
        public string chapterId;

        public List<GroupVM> groups = new List<GroupVM>();
    }

    [Serializable]
    public sealed class GroupVM
    {
        public string groupId;

        public GroupGateState gate;

        public List<DeedVM> deeds = new List<DeedVM>();
    }

    [Serializable]
    public sealed class DeedVM
    {
        public string deedId;

        public string title;

        public DeedGateState gate;

        public float progress01;
        public bool completed;
        public bool claimed;

        public bool isActive;
        public int activeSlotIndex; // -1 wenn nicht aktiv
        public bool isSlotLocked;    // abgeleitet aus "claimable"
    }
}
