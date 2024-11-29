using CGT.CharacterControls;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSpyGame
{
    public class PlayerHideTrigger : MonoBehaviour
    {
        [Tooltip("Helps us know when we're in range of a hiding spot")]
        [SerializeField] protected HidingSpotDetector _spotDetector;

        protected virtual void Awake()
        {
            _inputReader = GetComponentInParent<AltInputReader>();
            _player = GetComponentInParent<StealthPlayerController>();
        }

        protected AltInputReader _inputReader;
        protected StealthPlayerController _player;

        protected virtual void OnEnable()
        {
            _inputReader.HideStart += OnHideStartInput;
            _inputReader.CancelHideStart += OnCancelHideStart;
        }

        protected virtual void OnHideStartInput()
        {
            bool shouldStartHiding = !_player.IsHiding && SpotsInRangeOf.Count > 0;
            if (shouldStartHiding)
            {
                HideStartTrigger();
            }
        }

        public event Action HideStartTrigger = delegate { };

        protected virtual IList<Transform> SpotsInRangeOf { get { return _spotDetector.SpotsDetected; } }

        protected virtual void OnCancelHideStart()
        {
            if (_player.IsHiding)
            {
                CancelHideTrigger();
            }
        }

        public event Action CancelHideTrigger = delegate { };
        
        protected virtual void OnDisable()
        {
            _inputReader.HideStart -= OnHideStartInput;
            _inputReader.CancelHideStart -= OnCancelHideStart;
        }

    }
}