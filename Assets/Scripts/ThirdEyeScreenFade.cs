using CGT.CharacterControls;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleSpyGame
{
    public class ThirdEyeScreenFade : MonoBehaviour
    {
        [SerializeField] protected Image _screen;
        [Tooltip("On a scale of 0 to 100")]
        [SerializeField] protected float _darknessOpacity = 75f;
        [SerializeField] protected float _fadeDuration = 1f;

        protected virtual void Awake()
        {
            _inputReader = FindObjectOfType<AltInputReader>();
            Color invisible = _screen.color;
            invisible.a = 0;
            _screen.color = invisible;
        }

        protected AltInputReader _inputReader;

        protected virtual void OnEnable()
        {
            _inputReader.ThirdEyeToggleStart += OnThirdEyeToggle;
        }

        protected virtual void OnThirdEyeToggle()
        {
            _thirdEyeActive = !_thirdEyeActive;
            float targetOpacity;

            if (_thirdEyeActive)
            {
                targetOpacity = _darknessOpacity / 100f;
            }
            else
            {
                targetOpacity = 0;
            }

            _fadeTween?.Kill();
            _fadeTween = _screen.DOFade(targetOpacity, _fadeDuration);
        }

        protected bool _thirdEyeActive;
        protected Tween _fadeTween;

        protected virtual void OnDisable()
        {
            _inputReader.ThirdEyeToggleStart -= OnThirdEyeToggle;
        }
    }
}