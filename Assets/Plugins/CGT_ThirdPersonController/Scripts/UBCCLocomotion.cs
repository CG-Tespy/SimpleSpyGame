using CGT.PlayerMoveController;
using UnityEngine;

namespace CGT.CharacterControls
{
    public class UBCCLocomotion : OrderableBehaviour
    {
        [SerializeField] protected InputReader inputReader;
        [SerializeField] protected UBCCMovementApplier applier;
        [SerializeField] protected CharacterController charaController;
        
        public virtual float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
        [SerializeField] protected float moveSpeed = 5f;
        public virtual float MinMoveSpeed { get { return minMoveSpeed; } }
        protected float minMoveSpeed = 0.9f;

        protected virtual void OnEnable()
        {
            inputReader.MovePerformed += OnMovePerformed;
            inputReader.MoveCancelled += OnMoveCancelled;
            inputReader.SprintStarted += OnSprintStarted;
        }

        protected virtual void OnMovePerformed(Vector2 moveInputs)
        {
            inputHorizontal = moveInputs.x;
            inputVertical = moveInputs.y;
        }

        protected float inputHorizontal, inputVertical;

        public override void OnUpdate()
        {
            base.OnUpdate();
            UpdateRelativeMovementVals();
        }

        // Movement on the axes
        public float XMovement
        {
            get { return inputHorizontal * moveSpeed; }
        }

        public float ZMovement
        {
            get { return inputVertical * moveSpeed; }
        }

        protected virtual void UpdateRelativeMovementVals()
        {
            // Since we want to move relative to where the cam's facing
            forward = MainCamTrans.forward;
            right = MainCamTrans.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            // Relate the front with the Z direction (depth) and right with X (lateral movement)
            forward *= ZMovement;
            right *= XMovement;
        }

        protected Vector3 forward, right;

        protected virtual Transform MainCamTrans { get { return Camera.main.transform; } }

        protected virtual void OnMoveCancelled()
        {
            inputHorizontal = inputVertical = 0;
            forward = right = Vector3.zero;
        }

        protected virtual void OnSprintStarted()
        {
            IsSprinting = !IsSprinting; 
            // ^We're handling this like a toggle as opposed to requiring the player to
            // hold the sprint key to keep sprinting
        }

        protected virtual void OnDisable()
        {
            inputReader.MovePerformed -= OnMovePerformed;
            inputReader.MoveCancelled -= OnMoveCancelled;
            inputReader.SprintStarted -= OnSprintStarted;
        }

        protected bool inputCrouch, inputSprint;

        public virtual bool IsCrouching { get; protected set; }

        public virtual bool IsSprinting { get; protected set; }

        public override void OnFixedUpdate()
        {
            SetGroundMovement();
        }

        protected virtual void SetGroundMovement()
        {
            Debug.Log($"Ground loco setting movement");
            Movement = forward + right;
            applier.XMovement = Movement.x;
            applier.ZMovement = Movement.z;
        }

        public virtual Vector3 Movement { get; protected set; }
    }
}