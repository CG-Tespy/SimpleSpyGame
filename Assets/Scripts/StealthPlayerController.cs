using CGT;
using CGT.CharacterControls;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using UnityEngine.Serialization;
using CGT.Utils;
using System;

namespace SimpleSpyGame
{
    public class StealthPlayerController : MonoBehaviour
    {
        [SerializeField] protected AltInputReader _inputReader;
        [SerializeField] protected State _hidingState;
        [SerializeField] protected State _onHideExit;

        [Tooltip("Caught by an enemy, that is")]
        [FormerlySerializedAs("_disableIfCaught")]
        [SerializeField] protected GameObject[] _disableOnLevelOver = new GameObject[] { };

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
            _charaController = GetComponent<CharacterController>();
        }

        protected StateMachine _stateMachine;
        protected HidingSpotDetector _spotDetector;
        protected HidingSpotTraversal _spotTraversal;
        protected CharacterController _charaController;

        protected virtual void OnEnable()
        {
            _inputReader.HideStart += OnHideStartInput;
            _inputReader.CancelHideStart += OnCancelHideStart;
            StageEvents.PlayerWon += OnPlayerWonOrLost;
            StageEvents.PlayerLost += OnPlayerWonOrLost;
        }

        protected virtual void OnHideStartInput()
        {
            if (SpotsInRange.Count == 0 ||
                _spotTraversal.IsTraversing ||
                IsSpotted ||
                GameManager.S.LevelOver)
            {
                return;
            }

            Transform whereToHide = null;

            if (!IsHiding)
            {
                whereToHide = SpotsInRange[0];
            }
            else
            {
                whereToHide = _spotDetector.NearestSpotCamCanSee(Camera.main.transform.position,
                    CurrentHidingSpot);
            }

            if (whereToHide != null)
            {
                bool alreadyHiding = IsHiding;
                Action onComplete = null;

                if (!alreadyHiding)
                {
                    _stateMachine.ExitAllActiveStates();
                    _hidingState.Enter();
                    StartEnteringHidingSpot();
                }
                else
                {
                    onComplete = SignalTeleportEnd;
                    TeleportStart();
                }

                IsHiding = true;
                CurrentHidingSpot = whereToHide;

                // We only want a poof when teleporting from one hiding spot to another
                _spotTraversal.TraverseTo(whereToHide, alreadyHiding, onComplete);
            }
        }

        protected virtual void SignalTeleportEnd()
        {
            TeleportEnd();
        }

        public virtual bool IsSpotted { get; set; }

        /// <summary>
        /// For when this starts getting into a hiding spot (as opposed to teleporting to one)
        /// </summary>
        public event Action StartEnteringHidingSpot = delegate { };

        public event Action TeleportStart = delegate { };
        public event Action TeleportEnd = delegate { };

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
            if (IsSpotted || !IsHiding ||
                _onHideExit == null || _spotTraversal.IsTraversing ||
                GameManager.S.LevelOver)
            {
                return;
            }

            Debug.Log($"Stopped hiding");
            IsHiding = false;
            CurrentHidingSpot = null;
            _hidingState.Exit();
            _onHideExit.Enter();
        }

        protected virtual void OnPlayerWonOrLost()
        {
            _charaController.enabled = false;

            foreach (var toDisable in _disableOnLevelOver)
            {
                toDisable.SetActive(false);
            }
        }

        protected virtual void OnDisable()
        {
            _inputReader.HideStart -= OnHideStartInput;
            _inputReader.CancelHideStart -= OnCancelHideStart;
            StageEvents.PlayerWon -= OnPlayerWonOrLost;
            StageEvents.PlayerLost -= OnPlayerWonOrLost;
        }

    }
}