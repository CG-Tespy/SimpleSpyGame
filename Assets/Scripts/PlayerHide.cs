using CGT.Events;
using NaughtyAttributes;
using UnityEngine;

namespace FightToTheLast
{
    public class PlayerHide : MonoBehaviour
    {
        [Tooltip("Helps us know when we're in range of a hiding spot")]
        [SerializeField] protected Collider _spotDetector;
        [SerializeField] [Tag] protected string _hidingSpotTag;

        protected virtual void Awake()
        {
            if (!_spotDetector.TryGetComponent(out _collisionEvents))
            {
                _collisionEvents = _spotDetector.gameObject.AddComponent<CollisionEventNotifier>();
            }
        }

        protected virtual void OnEnable()
        {
            _collisionEvents.TriggerEnter += OnTriggerEnterResponse;
            _collisionEvents.TriggerExit += OnTriggerExitResponse;
        }

        protected CollisionEventNotifier _collisionEvents;

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

        protected virtual void OnDisable()
        {
            _collisionEvents.TriggerEnter -= OnTriggerEnterResponse;
            _collisionEvents.TriggerExit -= OnTriggerExitResponse;
        }
    }
}