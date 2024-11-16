using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CGT.Utils;

namespace CGT
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] protected List<Transform> _stateHolders = new List<Transform>();
        [Tooltip("Whether or not to check the state holders' children for states to manage.")]
        [SerializeField] protected bool _searchHolderChildren = true;
        [SerializeField] protected List<State> _initStates = new List<State>();

        protected virtual void Awake()
        {
            RegisterStates();

            foreach (State state in _allStates)
            {
                state.Init();
            }
        }

        protected virtual void RegisterStates()
        {
            _allStates.AddRange(_initStates);

            foreach (Transform holder in _stateHolders)
            {
                IList<State> statesFound;

                if (_searchHolderChildren)
                {
                    statesFound = holder.GetComponentsInChildren<State>();
                }
                else
                {
                    statesFound = holder.GetComponents<State>();
                }

                // To help with debugging, we only want to consider the states that start out enabled
                statesFound = (from state in statesFound
                              where state.enabled
                              select state).ToList();

                _allStates.AddRange(statesFound);
            }
        }

        protected HashSet<State> _allStates = new HashSet<State>();
        [SerializeField] protected List<State> _activeStates = new List<State>();

        protected virtual void OnEnable()
        {
            ListenForStateEvents();
        }

        protected virtual void ListenForStateEvents()
        {
            foreach (State state in _allStates)
            {
                state.Entered += OnStateEntered;
                state.Exited += OnStateExited;
            }
        }

        protected virtual void OnStateEntered(IState thatWasEntered)
        {
            // Best to queue this stuff so that we don't get any errors caused
            // by altering collections while they're being iterated over
            _toAdd.Add((State)thatWasEntered);
        }

        protected IList<State> _toAdd = new List<State>();

        protected virtual void OnStateExited(IState thatWasExited)
        {
            _toRemove.Add((State)thatWasExited);
        }

        protected IList<State> _toRemove = new List<State>();

        protected virtual void OnDisable()
        {
            UNlistenForStateEvents();
            foreach (var state in _activeStates)
            {
                state.Exit();
            }
        }

        protected virtual void UNlistenForStateEvents()
        {
            foreach (State state in _allStates)
            {
                state.Entered -= OnStateEntered;
                state.Exited -= OnStateExited;
            }
        }

        protected virtual void Start()
        {
            foreach (State state in _initStates)
            {
                state.Enter();
            }
        }

        protected virtual void Update()
        {
            foreach (State state in _activeStates)
            {
                state.ExecEarlyUpdate();
                state.ExecUpdate();
            }

            UpdateStateLists();
        }

        protected virtual void UpdateStateLists()
        {
            _activeStates.AddRange(_toAdd);
            _activeStates.RemoveAllIn(_toRemove);

            _toAdd.Clear();
            _toRemove.Clear();
        }

        protected virtual void LateUpdate()
        {
            foreach (State state in _activeStates)
            {
                state.ExecLateUpdate();
            }

            UpdateStateLists();
        }

        protected virtual void FixedUpdate()
        {
            foreach (State state in _activeStates)
            {
                state.ExecFixedUpdate();
            }

            UpdateStateLists();
        }

        public virtual bool HasStateActive(IState state)
        {
            return _activeStates.Contains(state);
        }
    
        public virtual void ExitAllStates()
        {
            foreach (IState state in _activeStates)
            {
                state.Exit();
            }
        }

        public virtual void Register(State state)
        {
            if (state == null)
            {
                Debug.LogError($"Cannot register a null state.");
                return;
            }

            if (!_allStates.Contains(state))
            {
                _allStates.Add(state);
                state.Init();
                state.Entered += OnStateEntered;
                state.Exited += OnStateExited;
            }
        }

    }
}