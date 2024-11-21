using CGT;
using CGT.CharacterControls;
using CGT.Events;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using System.Linq;
using DG.Tweening;


namespace FightToTheLast
{
    public class PlayerHideTrigger : MonoBehaviour
    {
        [SerializeField] protected Transform _mainBody;
        [Tooltip("Helps us know when we're in range of a hiding spot")]
        [SerializeField] protected Collider _spotDetector;

        [SerializeField] [Tag] protected string _hidingSpotTag;
        [SerializeField] protected State _hidingState;
        [SerializeField] protected GameObject[] _disableOnHideStart;
        [SerializeField] protected GameObject[] _enableOnHideEnd;

        [SerializeField] protected float _spotSwapRadius = 5f;

        [Tooltip("How far to rotate the player relative to the rotation of the hiding spot")]
        [SerializeField] protected Vector3 _rotationMultipliers = Vector3.one;
        // ^We use multipliers instead of absolute offsets to account for when a hiding spot's
        // rotation is negative on any axis

        [Tooltip("How far another hiding spot can be for the player to jump from one to another")]
        [SerializeField] protected float _spotJumpDetectorRadius = 5f;

        [Tooltip("Which state to transition to when the player wants to unhide")]
        [SerializeField] protected State _onHideExit;

        protected virtual void Awake()
        {
            if (!_spotDetector.TryGetComponent(out _collisionEvents))
            {
                _collisionEvents = _spotDetector.gameObject.AddComponent<CollisionEventNotifier>();
            }

            _stateMachine = GetComponentInParent<StateMachine>();
            _inputReader = GetComponentInParent<AltInputReader>();
            _charaController = GetComponentInParent<CharacterController>();
        }

        protected CollisionEventNotifier _collisionEvents;
        protected StateMachine _stateMachine;
        protected AltInputReader _inputReader;
        protected CharacterController _charaController;
        // ^We need to enable and disable this so we can pull off the
        // hiding-spot-swapping

        protected virtual void OnEnable()
        {
            _collisionEvents.TriggerEnter += OnTriggerEnterResponse;
            _collisionEvents.TriggerExit += OnTriggerExitResponse;
            _inputReader.HideStart += OnHideStartInput;
            _inputReader.CancelHideStart += OnCancelHideStart;
            _hidingState.Exited += OnHideExited;
        }

        protected virtual void OnTriggerEnterResponse(Collider other)
        {
            bool touchedHidingSpot = other.gameObject.CompareTag(_hidingSpotTag) && !CurrentlyHiding;

            if (touchedHidingSpot)
            {
                _hidingSpotToMoveTo = other.transform;
                Debug.Log("Touched hiding spot!");
            }
        }

        [SerializeField] [ReadOnly] protected Transform _hidingSpotToMoveTo;

        protected virtual void OnTriggerExitResponse(Collider other)
        {
            bool exitedHidingSpot = !CurrentlyHiding && other.transform != null && other.transform == _hidingSpotToMoveTo;

            if (exitedHidingSpot)
            {
                Debug.Log($"Exited hiding spot");
                _hidingSpotToMoveTo = null;
            }
        }

        protected virtual void OnHideStartInput()
        {
            bool shouldStartHiding = _hidingSpotToMoveTo != null && !CurrentlyHiding;
            if (shouldStartHiding)
            {
                _stateMachine.ExitAllActiveStates();
                _hidingState.Enter();

                ShiftToNewHidingSpot();
                
                foreach (GameObject gameObjectEl in _disableOnHideStart)
                {
                    gameObjectEl.SetActive(false);
                }

                _charaController.enabled = false;
            }

            bool shouldSwapToOtherSpot = CurrentlyHiding && _hidingSpotToMoveTo != null &&
                _hidingSpotToMoveTo != _hidingSpotWeAreIn;

            if (shouldSwapToOtherSpot)
            {
                ShiftToNewHidingSpot();
                _charaController.enabled = false;
            }

        }

        protected virtual bool CurrentlyHiding
        {
            get
            {
                bool result = _stateMachine.HasStateActive(_hidingState);
                return result;
            }
        }

        protected virtual void ShiftToNewHidingSpot()
        {
            Vector3 newRotation = _hidingSpotToMoveTo.localEulerAngles;
            newRotation.x *= _rotationMultipliers.x;
            newRotation.y *= _rotationMultipliers.y;
            newRotation.z *= _rotationMultipliers.z;

            _mainBody.localEulerAngles = newRotation;
            Vector3 newPos = _mainBody.position;
            newPos.x = _hidingSpotToMoveTo.position.x;
            newPos.y = _hidingSpotToMoveTo.position.y;

            _mainBody.DOMove(newPos, 0.1f);
            _hidingSpotWeAreIn = _hidingSpotToMoveTo;
        }

        [SerializeField] [ReadOnly] protected Transform _hidingSpotWeAreIn;

        protected virtual void OnCancelHideStart()
        {
            bool shouldStopHiding = CurrentlyHiding && _onHideExit != null;
            if (shouldStopHiding)
            {
                _hidingState.Exit();
                _onHideExit.Enter();
                _charaController.enabled = true;
            }
        }

        protected virtual void OnHideExited(IState exited)
        {
            foreach (GameObject gameObjectEl in _enableOnHideEnd)
            {
                gameObjectEl.SetActive(true);
            }

            _hidingSpotWeAreIn = null;
        }

        protected virtual void OnDisable()
        {
            _collisionEvents.TriggerEnter -= OnTriggerEnterResponse;
            _collisionEvents.TriggerExit -= OnTriggerExitResponse;
            _inputReader.HideStart -= OnHideStartInput;
            _inputReader.CancelHideStart -= OnCancelHideStart;
            _hidingState.Exited -= OnHideExited;
        }

        // For now, don't worry about whether the player is facing the spot

        protected virtual void Update()
        {
            if (CurrentlyHiding)
            {
                SearchForOtherSpotToHopTo();
            }
        }

        protected virtual void SearchForOtherSpotToHopTo()
        {
            // Since in Persona 5, you can quickly hop from one hiding spot to the next
            // while staying hidden
            Physics.OverlapSphereNonAlloc(_mainBody.position, _spotSwapRadius, hits);

            _hidingSpotToMoveTo = (from hitEl in hits
                                  where hitEl != null
                                  where hitEl.CompareTag(_hidingSpotTag)
                                  select hitEl.transform).FirstOrDefault();

            if (_hidingSpotToMoveTo != null)
            {
                Debug.Log($"Other spot found: {_hidingSpotToMoveTo.name}");
            }
        }

        protected Collider[] hits = new Collider[10];

        protected virtual void OnDrawGizmos()
        {
            if (_mainBody != null)
            {
                Color prevCol = Gizmos.color;
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_mainBody.position, _spotSwapRadius);
                Gizmos.color = prevCol;
            }
        }

    }
}