using UnityEngine.AI;
using UnityEngine;
using CGT;

namespace SimpleSpyGame
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
        protected Transform AgentTrans { get { return _navAgent.transform; } }
        protected Vector3 AgentPos { get { return AgentTrans.position; } }
        protected Transform SightOrigin
        {
            get
            {
                if (_controller == null)
                {
                    return null;
                }

                return _controller.SightOrigin;
            }
        }
        protected Transform Target { get { return _controller.Target; } }
        protected Vector3 TargetPos { get { return Target.position; } }
    }
}