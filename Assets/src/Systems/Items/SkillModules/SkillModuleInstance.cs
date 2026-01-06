using CHAL.Data;
using System;
using UnityEngine;

namespace CHAL.Systems.Items
{
    /// <summary>
    /// Persisted payload for a concrete SkillModule variant (module item + frameTier + coreType).
    /// IMPORTANT: instanceId is a deterministic VariantKey to enable stacking.
    /// </summary>
    [Serializable]
    public class SkillModuleInstance
    {
        public string instanceId;      // deterministic variant key (NOT a GUID)
        public string moduleItemId;    // e.g. "module:fireball"
        public string skillId;         // e.g. "fireball"
        public int frameTier;          // 1..N
        public CoreType coreType;      // Kinetic/Blazing/...

        public static string BuildVariantKey(string moduleItemId, int frameTier, CoreType coreType)
        {
            // stable, readable, safe for JSON
            return $"sm:{moduleItemId}:t_{frameTier}:c_{coreType}";
        }

        public static SkillModuleInstance Create(string moduleItemId, string skillId, int frameTier, CoreType coreType)
        {
            return new SkillModuleInstance
            {
                moduleItemId = moduleItemId,
                skillId = skillId,
                frameTier = Mathf.Max(1, frameTier),
                coreType = coreType,
                instanceId = BuildVariantKey(moduleItemId, frameTier, coreType)
            };
        }

    }
}
