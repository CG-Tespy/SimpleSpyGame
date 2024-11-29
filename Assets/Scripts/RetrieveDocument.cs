using CGT.Events;
using UnityEngine;

namespace SimpleSpyGame
{
    public class RetrieveDocument : MonoBehaviour
    {
        [SerializeField] protected Collider _retrievalCollider;

        protected virtual void Awake()
        {
            _collisionNotifier = _retrievalCollider.GetComponent<CollisionEventNotifier>();
            if (_collisionNotifier == null)
            {
                _collisionNotifier = _retrievalCollider.gameObject.AddComponent<CollisionEventNotifier>();
            }
        }

        protected CollisionEventNotifier _collisionNotifier;

        protected virtual void OnEnable()
        {
            _collisionNotifier.TriggerEnter += OnTriggerEnterResponse;
        }

        protected virtual void OnTriggerEnterResponse(Collider other)
        {
            GovernmentDocument govDoc = other.GetComponent<GovernmentDocument>();

            if (govDoc != null && !govDoc.IsRetrieved)
            {
                govDoc.OnCollect();
                DocsRetrieved++;
                Debug.Log($"Retrieved a doc!");
                StageEvents.DocRetrieved();
            }

        }

        public virtual int DocsRetrieved { get; protected set; }

        protected virtual void OnDisable()
        {
            _collisionNotifier.TriggerEnter -= OnTriggerEnterResponse;
        }

    }
}