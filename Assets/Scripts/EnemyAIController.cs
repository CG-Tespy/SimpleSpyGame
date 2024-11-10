using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightToTheLast
{
    public class EnemyAIController : MonoBehaviour
    {
        [field: SerializeField] public virtual EnemyAISettings AISettings { get; protected set; }
    }
}