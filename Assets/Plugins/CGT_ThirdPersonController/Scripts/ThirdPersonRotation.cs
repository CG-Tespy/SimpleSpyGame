using CGT.CharacterControls;
using UnityEngine;

namespace CGT.PlayerMoveController
{
    public class ThirdPersonRotation : OrderableBehaviour
    {
        
        [SerializeField] protected AltInputReader inputReader;
        [SerializeField] protected Transform toRotate;
        [SerializeField] protected float rotationSpeed = 10f;

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (!ThereIsMovementInputThisFrame || !this.isActiveAndEnabled)
            {
                return;
            }

            CheckWhatDirectionWeAreMoving();
            RotateAsAppropriate();
        }

        protected float InputHorizontal { get { return inputReader.CurrentMovementInput.x; } }
        protected float InputVertical { get { return inputReader.CurrentMovementInput.y; } }

        protected virtual void CheckWhatDirectionWeAreMoving()
        {
            forward = MainCamTrans.forward;
            right = MainCamTrans.right;

            // We don't want to make the character do a flip, so...
            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            // We need the forward and horiz movement to be scaled relative to the movement
            // on the corresponding axes
            forward *= InputVertical;
            right *= InputHorizontal;
        }

        protected Vector3 forward;
        protected virtual Transform MainCamTrans { get { return Camera.main.transform; } }
        protected Vector3 right;

        protected virtual void RotateAsAppropriate()
        {
            bool shouldApplyRotation = ThereIsMovementInputThisFrame;
            if (shouldApplyRotation)
            {
                Debug.Log($"Applying rotation!");
                float angle = Mathf.Atan2(forward.x + right.x,
                    forward.z + right.z) * Mathf.Rad2Deg;

                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                toRotate.rotation = Quaternion.Slerp(toRotate.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }

        protected virtual bool ThereIsMovementInputThisFrame
        {
            get
            {
                bool result = inputReader.CurrentMovementInput.magnitude > 0;
                return result;
            }
        }


    }
}