using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGT.CharacterControls
{
    public class IdleState : MotionState
    {
        [SerializeField] protected MotionState _onMoveStart;

        public override void ExecUpdate()
        {
            base.ExecUpdate();
            if (_onMoveStart != null && CurrentMovementInput != Vector2.zero)
            {
                this.Exit();
                _onMoveStart.Enter();
            }
        }

        protected override float MoveSpeed { get; } = 0;
    }
}