using CGT.PlayerMoveController;
using UnityEngine;

namespace CGT.CharacterControls
{
    public class UBCCJump : OrderableBehaviour
    {
        [SerializeField] protected InputReader inputReader;
        [SerializeField] protected CharacterController charaController;
        [SerializeField] protected UBCCMovementApplier applier;
        [SerializeField] protected float jumpForce = 8f;
        [SerializeField] protected float cooldownTime = 0.25f;

        protected virtual void OnEnable()
        {
            inputReader.JumpStarted += OnJumpStarted;
            inputReader.JumpCancelled += OnJumpCancelled;
        }

        protected virtual void OnJumpStarted()
        {
            isJumpPressed = true;
            Debug.Log($"Jump started");
        }

        protected bool isJumpPressed = false;

        protected virtual void OnJumpCancelled()
        {
            isJumpPressed = false;
            Debug.Log($"Jump cancelled");
        }

        protected virtual void OnDisable()
        {
            inputReader.JumpStarted -= OnJumpStarted;
            inputReader.JumpCancelled -= OnJumpCancelled;
        }

        public override void OnUpdate()
        {
            if (!_onCooldown && charaController.isGrounded && isJumpPressed)
            {
                applier.YMovement = jumpForce;
                _onCooldown = true;
                Debug.Log($"Applied jump");
                Invoke(nameof(EndCooldown), cooldownTime);
            }
        }

        protected bool _onCooldown;

        protected virtual void EndCooldown()
        {
            Debug.Log($"Jump off cooldown");
            _onCooldown = false;
        }
    }
}