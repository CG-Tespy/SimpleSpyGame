using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using InputInfo = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace CGT.PlayerMoveController
{
    public class InputReader : MonoBehaviour, ThirdPersonPlayerInput.IOverworldActions
    {
        protected virtual void Awake()
        {
            controls = new ThirdPersonPlayerInput();
        }

        protected ThirdPersonPlayerInput controls;

        protected virtual void OnEnable()
        {
            controls.Enable();

            PlayerMove.started += OnMoveInput;
            PlayerMove.performed += OnMovePerformed;
            PlayerMove.canceled += OnMoveCancelled;

            // We're treating sprinting as a toggle of sorts. Different from
            // changing the regular speed, as we expect things other than pressing
            // the button again to undo the sprint

            PlayerJump.started += OnJumpStarted;
            PlayerJump.canceled += OnJumpCancelled;
        }

        protected virtual InputAction PlayerMove
        {
            get { return controls.Overworld.Move; }
        }

        protected virtual void OnMoveInput(InputInfo info)
        {
            CurrentMovementInput = info.ReadValue<Vector2>();

            if (CurrentMovementInput != prevMovementInput)
            {
                MovementInputChanged(CurrentMovementInput);
            }

            prevMovementInput = CurrentMovementInput;
        }

        protected virtual void OnMovePerformed(InputInfo info)
        {
            OnMoveInput(info);
            MovePerformed(CurrentMovementInput);
        }

        public Vector2 CurrentMovementInput { get; protected set; }
        protected Vector2 prevMovementInput;

        public event UnityAction<Vector2> MovementInputChanged = delegate { };
        public event UnityAction<Vector2> MovePerformed = delegate { };

        protected virtual void OnMoveCancelled(InputInfo info)
        {
            OnMoveInput(info);
            MoveCancelled();
        }

        public event UnityAction MoveCancelled = delegate { };

        public virtual bool IsMovementPressed
        {
            get { return CurrentMovementInput.x != 0 || CurrentMovementInput.y != 0; }
        }

        protected virtual void OnSprintStarted(InputInfo info)
        {
            SprintStarted();
        }

        public event UnityAction SprintStarted = delegate { };

        protected virtual void OnToggleNormalMovement(InputInfo info)
        {
            NormalMovementToggleTriggered();
        }

        public event UnityAction NormalMovementToggleTriggered = delegate { };

        protected InputAction PlayerJump { get { return controls.Overworld.Jump; } }

        protected virtual void OnJumpInput(InputInfo info)
        {
            CurrentIsJumpPressed = info.ReadValueAsButton();

            if (CurrentIsJumpPressed != prevIsJumpPressed)
            {
                JumpInputChanged(CurrentIsJumpPressed);
            }

            prevIsJumpPressed = CurrentIsJumpPressed;
        }

        public bool CurrentIsJumpPressed { get; protected set; }
        protected bool prevIsJumpPressed;

        public event UnityAction<bool> JumpInputChanged = delegate { };

        protected virtual void OnJumpStarted(InputInfo info)
        {
            JumpStarted();
        }

        public event UnityAction JumpStarted = delegate { };

        protected virtual void OnJumpCancelled(InputInfo info)
        {
            JumpCancelled();
        }

        public event UnityAction JumpCancelled = delegate { };

        protected virtual void OnDisable()
        {
            controls.Disable();

            PlayerMove.started -= OnMoveInput;
            PlayerMove.performed -= OnMovePerformed;
            PlayerMove.canceled -= OnMoveCancelled;

            // We're treating sprinting as a toggle of sorts. Different from
            // changing the regular speed, as we expect things other than pressing
            // the button again to undo the sprint

            PlayerJump.started -= OnJumpStarted;
            PlayerJump.canceled -= OnJumpCancelled;
        }

        public virtual void OnLook(InputInfo info)
        {

        }

        public void OnMove(InputInfo context)
        {

        }

        public void OnSprint(InputInfo context)
        {

        }

        public void OnJump(InputInfo context)
        {
            throw new System.NotImplementedException();
        }

        public void OnMoveToggle(InputInfo context)
        {
            throw new System.NotImplementedException();
        }

        public void OnCrouch(InputInfo context)
        {
            throw new System.NotImplementedException();
        }

        public void OnCrouchToggle(InputInfo context)
        {
            throw new System.NotImplementedException();
        }

        public void OnHide(InputInfo context)
        {
            throw new System.NotImplementedException();
        }

        public void OnCancelHide(InputInfo context)
        {
            throw new System.NotImplementedException();
        }

        public void OnThirdEye(InputInfo context)
        {
            throw new System.NotImplementedException();
        }
    }
}