using CGT.CharacterControls;
using UnityEngine;

namespace CGT.PlayerMoveController
{
    public class UBCCMoveSpeed : OrderableBehaviour
    {
        [SerializeField] protected CharacterController charaController;
        [SerializeField] protected UBCCLocomotion locomotion;
        [SerializeField] protected float runSpeed = 5f;
        [SerializeField] protected float sprintSpeed = 8f;
        [SerializeField] protected float airSpeed = 5f;

        [Tooltip("Whether or not this applies ground speed")]
        [field: SerializeField] public bool AllowRun { get; set; } = true;

        [Tooltip("Whether or not this applies sprint speed")]
        [field: SerializeField] public bool AllowSprint { get; set; } = true;

        [Tooltip("Whether or not this applies air speed")]
        [field: SerializeField] public bool AllowAirMovement { get; set; } = true;

        public override void OnUpdate()
        {
            base.OnUpdate();
            locomotion.MoveSpeed = 0; // To allow better control of the speed
            HandleRunSpeed();
            HandleSprintSpeed();
            HandleAirSpeed();
        }

        protected virtual void HandleRunSpeed()
        {
            if (!charaController.isGrounded || !AllowRun)
            {
                return;
            }

            locomotion.MoveSpeed = runSpeed;
        }

        protected virtual void HandleSprintSpeed()
        {
            if (!charaController.isGrounded || !AllowSprint || !locomotion.IsSprinting)
            {
                return;
            }

            locomotion.MoveSpeed = sprintSpeed;
        }

        protected virtual void HandleAirSpeed()
        {
            if (charaController.isGrounded || !AllowAirMovement)
            {
                return;
            }

            locomotion.MoveSpeed = airSpeed;
        }

    }
}