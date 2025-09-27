using CHAL.Data;
using CHAL.Systems.Loot;
using CHAL.Systems.Wave;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyStruct EnemyData { get; private set; }
        public EnemyInstance EnemyInstance { get; private set; }


        public void Init(EnemyStruct enemstruct)
        {
            var def = UnitRegistry.Instance.GetEnemyByID(enemstruct.EnemyId);
            if (def == null)
            {
                DebugManager.Error($"EnemyDef '{enemstruct.EnemyId}' not found!");
                return;
            }

            EnemyInstance = new EnemyInstance(def, enemstruct);
            EnemyData = enemstruct;

            EnemyInstance.OnDied += OnEnemyInstanceDied;
        }


        private void OnEnemyInstanceDied(EnemyInstance instance)
        {
            Die();
        }

        private void Update()
        {
            EnemyInstance.UpdateEffects(Time.deltaTime);
        }

        private void Attack()
        {
            DebugManager.Log("Attack", DebugManager.EDebugLevel.Dev, "Fight");
        }

        private void OnMouseDown()
        {
            EnemyInstance.TakeDamage(999, DamageType.Physical);
        }

        private void Die()
        {
            DebugManager.Log($"Enemy {EnemyData.EnemyId} ({EnemyData.Rank}) - [{EnemyData.bonusTags}] killed!", DebugManager.EDebugLevel.Dev, "Fight");

            // Event feuern: sagt nur „ich bin tot“, inkl. Position
            OnEnemyKilled?.Invoke(this, EnemyData, transform.position);

            Destroy(gameObject);
        }

        // Static Event für alle EnemyController
        public static event System.Action<EnemyController, EnemyStruct, Vector3> OnEnemyKilled;
    }
}

