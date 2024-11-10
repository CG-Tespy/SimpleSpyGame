using UnityEngine;

namespace FightToTheLast
{
    [CreateAssetMenu(fileName = "NewEnemyAISettings", menuName = "FightToTheLast/EnemyAISettings")]
    public class EnemyAISettings : ScriptableObject
    {
        [field: SerializeField] public virtual float PatrolSpeed { get; protected set; } = 2f;
        [field: SerializeField] public virtual float ChaseSpeed { get; protected set; } = 5f;
        [field: SerializeField] public virtual float VisionRange { get; protected set; } = 5f;

    }
}