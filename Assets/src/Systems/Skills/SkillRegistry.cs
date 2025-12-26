using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    public sealed class SkillRegistry : ScriptableObject
    {
        private static SkillRegistry _instance;
        public static SkillRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateInstance<SkillRegistry>();
                    _instance.Reload();
                }
                return _instance;
            }
        }

        // IMPORTANT: adjust to your actual folder under Resources/
        // e.g. Resources/data/Skills/...
        private const string ResourcesPath = "data/Skills";

        private readonly Dictionary<string, SkillModuleDef> _byId = new();

        public void Reload()
        {
            _byId.Clear();

            var defs = Resources.LoadAll<SkillModuleDef>(ResourcesPath);
            foreach (var def in defs)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.SkillId))
                {
                    DebugManager.Warning($"[SkillRegistry] Skipping invalid SkillId in asset '{def?.name}'", "System");
                    continue;
                }

                if (_byId.ContainsKey(def.SkillId))
                {
                    DebugManager.Warning($"[SkillRegistry] Duplicate SkillId '{def.SkillId}' in asset '{def.name}'", "System");
                    continue;
                }

                _byId.Add(def.SkillId, def);
            }

            //TODO: Do some validations?

            DebugManager.Log($"[SkillRegistry] Loaded: {_byId.Count} skills from Resources/{ResourcesPath}",
                DebugManager.EDebugLevel.Production, "System");
        }

        public SkillModuleDef GetById(string skillId)
        {
            return _byId.TryGetValue(skillId, out var def) ? def : null;
        }

        public bool TryGet(string skillId, out SkillModuleDef def) => _byId.TryGetValue(skillId, out def);

        public IEnumerable<string> GetAllSkillIds() => _byId.Keys;
        public IEnumerable<SkillModuleDef> GetAllSkills() => _byId.Values;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorAutoReload()
        {
            if (!Application.isPlaying)
                Instance?.Reload();
        }

        internal void TriggertInstanc()
        {
            DebugManager.Log("trigger Instance form Itemregistry");
        }
#endif
    }
}
