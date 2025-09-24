using CHAL.Data;
using CHAL.Systems.Loot;
using CHAL.Systems.Wave;
using UnityEngine;

namespace CHAL.Systems.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyInstance EnemyData { get; private set; }

        public void Init(EnemyInstance instance)
        {
            EnemyData = instance;
        }

        private void Attack()
        {
            DebugManager.Log("Attack", DebugManager.EDebugLevel.Dev, "Fight");
        }

        private void OnMouseDown()
        {
            Die();
        }

        private void Die()
        {
            DebugManager.Log($"Enemy {EnemyData.EnemyId} ({EnemyData.Rank}) - [{EnemyData.Tags}] killed!", DebugManager.EDebugLevel.Dev, "Fight");

            // Event feuern: sagt nur „ich bin tot“, inkl. Position
            OnEnemyKilled?.Invoke(this, EnemyData, transform.position);

            Destroy(gameObject);
        }

        // Static Event für alle EnemyController
        public static event System.Action<EnemyController, EnemyInstance, Vector3> OnEnemyKilled;
    }
}

