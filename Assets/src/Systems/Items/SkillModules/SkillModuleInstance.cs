using CHAL.Data;
using System;
using UnityEngine;

namespace CHAL.Systems.Items
{
    /// <summary>
    /// Persisted payload for a concrete SkillModule variant (module item + frameTier + coreType).
    /// IMPORTANT: instanceId is a deterministic VariantKey to enable stacking.
    /// </summary>
    [Serializable] public class SkillModuleInstance
    {
        public string instanceId;      // deterministic variant key (NOT a GUID)
        public string moduleItemId;    // e.g. "module:fireball"
        public string skillId;         // e.g. "fireball"
        public int frameTier;          // 1..N
        public CoreType coreType;      // Kinetic/Blazing/...

/// <summary>
/// Builds a variant key based on the provided parameters.
/// </summary>
/// <param name="moduleItemId">The ID of the module item.</param>
/// <param name="frameTier">The frame tier, must be at least 1.</param>
/// <param name="coreType">The core type associated with the variant.</param>
/// <returns>A string representing the variant key.</returns>
        public static string BuildVariantKey(string moduleItemId, int frameTier, CoreType coreType)
        {
            // stable, readable, safe for JSON
            return $"sm:{moduleItemId}:t_{frameTier}:c_{coreType}";
        }

/// <summary>
/// Creates a new instance of SkillModuleInstance with the specified parameters.
/// </summary>
/// <param name="moduleItemId">The ID of the module item.</param>
/// <param name="skillId">The ID of the skill.</param>
/// <param name="frameTier">The frame tier, must be at least 1.</param>
/// <param name="coreType">The core type associated with the instance.</param>
/// <returns>A new SkillModuleInstance object.</returns>
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
