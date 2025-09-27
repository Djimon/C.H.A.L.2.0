using CHAL.Data;
using CHAL.Systems.Skill;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Enemy
{
    public class EnemyInstance : EffectReceiver   // deine abstrakte Basis für Effekte
    {
        public EnemyDef Definition { get; private set; }
        public EnemyStruct StructData { get; private set; }

        public event Action<EnemyInstance> OnDied;

        public EnemyInstance(EnemyDef def, EnemyStruct data)
        {
            Definition = def;
            StructData = data;

            MaxHP = def.baseHP;
            CurrentHP = MaxHP;
        }

        public override void TakeDamage(float amount, DamageType type)
        {
            // TODO: Armor/Resist später berücksichtigen
            CurrentHP -= amount;

            DebugManager.Log($"Enemy {StructData.EnemyId} took {amount} {type} damage (HP={CurrentHP}/{MaxHP})",
                DebugManager.EDebugLevel.Dev, "Fight");

            if (CurrentHP <= 0)
                OnDeath();
        }

        protected override void OnDeath()
        {
            DebugManager.Log($"Enemy {StructData.EnemyId} died!", DebugManager.EDebugLevel.Dev, "Fight");
            OnDied?.Invoke(this);
            // Loot/XP Events feuern etc.
        }
    }
}
