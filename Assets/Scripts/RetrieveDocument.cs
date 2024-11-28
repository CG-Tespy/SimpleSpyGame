using CGT.Events;
using UnityEngine;

namespace SimpleSpyGame
{
    public class RetrieveDocument : MonoBehaviour
    {
        [SerializeField] protected CollisionEventNotifier _retrievalCollider;

        protected virtual void OnEnable()
        {
            _retrievalCollider.TriggerEnter += OnTriggerEnterResponse;
        }

        protected virtual void OnTriggerEnterResponse(Collider other)
        {
            GovernmentDocument govDoc = other.GetComponent<GovernmentDocument>();

            if (govDoc != null && !govDoc.IsRetrieved)
            {
                govDoc.OnCollect();
                DocsRetrieved++;
                Debug.Log($"Retrieved a doc!");
            }
        }

        public virtual int DocsRetrieved { get; protected set; }

        protected virtual void OnDisable()
        {
            _retrievalCollider.TriggerEnter -= OnTriggerEnterResponse;
        }

    }
}