using UnityEngine;

namespace SimpleSpyGame
{
    [CreateAssetMenu(fileName = "NewEnemyAISettings", menuName = "FightToTheLast/EnemyAISettings")]
    public class EnemyAISettings : ScriptableObject
    {
        [field: SerializeField] public virtual float PatrolSpeed { get; protected set; } = 2f;
        [field: SerializeField] public virtual float ChaseSpeed { get; protected set; } = 5f;
        [Tooltip("The enemy will keep chasing the target when said target is within this distance of the enemy")]
        [field: SerializeField] public virtual float ChaseRange { get; protected set; } = 3f;
        [field: SerializeField] public virtual float VisionRange { get; protected set; } = 5f;
        [field: SerializeField] public virtual float VisionAngle { get; protected set; } = 90f;

        [Tooltip("How long this spends pausing upon reaching a waypoint")]
        [field: SerializeField] public virtual float PatrolPauseDur { get; protected set; } = 1f;
        [Tooltip("How long the AI spends turning right before moving to the next point in the path")]
        [field: SerializeField] public virtual float PatrolTurnDur { get; protected set; } = 0.5f;
        [field: SerializeField] public virtual float WaypointStoppingDistance { get; protected set; } = 0.01f;
        [field: SerializeField] public virtual float ChaseStoppingDistance { get; protected set; } = 1.03f;
        [field: SerializeField] public virtual LayerMask TargetLayers { get; protected set; } = ~0;
        [field: SerializeField] public virtual LayerMask ObstacleLayers { get; protected set; } = ~0;
        
    }
}