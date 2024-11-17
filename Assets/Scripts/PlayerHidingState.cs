using CGT;
using CGT.CharacterControls;
using UnityEngine;

namespace FightToTheLast
{
    public class PlayerHidingState : State
    {
        [SerializeField] protected bool _changeCollHeight = true;
        [Tooltip("The height of the player collider while hiding")]
        [SerializeField] protected float _hideCollHeight = 0.5f;

        [SerializeField] protected bool _changeCollCenter = true;
        [SerializeField] protected Vector3 _hideCollCenter = Vector3.zero;

        [Tooltip("Which state to enter when the right input makes this one exit")]
        [SerializeField] protected State _triggerOnExit;

        public override void Init()
        {
            base.Init();
            _charaController = GetComponentInParent<CharacterController>();
            _origCollHeight = _charaController.height;
            _origCollCenter = _charaController.center;
            _moveApplier = GetComponentInParent<UBCCMovementApplier>();
            _inputReader = GetComponentInParent<AltInputReader>();
        }

        protected CharacterController _charaController;
        protected float _origCollHeight;
        protected Vector3 _origCollCenter;
        protected UBCCMovementApplier _moveApplier;
        protected AltInputReader _inputReader;

        public override void Enter(IState enteringFrom = null)
        {
            base.Enter(enteringFrom);
            Debug.Log($"Entered hiding state");
            _inputReader.HideStart += OnHideInputStart;

            if (_changeCollHeight)
            {
                _charaController.height = _hideCollHeight;
            }

            if (_changeCollCenter)
            {
                _charaController.center = _hideCollCenter;
            }

            _moveApplier.XMovement = _moveApplier.ZMovement = 0;
        }

        protected virtual void OnHideInputStart()
        {
            if (_triggerOnExit != null)
            {
                TransitionTo(_triggerOnExit);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _inputReader.HideStart -= OnHideInputStart;

            if (_changeCollHeight)
            {
                _charaController.height = _origCollHeight;
            }

            if (_changeCollCenter)
            {
                _charaController.center = _origCollCenter;
            }
        }
    }
}