using UnityEngine;

namespace SimpleSpyGame
{
    public class GovernmentDocument : MonoBehaviour
    {
        public virtual void OnCollect()
        {
            if (IsRetrieved)
            {
                return;
            }

            IsRetrieved = true;

            // If we decide to add a visual indicator of this being retrieved, we might
            // want to work with that here
            AnyRetrieved(this);
        }

        public virtual bool IsRetrieved { get; protected set; }
        public static event System.Action<GovernmentDocument> AnyRetrieved = delegate { };
    }
}