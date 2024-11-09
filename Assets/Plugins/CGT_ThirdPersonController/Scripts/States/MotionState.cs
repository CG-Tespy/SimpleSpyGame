using CGT.PlayerMoveController;
using UnityEngine;

namespace CGT.CharacterControls
{
    public abstract class MotionState : State
    {
        public override void Init()
        {
            base.Init();
            _charaController = GetComponentInParent<CharacterController>();
            _steering = GetComponentInParent<LocomotionTracker>();
            _moveApplier = GetComponentInParent<UBCCMovementApplier>();
            _inputReader = GetComponentInParent<IMovementInputReader>();
        }

        protected CharacterController _charaController;
        protected LocomotionTracker _steering;
        protected UBCCMovementApplier _moveApplier;
        protected IMovementInputReader _inputReader;

        public override void Enter()
        {
            base.Enter();
            ListenForInput();
        }

        protected virtual void ListenForInput()
        {
            // Toggling the movement isn't exactly essential, so we don't need to 
            // make a fuss about the toggle action being unset.
            _inputReader.JumpStart += OnJumpInputStarted;
            _inputReader.JumpCancel += OnJumpInputCanceled;

            _inputReader.MoveToggleStart += OnMoveToggleStarted;
            _inputReader.CrouchToggleStart += OnCrouchInputStarted;
        }

        protected Vector2 CurrentMovementInput { get { return _inputReader.CurrentMovementInput; } }

        protected virtual void OnMoveToggleStarted()
        {

        }

        protected virtual void OnJumpInputStarted()
        {

        }

        // For when you want to adjust jump height based on how long the
        // player has the jump input pressed
        protected virtual void OnJumpInputCanceled()
        {

        }

        protected virtual void OnCrouchInputStarted()
        {

        }

        public override void Exit()
        {
            base.Exit();
            UNlistenForInput();
        }

        protected virtual void UNlistenForInput()
        {
            _inputReader.JumpStart -= OnJumpInputStarted;
            _inputReader.JumpCancel -= OnJumpInputCanceled;

            _inputReader.MoveToggleStart -= OnMoveToggleStarted;
            _inputReader.CrouchToggleStart -= OnCrouchInputStarted;
        }

        protected virtual MotionSpeeds MotionSpeeds { get { return _steering.MotionSpeeds; } }
        protected abstract float MoveSpeed { get; }
        
    }

}