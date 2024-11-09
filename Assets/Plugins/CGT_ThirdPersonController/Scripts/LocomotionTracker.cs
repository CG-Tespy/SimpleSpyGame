using UnityEngine;

namespace CGT.CharacterControls
{
    /// <summary>
    /// A hub of sorts for the locomotion-executors.
    /// </summary>
    public class LocomotionTracker : MonoBehaviour
    {
        [field: SerializeField] public virtual MotionSpeeds MotionSpeeds { get; protected set; }

        protected virtual void Awake()
        {
            _inputReader = GetComponentInParent<IMovementInputReader>();
        }

        protected IMovementInputReader _inputReader;

        public virtual Vector3 CurrentInput { get { return _inputReader.CurrentMovementInput; } }

        protected virtual void Update()
        {
            UpdateRelativeMovementVals();
        }

        protected virtual void UpdateRelativeMovementVals()
        {
            // Since we might want to move relative to where the cam's facing
            _forward = MainCamTrans.forward;
            _right = MainCamTrans.right;

            // To avoid unintended vertical movement
            _forward.y = 0;
            _right.y = 0;

            _forward.Normalize();
            _right.Normalize();

            // Relate the front with the Z direction (depth) and _right with X (lateral movement)
            _forward *= CurrentInput.y;
            _right *= CurrentInput.x;
        }

        public virtual Vector3 ForwardSteer
        {
            get { return _forward; }
        }
        protected Vector3 _forward;
        public virtual Vector3 RightSteer
        {
            get { return _right; }
        }
        protected Vector3 _right;

        protected virtual Transform MainCamTrans { get { return Camera.main.transform; } }

    }
}