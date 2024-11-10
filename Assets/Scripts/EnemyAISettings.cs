using UnityEngine;

namespace FightToTheLast
{
    [CreateAssetMenu(fileName = "NewEnemyAISettings", menuName = "FightToTheLast/EnemyAISettings")]
    public class EnemyAISettings : ScriptableObject
    {
        [field: SerializeField] public virtual float PatrolSpeed { get; protected set; } = 2f;
        [field: SerializeField] public virtual float ChaseSpeed { get; protected set; } = 5f;
        [field: SerializeField] public virtual float VisionRange { get; protected set; } = 5f;

        [Tooltip("How long this spends pausing upon reaching a waypoint")]
        [field: SerializeField] public virtual float PatrolPauseDur { get; protected set; } = 1f;
        [Tooltip("How long the AI spends turning right before moving to the next point in the path")]
        [field: SerializeField] public virtual float PatrolTurnDur { get; protected set; } = 0.5f;

    }
}