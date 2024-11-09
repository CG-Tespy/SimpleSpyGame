using CGT.CharacterControls;
using UnityEngine;

namespace CGT.PlayerMoveController
{
    public class UBCCAnimationManager : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected UBCCMovementApplier movementApplier;
        [SerializeField] protected CharacterController charaController;
        [SerializeField] protected UBCCLocomotion locomotion;
        [SerializeField] protected UBCCMoveSpeed moveSpeed;
        [SerializeField] protected float _transitionSpeed = 5f;

        [Header("Animator numeric values (use positive nums)")]
        [SerializeField] protected float runVal = 0.5f;
        [SerializeField] protected float sprintVal = 1f;

        [Header("Animation Keys")]
        [SerializeField] protected string horizMoveKey = "horiz";
        [SerializeField] protected string vertMoveKey = "vert";
        [SerializeField] protected string inAirKey = "air";
        [SerializeField] protected string airVelKey = "airVel";

        protected virtual void Update()
        {
            bool shouldFreezeAnimation = Time.timeScale <= 0;
            if (shouldFreezeAnimation)
            {
                return;
            }

            HandleHorizGroundAnims();
            HandleVertGroundAnims();
            HandleAirAnims();

            animator.SetBool(inAirKey, charaController.isGrounded == false);
        }

        protected virtual void HandleHorizGroundAnims()
        {
            if (!charaController.isGrounded)
            {
                return;
            }

            float horizMovement = 0;
            bool thereIsHorizMovement = locomotion.XMovement != 0;
            
            if (thereIsHorizMovement)
            {
                horizMovement = runVal;

                if (locomotion.IsSprinting && moveSpeed.AllowSprint)
                {
                    horizMovement = sprintVal;
                }
            }
            
            horizMovement *= Mathf.Sign(locomotion.XMovement);

            float prevHorizMovement = animator.GetFloat(horizMoveKey);
            horizMovement = Mathf.Lerp(prevHorizMovement, horizMovement, Time.deltaTime * _transitionSpeed);
            animator.SetFloat(horizMoveKey, horizMovement);
        }

        protected virtual void HandleVertGroundAnims()
        {
            if (!charaController.isGrounded)
            {
                return;
            }

            float vertMovement = 0;

            bool thereIsVertMovement = locomotion.ZMovement != 0;

            if (thereIsVertMovement)
            {
                vertMovement = runVal;

                if (locomotion.IsSprinting && moveSpeed.AllowSprint)
                {
                    vertMovement = sprintVal;
                }
            }
            
            vertMovement *= Mathf.Sign(locomotion.ZMovement);

            float prevVertMovement = animator.GetFloat(vertMoveKey);
            vertMovement = Mathf.Lerp(prevVertMovement, vertMovement, Time.deltaTime * _transitionSpeed);
            animator.SetFloat(vertMoveKey, vertMovement);
        }

        protected static float minMove = -1, maxMove = 1;

        protected virtual void HandleAirAnims()
        {
            if (charaController.isGrounded)
            {
                return;
            }

            float yMove = movementApplier.YMovement;
            float airVel = 0;

            if (yMove > 0)
            {
                airVel = maxMove;
            }
            else
            {
                airVel = minMove;
            }

            animator.SetFloat(airVelKey, airVel);
        }

        protected virtual Vector3 CharaControllerVel { get { return charaController.velocity; } }

        // Since we want the animation to freeze when this is disabled
        protected virtual void Awake()
        {
            _animPrevSpeed = animator.speed;
        }

        protected float _animPrevSpeed;

        protected virtual void OnEnable()
        {
            animator.speed = _animPrevSpeed;
        }

        protected virtual void OnDisable()
        {
            animator.speed = 0;
        }

    }
}