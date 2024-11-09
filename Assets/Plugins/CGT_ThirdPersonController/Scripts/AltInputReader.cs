using System;
using UnityEngine;
using InputInfo = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace CGT.CharacterControls
{
    // Keeps track of the inputs
    public class AltInputReader : MonoBehaviour, ThirdPersonPlayerInput.IOverworldActions, IMovementInputReader
    {
        protected virtual void Awake()
        {
            _controls = new ThirdPersonPlayerInput();
            _controls.Overworld.SetCallbacks(this);
        }

        protected ThirdPersonPlayerInput _controls;

        protected virtual void OnEnable()
        {
            _controls.Overworld.Enable();
        }

        protected virtual void OnDisable()
        {
            _controls.Overworld.Disable();
        }

        public virtual void OnJump(InputInfo context)
        {
            if (context.performed)
            {
                JumpStart();
            }
            else if (context.canceled)
            {
                JumpCancel();
            }
        }

        public event Action JumpStart = delegate { };
        public event Action JumpCancel = delegate { };
        
        public virtual void OnMove(InputInfo context)
        {
            CurrentMovementInput = context.ReadValue<Vector2>();
        }

        public Vector2 CurrentMovementInput { get; protected set; }

        public virtual void OnMoveToggle(InputInfo context)
        {
            // For toggling between walking, running and sprinting
            if (context.performed)
            {
                MoveToggleStart();
            }
        }

        public event Action MoveToggleStart = delegate { };

        public virtual void OnCrouchToggle(InputInfo context)
        {
            // Players usually won't want the crouch state to be mixed in with the toggle
            // for walking, running and sprinting. That's why we have crouching handled by
            // its own separate event
            if (context.performed)
            {
                CrouchToggleStart();
            }
        }

        public event Action CrouchToggleStart = delegate { };

        public virtual void OnLook(InputInfo context)
        {
            // Ignore this. We can just let Cinemachine worry about this input
        }
    }
}