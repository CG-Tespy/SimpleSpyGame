using CGT.CharacterControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using CGT;

// Since for some reason, the UBCC anim manager can't access the SimpleSpyGame namespace
namespace SimpleSpyGame
{
    public class SpyGameAnimManager : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected StateMachine _stateMachine;

        [Header("States To Watch For")]
        [SerializeField] protected MotionState _idleState;
        [SerializeField] protected MotionState _walkState, _runState, _sprintState;
        [SerializeField] protected State _hidingState;

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
        [SerializeField] protected string hidingKey = "hide";
        [SerializeField] protected string victoryKey = "victory";
        [SerializeField] protected string failureKey = "failure";

        protected virtual void Awake()
        {
            _animPrevSpeed = _animator.speed;
            _inputReader = GetComponentInParent<IMovementInputReader>();
            _steering = GetComponentInParent<LocomotionTracker>();

            _animBoolKeys = new List<string>()
            {
                hidingKey,
            };

            _animFloatKeys = new List<string>()
            {
                horizMoveKey,
                forwardMoveKey,
                airVelKey,
            };
        }

        protected float _animPrevSpeed;
        protected IMovementInputReader _inputReader;
        protected LocomotionTracker _steering;
        protected IList<string> _animBoolKeys = new List<string>();
        protected IList<string> _animFloatKeys = new List<string>();

        protected virtual void Update()
        {
            bool shouldFreezeAnimation = Time.timeScale <= 0;
            if (shouldFreezeAnimation || _levelOver)
            {
                return;
            }

            ResetAnimBools();
            UpdateMainMovementVal();
            UpdateCacheBasedOnInput();
            HandleHorizGroundAnims();
            HandleForwardGroundAnims();
            HandleAirAnims();
            HandleHidingAnims();
        }

        protected bool _levelOver;

        protected virtual void ResetAnimBools()
        {
            foreach (string key in _animBoolKeys)
            {
                _animator.SetBool(key, false);
            }

        }

        protected virtual void UpdateMainMovementVal()
        {
            _mainMovementVal = 0;

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

        protected virtual void HandleHidingAnims()
        {
            if (_stateMachine.HasStateActive(_hidingState))
            {
                _animator.SetBool(hidingKey, true);
            }
        }

        protected virtual void OnEnable()
        {
            StageEvents.PlayerWon += OnPlayerLostOrWon;
            StageEvents.PlayerLost += OnPlayerLostOrWon;
            StageEvents.PlayerWon += OnPlayerWon;
            StageEvents.PlayerLost += OnPlayerLost;
            _animator.speed = _animPrevSpeed;
        }

        protected virtual void OnDisable()
        {
            StageEvents.PlayerWon -= OnPlayerLostOrWon;
            StageEvents.PlayerLost -= OnPlayerLostOrWon;
            StageEvents.PlayerWon -= OnPlayerWon;
            StageEvents.PlayerLost -= OnPlayerLost;
            _animator.speed = 0;
        }

        [SerializeField] protected GameObject[] _toDisableOnGameOver = new GameObject[0];

        protected virtual void OnPlayerLostOrWon()
        {
            _levelOver = true;

            ResetAnimBools();
            _animator.SetFloat(horizMoveKey, 0);
            _animator.SetFloat(forwardMoveKey, 0);
            
        }

        protected virtual void OnPlayerWon()
        {
            _animator.SetBool(victoryKey, true);
        }

        protected virtual void OnPlayerLost()
        {
            _animator.SetBool(failureKey, true);
        }

    }
}