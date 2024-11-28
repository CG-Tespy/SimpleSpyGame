using CGT;
using CGT.CharacterControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

namespace SimpleSpyGame
{
    public class StealthPlayerController : MonoBehaviour
    {
        [SerializeField] protected AltInputReader _inputReader; 
        [SerializeField] protected State _hidingState;
        [SerializeField] protected State _onHideExit;

        protected virtual void Awake()
        {
            FindComponents();

            _stateMachine.Register(_hidingState);
            _stateMachine.Register(_onHideExit);
        }

        protected virtual void FindComponents()
        {
            if (_inputReader == null)
            {
                _inputReader = GetComponent<AltInputReader>();
            }

            _stateMachine = GetComponent<StateMachine>();
            _spotDetector = GetComponentInChildren<HidingSpotDetector>();
            _spotTraversal = GetComponentInChildren<HidingSpotTraversal>();
        }

        protected StateMachine _stateMachine;
        protected HidingSpotDetector _spotDetector;
        protected HidingSpotTraversal _spotTraversal;
        
        protected virtual void OnEnable()
        {
            _inputReader.HideStart += OnHideStartInput;
            _inputReader.CancelHideStart += OnCancelHideStart;
        }

        protected virtual void OnHideStartInput()
        {
            if (SpotsInRange.Count == 0 || _spotTraversal.IsTraversing)
            {
                return;
            }

            Transform whereToHide = (from spot in SpotsInRange
                                     where spot != null && spot != CurrentHidingSpot
                                     select spot).FirstOrDefault();

            if (whereToHide != null)
            {
                bool alreadyHiding = IsHiding;

                if (!alreadyHiding)
                {
                    _stateMachine.ExitAllActiveStates();
                    _hidingState.Enter();
                }

                IsHiding = true;
                CurrentHidingSpot = whereToHide;
                
                // We only want a poof when teleporting from one hiding spot to another
                _spotTraversal.TraverseTo(whereToHide, alreadyHiding);
            }
        }

        public virtual bool IsHiding
        {
            get { return _isHiding; }
            protected set { _isHiding = value; }
        }

        [SerializeField]
        [ReadOnly] protected bool _isHiding;

        protected IList<Transform> SpotsInRange { get { return _spotDetector.SpotsDetected; } }
        public virtual Transform CurrentHidingSpot { get; protected set; }

        protected virtual void OnCancelHideStart()
        {
            if (!IsHiding || _onHideExit == null || _spotTraversal.IsTraversing)
            {
                return;
            }

            Debug.Log($"Stopped hiding");
            IsHiding = false;
            CurrentHidingSpot = null;
            _hidingState.Exit();
            _onHideExit.Enter();
        }

        protected virtual void OnDisable()
        {
            _inputReader.HideStart -= OnHideStartInput;
            _inputReader.CancelHideStart -= OnCancelHideStart;
        }

    }
}