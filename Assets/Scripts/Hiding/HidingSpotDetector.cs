using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using CGT.Utils;

namespace SimpleSpyGame
{
    public class HidingSpotDetector : MonoBehaviour
    {
        [SerializeField] [Tag] protected string _hidingSpotTag = string.Empty;

        [Tooltip("For detecting spots to jump to (implying you're currently in one")]
        [SerializeField] protected SphereCollider _jumpDetectionOrigin;

        [Tooltip("For detecting spots to move to (implying you're NOT currently in one)")]
        [SerializeField] protected SphereCollider _normalDetectionOrigin;

        [SerializeField] protected LayerMask _hidingSpotLayers = ~0;
        [SerializeField] protected LayerMask _obstacleLayers = ~0;

        protected virtual void Awake()
        {
            _player = GetComponentInParent<StealthPlayerController>();
        }

        protected StealthPlayerController _player;
        
        protected virtual void Update()
        {
            _potentialSpots.Clear();

            if (_player.IsHiding)
            {
                //Debug.Log($"Searching for hiding spots using the jump origin.");
                FindHidingSpots(_jumpDetectionOrigin);
            }
            else
            {
                //Debug.Log($"Searching for hiding spots using the normal origin.");
                FindHidingSpots(_normalDetectionOrigin);
            }

            FilterHidingSpots();
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

        protected virtual void FilterHidingSpots()
        {
            // We don't want to count those outside the player character's line of sight

            for (int i = 0; i < _spotsDetected.Count; i++)
            {
                Transform spot = _spotsDetected[i];
                Vector3 towardsSpot = (spot.position - Camera.main.transform.position).normalized;

                RaycastHit hit;
                bool obstructedViewOfSpot = Physics.Raycast(Camera.main.transform.position, towardsSpot,
                    out hit, 100, _obstacleLayers);

                if (obstructedViewOfSpot)
                {
                    Debug.Log($"{hit.collider.name} is blocking the cam's view of {spot.name}");
                    Debug.DrawLine(Camera.main.transform.position, spot.position, Color.yellow);
                    _spotsDetected.Remove(spot);
                    i--; // To avoid going out of bounds
                }
            }
        }

        protected RaycastHit _filterHit;

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

        public virtual Transform NearestSpotCamCanSee(Vector3 basePos, Transform spotToIgnore, float camAngleLimit = 45f)
        {
            IList<Transform> spotsToConsider = (from spotEl in SpotsDetected
                                                where spotEl != null && spotEl != spotToIgnore
                                                select spotEl).ToList();

            if (spotsToConsider.Count == 0)
            {
                return null;
            }

            Transform nearestSpot = null;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < spotsToConsider.Count; i++)
            {
                Transform spot = spotsToConsider[i];
                float distance = Vector3.Distance(spot.position, basePos);
                if (distance < nearestDistance)
                {
                    Vector3 towardsSpot = (spot.position - basePos).normalized;
                    bool withinAngleLimit = Vector3.Angle(Camera.main.transform.forward, towardsSpot) < camAngleLimit;
                    RaycastHit hit;
                    bool isItObstructed = Physics.Raycast(basePos, towardsSpot, out hit, distance, _obstacleLayers);

                    if (withinAngleLimit && !isItObstructed)
                    {
                        nearestDistance = distance;
                        nearestSpot = spot;
                    }
                }
            }

            return nearestSpot;
        }

    }
}