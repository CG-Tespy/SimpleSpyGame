using UnityEngine;

namespace CGT.CharacterControls
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] protected LayerMask _groundLayers;
        [SerializeField] protected Transform _baseTrans;
        [SerializeField] protected float _castDistance = 0.2f;

        protected virtual void Update()
        {
            Ray ray = new Ray();
            ray.origin = _baseTrans.position;
            ray.direction = Vector3.down;

            IsPositive = Physics.Raycast(ray, _castDistance);
        }

        [field: SerializeField] public virtual bool IsPositive { get; protected set; }

        private void OnDrawGizmos()
        {
            if (_baseTrans != null)
            {
                Vector3 origin = _baseTrans.position;
                Vector3 endPoint = origin + (Vector3.down * _castDistance);

                Color prevColor = Gizmos.color;
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(origin, endPoint);
                Gizmos.color = prevColor;
            }
        }
    }
}