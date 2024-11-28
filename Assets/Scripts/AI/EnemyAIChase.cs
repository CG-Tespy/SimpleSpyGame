using CGT;
using UnityEngine;

namespace SimpleSpyGame
{
    public class EnemyAIChase : EnemyAIState
    {
        [SerializeField] protected State _onTargetLost;
        [SerializeField] protected float _delayBetweenUpdates = 0.1f;

        public override void Enter(IState enteringFrom = null)
        {
            base.Enter(enteringFrom);
            _navAgent.isStopped = false;
            _navAgent.stoppingDistance = Settings.ChaseStoppingDistance;
            _navAgent.speed = Settings.ChaseSpeed;
            InvokeRepeating(nameof(HandleChase), 0f, _delayBetweenUpdates);
        }

        public override void ExecUpdate()
        {
            base.ExecUpdate();
            DrawDebugStuff();

        }

        protected virtual void HandleChase()
        {
            float distanceFromTarget = Vector3.Distance(TargetPos, AgentPos);
            //Debug.Log($"Chase state distance from target: {distanceFromTarget}");
            bool targetWithinRange = distanceFromTarget < Settings.VisionRange;

            Vector3 towardsTarget = (TargetPos - AgentPos).normalized;

            RaycastHit hit;
            bool targetViewObstructed = Physics.Raycast(AgentPos, towardsTarget, out hit, distanceFromTarget, Settings.ObstacleLayers);

            if (hit.collider != null)
            {
                Debug.Log($"Obstacles obstructing the view of {_controller.name} is {hit.transform.name}");
            }

            bool weCanSeeTheTarget = targetWithinRange && !targetViewObstructed;
            if (weCanSeeTheTarget)
            {
                _navAgent.SetDestination(TargetPos);
            }
            else
            {
                _navAgent.isStopped = true;
                if (_onTargetLost != null)
                {
                    TransitionTo(_onTargetLost);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            CancelInvoke();
            _controller.Target = null;
            _navAgent.isStopped = true;
        }

        private void DrawDebugStuff()
        {
            if (SightOrigin != null)
            {
                Vector3 startPoint = SightOrigin.position;
                Vector3 endPoint = startPoint + (SightOrigin.forward * Settings.VisionRange);
                Debug.DrawLine(startPoint, endPoint, Color.blue);
            }
        }
    }
}