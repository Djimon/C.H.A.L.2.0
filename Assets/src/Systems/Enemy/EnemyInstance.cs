using CHAL.Data;
using CHAL.Systems.Skill;
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
/// Applies damage to the enemy, considering the damage type.
/// Logs the damage received and the resulting health.
/// </summary>
/// <param name="amount">The amount of damage to apply.</param>
/// <param name="type">The type of damage being applied.</param>
        public override void TakeDamage(float amount, DamageType type)
        {
            if (amount <= 0f) return;

            DebugManager.Log(
                $"Enemy {StructData.EnemyId} incoming {amount} {type} damage (before defenses)",
                DebugManager.EDebugLevel.Dev,
                "Combat"
            );

            // nutzt jetzt den zentralen Wrapper in EffectReceiver
            base.TakeDamage(amount, type);

            DebugManager.Log(
                $"Enemy {StructData.EnemyId} HP after damage: {CurrentHP}/{MaxHP}",
                DebugManager.EDebugLevel.Dev,
                "Combat"
            );
        }

        protected override void OnDeath()
        {
            DebugManager.Log($"Enemy {StructData.EnemyId} died!", DebugManager.EDebugLevel.Dev, "Combat");
            OnDied?.Invoke(this);
            // Loot/XP Events feuern etc.
        }

        public override float GetBaseDamage()
        {
            return Definition.baseDamage;
        }
    }
}
