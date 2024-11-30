using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CGT.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CGT
{
    public class ScaleButtonOnHover : MonoBehaviour
    {
        [SerializeField] protected Button _toScale;
        [SerializeField] protected float _scaleDuration = 2f;
        [SerializeField] protected Vector3 _scaleCeiling = Vector3.one * 1.25f;
        
        protected virtual void Awake()
        {
            if (_toScale == null)
            {
                _toScale = this.GetComponent<Button>();
            }

            _pointerEvents = _toScale.GetComponent<UIPointerEvents>();
            if (_pointerEvents == null)
            {
                _pointerEvents = _toScale.gameObject.AddComponent<UIPointerEvents>();
            }

            _origScale = _toScale.transform.localScale;
            _scaleSequence = DOTween.Sequence();
        }

        protected UIPointerEvents _pointerEvents;
        protected Vector3 _origScale;
        protected Sequence _scaleSequence;

        protected virtual void OnEnable()
        {
            _pointerEvents.PointerEnter += OnPointerEnter;
            _pointerEvents.PointerExit += OnPointerExit;
        }

        protected virtual void OnPointerEnter(PointerEventData eventData)
        {
            _scaleSequence.Kill();
            _scaleSequence = DOTween.Sequence();
            
            // For avoiding issues that may arise between mouse and gamepad inputs
            _toScale.transform.DOPunchScale(_scaleCeiling, _scaleDuration, 1, 1);

        }

        protected virtual void OnPointerExit(PointerEventData eventData)
        {
            _toScale.transform.DOKill();
            //_toResize.transform.DOPunchScale(_origScale, 0.5f, 10, 0);
        }

        protected virtual void OnDisable()
        {
            _pointerEvents.PointerEnter -= OnPointerEnter;
            _pointerEvents.PointerExit -= OnPointerExit;
        }
    }
}