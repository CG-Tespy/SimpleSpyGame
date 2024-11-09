using UnityEngine;

namespace CGT.PlayerMoveController
{
    public class ThirdPersonRotation : OrderableBehaviour
    {
        [SerializeField] protected InputReader inputReader;
        [SerializeField] protected Transform toRotate;
        [SerializeField] protected float rotationSpeed = 10f;

        public override void OnFixedUpdate()
        {
            base.OnUpdate();
            if (!ThereIsMovementInputThisFrame)
            {
                return;
            }

            CheckWhatDirectionWeAreMoving();
            RotateAsAppropriate();
        }

        protected virtual void OnEnable()
        {
            inputReader.MovePerformed += OnMovePerformed;
        }

        protected virtual void OnMovePerformed(Vector2 moveInputs)
        {
            inputHorizontal = moveInputs.x;
            inputVertical = moveInputs.y;

            // For some reason, doing the rotation during each frame this func executes just doesn't work.
            // Hence the need to do it during OnUpdate.

            //CheckWhatDirectionWeAreMoving();
            //RotateAsAppropriate();
        }

        protected float inputHorizontal, inputVertical;

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
            forward *= inputVertical;
            right *= inputHorizontal;
        }

        protected Vector3 forward;
        protected virtual Transform MainCamTrans { get { return Camera.main.transform; } }
        protected Vector3 right;

        protected virtual void RotateAsAppropriate()
        {
            bool shouldApplyRotation = ThereIsMovementInputThisFrame;
            if (shouldApplyRotation)
            {
                float angle = Mathf.Atan2(forward.x + right.x,
                    forward.z + right.z) * Mathf.Rad2Deg;

                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                toRotate.rotation = Quaternion.Slerp(toRotate.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }

        protected virtual bool ThereIsMovementInputThisFrame
        {
            get { return inputHorizontal != 0 || inputVertical != 0; }
        }

        protected virtual void OnDisable()
        {
            inputReader.MovePerformed -= OnMovePerformed;
        }

    }
}