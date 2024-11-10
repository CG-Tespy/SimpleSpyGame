using UnityEngine.AI;
using CGT;

namespace FightToTheLast
{
    public class EnemyAIState : State
    {
        public override void Init()
        {
            base.Init();
            _navAgent = GetComponentInParent<NavMeshAgent>();
            _controller = GetComponentInParent<EnemyAIController>();

        }

        protected NavMeshAgent _navAgent;
        protected EnemyAIController _controller;
        protected EnemyAISettings Settings { get { return _controller.AISettings; } }
    }
}