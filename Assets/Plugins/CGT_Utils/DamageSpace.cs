using CGT.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CGT
{
    public class DamageSpace : MonoBehaviour
    {
        [SerializeField] protected CollisionEventNotifier _collider;
        [SerializeField] protected List<string> _validTargetTags = new List<string>();
        [field: SerializeField] public virtual float DamageToDeal { get; set; } = 10f;

        protected virtual void Awake()
        {
            _collider.gameObject.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            _collider.TriggerEnter += OnTriggerEnterResponse;
        }

        protected virtual void OnTriggerEnterResponse(Collider other)
        {
            INumericResourceHandler damageable = other.GetComponent<INumericResourceHandler>();
            if (damageable != null && _validTargetTags.Contains(other.tag)
                && !_alreadyHurtThisFrame.Contains(damageable))
            {
                damageable.CurrentValue -= DamageToDeal;
                _alreadyHurtThisFrame.Add(damageable);
                DamagedSomething(damageable);
            }
        }

        protected IList<INumericResourceHandler> _alreadyHurtThisFrame = new List<INumericResourceHandler>();
        // ^We want to avoid hurting the same damageable more than once in the same frame

        public UnityAction<INumericResourceHandler> DamagedSomething = delegate { };

        protected virtual void OnDisable()
        {
            _collider.TriggerEnter -= OnTriggerEnterResponse;
        }

        protected virtual void LateUpdate()
        {
            _alreadyHurtThisFrame.Clear();
        }

        public virtual void Flash(float delay, float duration = 0.1f)
        {
            CancelInvoke();
            _collider.gameObject.SetActive(false);
            Invoke(nameof(StartFlash), delay);
            Invoke(nameof(EndFlash), delay + duration);
        }

        protected virtual void StartFlash()
        {
            _collider.gameObject.SetActive(true);
        }
        protected virtual void EndFlash()
        {
            _collider.gameObject.SetActive(false);
        }

    }
}
