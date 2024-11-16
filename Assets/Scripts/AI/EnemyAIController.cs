using UnityEngine;

namespace FightToTheLast
{
    public class EnemyAIController : MonoBehaviour
    {
        [field: SerializeField] public virtual EnemyAISettings AISettings { get; protected set; }
        [field: SerializeField] public virtual Transform SightOrigin { get; protected set; }

        public virtual Transform Target { get; set; }
    }
}