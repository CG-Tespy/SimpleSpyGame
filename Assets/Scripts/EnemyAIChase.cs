using CGT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightToTheLast
{
    public class EnemyAIChase : EnemyAIState
    {
        [SerializeField] protected State _onTargetLost;

        public override void Enter(IState enteringFrom = null)
        {
            base.Enter(enteringFrom);
            _navAgent.isStopped = false;
        }

        public override void ExecUpdate()
        {
            base.ExecUpdate();

            bool targetWithinRange = Vector3.Distance(TargetPos, AgentPos) < Settings.VisionRange;

            Vector3 towardsTarget = (TargetPos - AgentPos).normalized;
            bool targetViewObstructed = Physics.Raycast(AgentPos, towardsTarget,
                Settings.VisionRange, Settings.ObstacleLayers);

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
            _navAgent.isStopped = true;
        }
    }
}