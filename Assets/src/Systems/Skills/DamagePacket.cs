using System;
using System.Collections.Generic;
using CHAL.Data;

namespace CHAL.Systems.Skill
{

    [Serializable]
/// <summary>
/// Represents a packet of damage information, including damage types and totals.
/// </summary>
    public class DamagePacket
    {
     
        public Dictionary<DamageType, float> DamagePerType { get; } = new();

        public float TotalDamageBeforeDef { get; private set; }
  
        public bool IsHitBased { get; set; } = true;
  
        public bool IsDot { get; set; } = false;

/// <summary>
/// Adds damage of a specified type to the total damage.
/// </summary>
/// <param name="type">The type of damage to add.</param>
/// <param name="amount">The amount of damage to add.</param>
        public void AddDamage(DamageType type, float amount)
        {
            if (amount <= 0f)
                return;

            if (DamagePerType.TryGetValue(type, out var existing))
                DamagePerType[type] = existing + amount;
            else
                DamagePerType[type] = amount;

            TotalDamageBeforeDef += amount;
        }
    }
}
