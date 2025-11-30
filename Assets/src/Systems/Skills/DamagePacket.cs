using System;
using System.Collections.Generic;
using CHAL.Data;

namespace CHAL.Systems.Skill
{

    [Serializable]
    public class DamagePacket
    {
     
        public Dictionary<DamageType, float> DamagePerType { get; } = new();

        public float TotalDamageBeforeDef { get; private set; }
  
        public bool IsHitBased { get; set; } = true;
  
        public bool IsDot { get; set; } = false;

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
