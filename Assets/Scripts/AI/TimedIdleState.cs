using CGT;
using UnityEngine;

namespace FightToTheLast
{
    public class TimedIdleState : EnemyAIState
    {
        [SerializeField] protected State _onTimeUp;

        public override void Enter(IState enteringFrom = null)
        {
            base.Enter(enteringFrom);
            Invoke(nameof(MoveToNextState), Settings.PatrolPauseDur);
        }

        protected virtual void MoveToNextState()
        {
            if (_onTimeUp != null)
            {
                TransitionTo(_onTimeUp);
            }
        }
    }
}