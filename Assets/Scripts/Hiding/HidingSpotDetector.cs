using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

namespace FightToTheLast
{
    public class HidingSpotDetector : MonoBehaviour
    {
        [SerializeField] [Tag] protected string _hidingSpotTag = string.Empty;

        [Tooltip("For detecting spots to jump to (implying you're currently in one")]
        [SerializeField] protected Transform _jumpDetectionOrigin;
        [SerializeField] protected float _jumpDetectionRadius = 10f;

        [Tooltip("For detecting spots to move to (implying you're NOT currently in one)")]
        [SerializeField] protected Transform _normalDetectionOrigin;
        [SerializeField] protected float _normalDetectionRadius = 3f;

        [SerializeField] protected LayerMask _hidingSpotLayers = ~0;

        protected virtual void Awake()
        {
            _player = GetComponentInParent<StealthPlayerController>();
        }

        protected StealthPlayerController _player;
        
        protected virtual void Update()
        {
            if (_player.IsHiding)
            {
                Debug.Log($"Searching for hiding spots using the jump origin.");
                FindHidingSpots(_jumpDetectionOrigin, _jumpDetectionRadius);
            }
            else
            {
                Debug.Log($"Searching for hiding spots using the normal origin.");
                FindHidingSpots(_normalDetectionOrigin, _normalDetectionRadius);
            }
        }

        protected virtual void FindHidingSpots(Transform detectionOrigin, float searchRadius)
        {
            Physics.OverlapSphereNonAlloc(detectionOrigin.position, searchRadius, _potentialSpots, _hidingSpotLayers);

            _spotsDetected = (from coll in _potentialSpots
                              where coll != null && coll.CompareTag(_hidingSpotTag)
                              select coll.transform).ToList();
        }

        protected Collider[] _potentialSpots = new Collider[5];
        public IList<Transform> SpotsDetected
        {
            get { return _spotsDetected; }
        }

        [SerializeField] [ReadOnly] protected List<Transform> _spotsDetected = new List<Transform>();

        protected virtual void OnDrawGizmos()
        {
            Color prevColor = Gizmos.color;
            Gizmos.color = Color.blue;
            if (_normalDetectionOrigin != null)
            {
                Gizmos.DrawWireSphere(_normalDetectionOrigin.position, _normalDetectionRadius);
            }

            if (_jumpDetectionOrigin != null)
            {
                Gizmos.DrawWireSphere(_jumpDetectionOrigin.position, _jumpDetectionRadius);
            }

            Gizmos.color = prevColor;
        }

    }
}