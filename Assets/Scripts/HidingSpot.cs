using CGT.CharacterControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSpyGame
{
    public class HidingSpot : MonoBehaviour
    {
        [Tooltip("This will show up while Third Eye is active")]
        [SerializeField] protected GameObject _thirdEyeVfx;

        protected virtual void Awake()
        {
            _inputReader = FindObjectOfType<AltInputReader>();
            _thirdEyeVfx.SetActive(false);
        }
        
        protected AltInputReader _inputReader;


        protected virtual void OnEnable()
        {
            _inputReader.ThirdEyeToggleStart += OnThirdEyeToggle;
        }

        protected virtual void OnThirdEyeToggle()
        {
            _thirdEyeVfx.SetActive(!_thirdEyeVfx.activeSelf);
        }

        protected virtual void OnDisable()
        {
            _inputReader.ThirdEyeToggleStart -= OnThirdEyeToggle;
        }
    }
}