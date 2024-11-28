using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using CGT.Utils;
using CGT;

namespace SimpleSpyGame
{
    public class HidingSpotDetector : MonoBehaviour
    {
        [SerializeField] [Tag] protected string _hidingSpotTag = string.Empty;

        [Tooltip("For detecting spots to jump to (implying you're currently in one")]
        [SerializeField] protected BoxCollider _jumpDetectionOrigin;

        [Tooltip("For detecting spots to move to (implying you're NOT currently in one)")]
        [SerializeField] protected SphereCollider _normalDetectionOrigin;

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
                FindHidingSpots(_jumpDetectionOrigin);
            }
            else
            {
                Debug.Log($"Searching for hiding spots using the normal origin.");
                FindHidingSpots(_normalDetectionOrigin);
            }
        }

        protected virtual void FindHidingSpots(SphereCollider detectionOrigin)
        {
            Physics.OverlapSphereNonAlloc(detectionOrigin.transform.position,
                detectionOrigin.radius, _potentialSpots, _hidingSpotLayers);

            _spotsDetected = (from coll in _potentialSpots
                              where coll != null && coll.CompareTag(_hidingSpotTag)
                              select coll.transform).Distinct().ToList();
        }

        protected Collider[] _potentialSpots = new Collider[5];

        [SerializeField][ReadOnly] protected List<Transform> _spotsDetected = new List<Transform>();

        public IList<Transform> SpotsDetected
        {
            get { return _spotsDetected; }
        }

        protected virtual void FindHidingSpots(BoxCollider detectionOrigin)
        {
            Physics.OverlapBoxNonAlloc(detectionOrigin.transform.position,
                detectionOrigin.size / 2, _potentialSpots,
                detectionOrigin.transform.rotation, _hidingSpotLayers);

            _spotsDetected = (from coll in _potentialSpots
                              where coll != null && coll.CompareTag(_hidingSpotTag)
                              select coll.transform).Distinct().ToList();
        }

        protected virtual void OnDrawGizmos()
        {
            Color prevColor = Gizmos.color;
            Gizmos.color = Color.blue;
            if (_normalDetectionOrigin != null)
            {
                SphereCollider collider = _normalDetectionOrigin.GetComponent<SphereCollider>();
                Gizmos.DrawWireSphere(_normalDetectionOrigin.transform.position, collider.radius);
            }

            Gizmos.color = prevColor;
        }

    }
}