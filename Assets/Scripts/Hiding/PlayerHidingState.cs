using CGT;
using CGT.CharacterControls;
using UnityEngine;

namespace SimpleSpyGame
{
    public class PlayerHidingState : State
    {
        [SerializeField] protected bool _changeCollHeight = true;
        [Tooltip("The height of the player collider while hiding")]
        [SerializeField] protected float _hideCollHeight = 0.5f;

        [SerializeField] protected bool _changeCollCenter = true;
        [SerializeField] protected Vector3 _hideCollCenter = Vector3.zero;

        public override void Init()
        {
            base.Init();
            _charaController = GetComponentInParent<CharacterController>();
            _origCollHeight = _charaController.height;
            _origCollCenter = _charaController.center;
            _moveApplier = GetComponentInParent<UBCCMovementApplier>();
        }

        protected CharacterController _charaController;
        protected float _origCollHeight;
        protected Vector3 _origCollCenter;
        protected UBCCMovementApplier _moveApplier;

        public override void Enter(IState enteringFrom = null)
        {
            base.Enter(enteringFrom);
            Debug.Log($"Entered hiding state");

            if (_changeCollHeight)
            {
                _charaController.height = _hideCollHeight;
            }

            if (_changeCollCenter)
            {
                _charaController.center = _hideCollCenter;
            }

            _moveApplier.XMovement = _moveApplier.ZMovement = 0;
            // ^ To stop any ground movement
        }

        public override void Exit()
        {
            base.Exit();

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