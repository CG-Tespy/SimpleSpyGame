using CGT;
using CGT.CharacterControls;
using CGT.Events;
using NaughtyAttributes;
using UnityEngine;

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
        [SerializeField] private GameObject[] _enableOnHideEnd;

        [Tooltip("How far to rotate the player relative to the rotation of the hiding spot")]
        [SerializeField] protected Vector3 _rotationMultipliers = Vector3.one;
        // ^We use multipliers instead of absolute offsets to account for when a hiding spot's
        // rotation is negative on any axis

        protected virtual void Awake()
        {
            if (!_spotDetector.TryGetComponent(out _collisionEvents))
            {
                _collisionEvents = _spotDetector.gameObject.AddComponent<CollisionEventNotifier>();
            }

            _stateMachine = GetComponentInParent<StateMachine>();
            _inputReader = GetComponentInParent<AltInputReader>();
        }

        protected CollisionEventNotifier _collisionEvents;
        protected StateMachine _stateMachine;
        protected AltInputReader _inputReader;

        protected virtual void OnEnable()
        {
            _collisionEvents.TriggerEnter += OnTriggerEnterResponse;
            _collisionEvents.TriggerExit += OnTriggerExitResponse;
            _inputReader.HideStart += OnHideStartInput;
            _hidingState.Exited += OnHideExited;
        }

        protected virtual void OnTriggerEnterResponse(Collider other)
        {
            bool touchedHidingSpot = other.gameObject.CompareTag(_hidingSpotTag);

            if (touchedHidingSpot)
            {
                _hidingSpotInRange = other.transform;
                Debug.Log("Touched hiding spot!");
            }
        }

        protected Transform _hidingSpotInRange;

        protected virtual void OnTriggerExitResponse(Collider other)
        {
            bool exitedHidingSpot = other.transform != null && other.transform == _hidingSpotInRange;

            if (exitedHidingSpot)
            {
                Debug.Log($"Exited hiding spot");
                _hidingSpotInRange = null;
            }
        }

        protected virtual void OnHideStartInput()
        {
            if (_hidingSpotInRange && !_stateMachine.HasStateActive(_hidingState))
            {
                _stateMachine.ExitAllActiveStates();
                _hidingState.Enter();

                Vector3 newRotation = _hidingSpotInRange.localEulerAngles;
                newRotation.x *= _rotationMultipliers.x;
                newRotation.y *= _rotationMultipliers.y;
                newRotation.z *= _rotationMultipliers.z;

                _mainBody.localEulerAngles = newRotation;
                
                foreach (GameObject gameObjectEl in _disableOnHideStart)
                {
                    gameObjectEl.SetActive(false);
                }
            }
        }

        protected virtual void OnHideExited(IState exited)
        {
            foreach (GameObject gameObjectEl in _enableOnHideEnd)
            {
                gameObjectEl.SetActive(true);
            }
        }

        protected virtual void OnDisable()
        {
            _collisionEvents.TriggerEnter -= OnTriggerEnterResponse;
            _collisionEvents.TriggerExit -= OnTriggerExitResponse;
            _inputReader.HideStart -= OnHideStartInput;
            _hidingState.Exited -= OnHideExited;
        }

        // For now, don't worry about whether the player is facing the spot

        protected virtual void Update()
        {
            
        }
    }
}