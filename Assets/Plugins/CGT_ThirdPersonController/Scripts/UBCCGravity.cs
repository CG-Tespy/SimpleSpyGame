using UnityEngine;

namespace CGT.CharacterControls
{
    public class UBCCGravity : OrderableBehaviour
    {
        [SerializeField] protected CharacterController charaController;
        [SerializeField] protected UBCCMovementApplier applier;
        [SerializeField] protected UBCCJump jumpFunctionality;
        [SerializeField] protected float aerialGravity = 9.8f;
        [SerializeField] protected float groundedGravity = 8f;

        public override void StartInit()
        {
            base.StartInit();
        }

        public override void OnFixedUpdate()
        {
            HandleGroundGravity();
            HandleAerialGravity();
        }

        protected virtual void HandleGroundGravity()
        {
            if (charaController.isGrounded)
            {
                float newYMovement = applier.YMovement - (groundedGravity * Time.deltaTime);
                float withForceCappedAtGravity = Mathf.Max(newYMovement, -groundedGravity);
                applier.YMovement = withForceCappedAtGravity;
            }
        }

        protected virtual void HandleAerialGravity()
        {
            if (!charaController.isGrounded)
            {
                bool isFalling = applier.YMovement <= 0;
                bool isRising = applier.YMovement > 0;

                if (isFalling)
                {
                    applier.YMovement = -aerialGravity;
                }
                else if (isRising)
                {
                    // We want to slow down (rather than reverse) the ascent, hence us
                    // using deltaTime here instead of setting an unscaled value.
                    float currentAscent = applier.YMovement;
                    float slowedAscent = currentAscent - (aerialGravity * Time.deltaTime);
                    applier.YMovement = slowedAscent;
                }
            }
        }

        protected Vector3 directionOfGravity = Vector3.down;
    }
}