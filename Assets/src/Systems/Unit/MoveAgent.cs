using UnityEngine;

using UnityEngine.AI;

namespace CHAL.Systems.AI
{


    /// <summary>
    /// Wrapper around NavMeshAgent.
    /// Handles movement and stoppping with Speed/StoppingDistance.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class MoveAgent : MonoBehaviour
    {
        [Header("Runtime (read-only)")]
        [SerializeField] private NavMeshAgent _agent;

        [Header("Tuning")]
        [SerializeField] private float _destinationEpsilon = 0.05f; // „Signifikanz“, um SetDestination-Spam zu vermeiden

        public float BaseSpeed { get; private set; } = 3.5f;
        public float CurrentSpeed { get; private set; } = 3.5f;
        public float StoppingDistance
        {
            get => _agent != null ? _agent.stoppingDistance : _stoppingDistanceCache;
            set
            {
                _stoppingDistanceCache = value;
                if (_agent != null) _agent.stoppingDistance = Mathf.Max(0f, value);
            }
        }

        private float _stoppingDistanceCache = 0f;

        private void Awake()
        {
            if (_agent == null) _agent = GetComponent<NavMeshAgent>();
            // Default-Setup – wird bei Init ggf. überschrieben
            if (_agent != null)
            {
                _agent.updateRotation = true;
                _agent.updateUpAxis = true;
                _agent.autoBraking = false; // reduziert Stottern
                _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            }
        }

        public void Init(float baseSpeed, bool isHero, float radius = 0.35f, int? overridePriority = null)
        {
            BaseSpeed = Mathf.Max(0.1f, baseSpeed);
            CurrentSpeed = BaseSpeed;

            if (_agent == null) _agent = GetComponent<NavMeshAgent>();

            _agent.radius = Mathf.Max(0.05f, radius);
            _agent.speed = CurrentSpeed;
            _agent.acceleration = Mathf.Max(4f, baseSpeed * 4f);
            _agent.angularSpeed = 720f;
            _agent.autoBraking = false;
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            _agent.avoidancePriority = overridePriority ?? (isHero ? 10 : 50);
            _agent.stoppingDistance = _stoppingDistanceCache;
        }

        /// <summary>
        /// Get effecitve movement speed.
        /// </summary>
        public void ApplyRuntimeSpeed(float speedMultiplier)
        {
            speedMultiplier = Mathf.Max(0f, speedMultiplier);
            CurrentSpeed = BaseSpeed * speedMultiplier;
            if (_agent != null) _agent.speed = CurrentSpeed;
        }

        /// <summary>
        /// Set destination
        /// </summary>
        public void SetDestination(Vector3 worldPos)
        {
            if (_agent == null) return;
            // Nur neu setzen, wenn sich das Ziel „merklich“ ändert (verhindert Spam & teures Replan)
            if (_agent.hasPath)
            {
                Vector3 diff = _agent.destination - worldPos;
                if (diff.sqrMagnitude <= _destinationEpsilon * _destinationEpsilon) return;
            }
            _agent.isStopped = false;
            _agent.SetDestination(worldPos);
        }

        /// <summary>
        /// Stand still
        /// </summary>
        public void StopOrHold()
        {
            if (_agent == null) return;
            // isStopped verhindert „Zucken“ am Ziel
            _agent.isStopped = true;
            // Kein ResetPath: wir wollen, dass er die lokale Position hält
        }

        /// <summary>
        /// Reached the destination?
        /// </summary>
        public bool IsInStoppingRange(Vector3 targetPos)
        {
            if (_agent == null) return true;

            // Wenn kein gültiger Pfad vorliegt/noch berechnet wird, fallback auf Distanz
            if (_agent.pathPending) return Vector3.SqrMagnitude(targetPos - transform.position) <= (StoppingDistance * StoppingDistance);

            if (!_agent.hasPath)
            {
                // Kein Pfad -> Distanz prüfen
                return Vector3.SqrMagnitude(targetPos - transform.position) <= (StoppingDistance * StoppingDistance);
            }

            // remainingDistance ist zuverlässiger, wenn Pfad existiert
            if (_agent.remainingDistance <= Mathf.Max(StoppingDistance, 0.01f))
                return true;

            return false;
        }
    }
}