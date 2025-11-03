using CHAL.Data;
using CHAL.Systems.Unit;
using System;


namespace CHAL.Systems.Enemy
{
/// <summary>
/// Represents an instance of an enemy in the game, inheriting from EffectReceiver.
/// This class serves as a base for enemy effects and behaviors.
/// </summary>
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

/// <summary>
/// Applies damage to the enemy and updates its health.
/// </summary>
/// <param name="amount">The amount of damage to apply.</param>
/// <param name="type">The type of damage being inflicted.</param>
        public override void TakeDamage(float amount, DamageType type)
        {
            // TODO: Armor/Resist später berücksichtigen
            CurrentHP -= amount;

            DebugManager.Log($"Enemy {StructData.EnemyId} took {amount} {type} damage (HP={CurrentHP}/{MaxHP})",
                DebugManager.EDebugLevel.Dev, "Combat");

            if (CurrentHP <= 0)
                OnDeath();
        }

        protected override void OnDeath()
        {
            DebugManager.Log($"Enemy {StructData.EnemyId} died!", DebugManager.EDebugLevel.Dev, "Combat");
            OnDied?.Invoke(this);
            // Loot/XP Events feuern etc.
        }
    }
}
