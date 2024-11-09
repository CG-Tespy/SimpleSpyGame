using UnityEngine;

namespace CGT.CharacterControls
{
    public enum MotionSpeedVal
    {
        Null, 
        Walk,
        Run,
        Sprint,
        Air,
        CrouchWalk
    }

    public class MotionOnGround : MotionState
    {
        [SerializeField] protected MotionSpeedVal _speed;
        [SerializeField] protected MotionState _onMoveStop;
        [SerializeField] protected MotionState _onMoveToggle;

        protected override float MoveSpeed
        {
            get
            {
                float result = 0f;

                switch (_speed)
                {
                    case MotionSpeedVal.Walk:
                        result = MotionSpeeds.WalkSpeed;
                        break;
                    case MotionSpeedVal.Run:
                        result = MotionSpeeds.RunSpeed;
                        break;
                    case MotionSpeedVal.Sprint:
                        result = MotionSpeeds.SprintSpeed;
                        break;
                    case MotionSpeedVal.Air:
                        result = MotionSpeeds.AirSpeed;
                        break;
                    case MotionSpeedVal.CrouchWalk:
                        result = MotionSpeeds.CrouchWalkSpeed;
                        break;
                    default:
                        Debug.LogError($"Did not account for MotionSpeedVal {_speed}");
                        break;
                }

                return result;
            }
        }

        public override void ExecFixedUpdate()
        {
            base.ExecFixedUpdate();
            SetGroundMovement();
            if (_inputReader.CurrentMovementInput == Vector2.zero && _onMoveStop != null)
            {
                Debug.Log($"State {this.name} transitioning to {_onMoveStop.name} in response to input being zero");
                TransitionTo(_onMoveStop);
            }
        }

        protected virtual void SetGroundMovement()
        {
            Vector3 forward = _steering.ForwardSteer * MoveSpeed,
                right = _steering.RightSteer * MoveSpeed;
            Vector3 movement = forward + right;
            _moveApplier.XMovement = movement.x;
            _moveApplier.ZMovement = movement.z;
        }

        protected override void OnMoveToggleStarted()
        {
            base.OnMoveToggleStarted();
            if (_onMoveToggle != null)
            {
                Debug.Log($"State {this.name} transitioning to {_onMoveToggle.name} in response to move toggle");
                TransitionTo(_onMoveToggle);
            }
        }
    }
}