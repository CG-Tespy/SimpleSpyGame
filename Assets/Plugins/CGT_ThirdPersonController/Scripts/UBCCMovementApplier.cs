using UnityEngine;

namespace CGT.CharacterControls
{
    /// <summary>
    /// Manages the movement for an instance of a Unity Built-In Character Controller
    /// </summary>
    public class UBCCMovementApplier : MonoBehaviour
    {
        [SerializeField] protected CharacterController charaController;

        protected virtual void Awake()
        {
            if (charaController == null)
            {
                charaController = GetComponent<CharacterController>();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (charaController.enabled)
            {
                Vector3 scaled = Movement * Time.fixedDeltaTime;
                //Debug.Log($"Applying movement. Unscaled: {Movement} \nScaled: {scaled}");
                charaController.Move(scaled);
            }
        }

        /// <summary>
        /// Unscaled
        /// </summary>
        public virtual Vector3 Movement
        {
            get { return movement; }
            set { movement = value; }
        }

        protected Vector3 movement;

        /// <summary>
        /// Unscaled
        /// </summary>
        public virtual float XMovement
        {
            get { return movement.x; }
            set { movement.x = value; }
        }

        /// <summary>
        /// Unscaled
        /// </summary>
        public virtual float YMovement
        {
            get { return movement.y; }
            set { movement.y = value; }
        }

        /// <summary>
        /// Unscaled
        /// </summary>
        public virtual float ZMovement
        {
            get { return movement.z; }
            set { movement.z = value; }
        }

        protected virtual void OnDisable()
        {
            XMovement = YMovement = ZMovement = 0;
        }
    }
}