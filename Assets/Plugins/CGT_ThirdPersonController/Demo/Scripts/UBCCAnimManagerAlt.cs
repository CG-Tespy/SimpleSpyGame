using UnityEngine;

namespace CGT.CharacterControls
{
    public class UBCCAnimManagerAlt : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected StateMachine _stateMachine;

        [SerializeField] protected MotionState _idleState, _walkState, _runState, _sprintState;

        [SerializeField] protected UBCCMovementApplier _movementApplier;
        [SerializeField] protected GroundCheck _groundCheck;
        [SerializeField] protected float _transitionSpeed = 5f;

        [Header("Animator numeric values (use positive nums)")]
        [SerializeField] protected float _walkVal = 0.33f;
        [SerializeField] protected float _runVal = 0.5f;
        [SerializeField] protected float _sprintVal = 1f;

        [Header("Animation Keys")]
        [SerializeField] protected string horizMoveKey = "horiz";
        [SerializeField] protected string forwardMoveKey = "vert";
        [SerializeField] protected string inAirKey = "air";
        [SerializeField] protected string airVelKey = "airVel";

        protected virtual void Awake()
        {
            _animPrevSpeed = _animator.speed;
            _inputReader = GetComponentInParent<IMovementInputReader>();
            _steering = GetComponentInParent<LocomotionTracker>();
        }

        protected float _animPrevSpeed;
        protected IMovementInputReader _inputReader;
        protected LocomotionTracker _steering;

        protected virtual void Update()
        {
            bool shouldFreezeAnimation = Time.timeScale <= 0;
            if (shouldFreezeAnimation)
            {
                return;
            }

            UpdateMainMovementVal();
            UpdateCacheBasedOnInput();
            HandleHorizGroundAnims();
            HandleForwardGroundAnims();
            HandleAirAnims();
        }

        protected virtual void UpdateMainMovementVal()
        {
            bool charIsWalking = _walkState != null && _stateMachine.HasStateActive(_walkState);
            if (charIsWalking)
            {
                _mainMovementVal = _walkVal;
            }

            bool charIsRunning = _runState != null && _stateMachine.HasStateActive(_runState);
            if (charIsRunning)
            {
                _mainMovementVal = _runVal;
            }

            bool charIsSprinting = _sprintState != null && _stateMachine.HasStateActive(_sprintState);
            if (charIsSprinting)
            {
                _mainMovementVal = _sprintVal;
            }
        }

        protected float _mainMovementVal;

        protected virtual void UpdateCacheBasedOnInput()
        {
            _horizMovement = _forwardMovement = 0;

            bool thereIsHorizMovement = _inputReader.CurrentMovementInput.x != 0;
            if (thereIsHorizMovement)
            {
                _horizMovement = _mainMovementVal;
            }

            bool thereIsForwardMovement = _inputReader.CurrentMovementInput.y != 0;
            if (thereIsForwardMovement)
            {
                _forwardMovement = _mainMovementVal;
            }

            _horizMovement *= Mathf.Sign(_inputReader.CurrentMovementInput.x);
            _forwardMovement *= Mathf.Sign(_inputReader.CurrentMovementInput.y);
        }

        protected float _horizMovement, _forwardMovement;

        protected virtual void HandleHorizGroundAnims()
        {
            if (!_groundCheck.IsPositive)
            {
                return;
            }

            NotifyAnimatorOfHorizMovement();
        }

        protected virtual void NotifyAnimatorOfHorizMovement()
        {
            float prevHorizMovement = _animator.GetFloat(horizMoveKey);
            _horizMovement = Mathf.Lerp(prevHorizMovement, _horizMovement, Time.deltaTime * _transitionSpeed);
            // We want to limit the decimal points
            _horizMovement = (float)System.Math.Round(_horizMovement, 3);
            //Debug.Log($"POST-lerp anim horiz movement is now {_horizMovement}");
            _animator.SetFloat(horizMoveKey, _horizMovement);
        }

        protected virtual void HandleForwardGroundAnims()
        {
            if (!_groundCheck.IsPositive)
            {
                return;
            }

            NotifyAnimatorOfForwardMovement();
        }

        protected virtual void NotifyAnimatorOfForwardMovement()
        {
            float prevForwardMovement = _animator.GetFloat(forwardMoveKey);
            _forwardMovement = Mathf.Lerp(prevForwardMovement, _forwardMovement, Time.deltaTime * _transitionSpeed);
            _forwardMovement = (float)System.Math.Round(_forwardMovement, 3);
            //Debug.Log($"POST-lerp anim FORWARD movement is now {_forwardMovement}");
            _animator.SetFloat(forwardMoveKey, _forwardMovement);
        }

        protected virtual void HandleAirAnims()
        {
            _animator.SetBool(inAirKey, _groundCheck.IsPositive == false);

            if (_groundCheck.IsPositive)
            {
                return;
            }

            float yMove = _movementApplier.YMovement;
            float airVel = 0;

            if (yMove > 0)
            {
                airVel = maxMove;
            }
            else
            {
                airVel = minMove;
            }

            _animator.SetFloat(airVelKey, airVel);
        }

        protected static float minMove = -1, maxMove = 1;

        protected virtual void OnEnable()
        {
            _animator.speed = _animPrevSpeed;
        }

        protected virtual void OnDisable()
        {
            _animator.speed = 0;
        }
    }
}