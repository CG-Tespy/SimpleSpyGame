using CGT;
using CGT.CharacterControls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FightToTheLast
{
    /// <summary>
    /// Disables and enables InputActions when a specified state is entered or exited.
    /// </summary>
    public class InputActionControlOnState : MonoBehaviour
    {
        [TextArea(3, 6)]
        [SerializeField] protected string _notes = "";
        [SerializeField] protected State _state;
        [SerializeField] protected InputActionReference[] _actions;
        [SerializeField] protected Enablement _enablement = Enablement.EnableOnEnterDisableOnExit;
        [SerializeField] protected float _delayBeforeEnable, _delayBeforeDisable;

        public enum Enablement
        {
            Null,
            EnableOnEnterDisableOnExit,
            DisableOnEnterEnableOnExit
        }

        protected virtual void Awake()
        {
            // Turns out that the refs we assign to this script aren't the ones that input reader uses. 
            // Thus, we have to enable and disable things through the reader itself
            _inputReader = GetComponentInParent<AltInputReader>();
        }

        protected AltInputReader _inputReader;

        protected virtual void OnEnable()
        {
            _state.Entered += OnStateEntered;
            _state.Exited += OnStateExited;
        }

        protected virtual void OnStateEntered(IState entered)
        {
            CancelInvoke();

            switch (_enablement)
            {
                case Enablement.EnableOnEnterDisableOnExit:
                    Invoke(nameof(EnableThings), _delayBeforeEnable);
                    break;
                case Enablement.DisableOnEnterEnableOnExit:
                    Invoke(nameof(DisableThings), _delayBeforeDisable);
                    break;
                default:
                    Debug.LogError($"Did not account for {_enablement}");
                    break;
            }
        }

        protected virtual void EnableThings()
        {
            foreach (InputActionReference action in _actions)
            {
                action.action.Enable();
                // ^For when it's an action not used by the input reader, but something like the CinemachineInputProvider
                _inputReader.Enable(action.name);
            }
        }

        protected virtual void DisableThings()
        {
            foreach (InputActionReference action in _actions)
            {
                action.action.Disable();
                _inputReader.Disable(action.name);
            }
        }

        protected virtual void OnStateExited(IState exited)
        {
            CancelInvoke();

            switch (_enablement)
            {
                case Enablement.EnableOnEnterDisableOnExit:
                    Invoke(nameof(DisableThings), _delayBeforeDisable);
                    break;
                case Enablement.DisableOnEnterEnableOnExit:
                    Invoke(nameof(EnableThings), _delayBeforeEnable);
                    break;
                default:
                    Debug.LogError($"Did not account for {_enablement}");
                    break;
            }
        }

        protected virtual void OnDisable()
        {
            _state.Entered -= OnStateEntered;
            _state.Exited -= OnStateExited;
        }
    }
}