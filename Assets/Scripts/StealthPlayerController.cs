using CGT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightToTheLast
{
    public class StealthPlayerController : MonoBehaviour
    {
        [SerializeField] protected State _hidingState;
        public virtual bool IsHiding { get; private set; }

        protected virtual void OnEnable()
        {
            _hidingState.Entered += OnHideStateEntered;
            _hidingState.Exited += OnHideStateEntered;
        }

        protected virtual void OnHideStateEntered(IState entered)
        {
            IsHiding = true;
        }

        protected virtual void OnHideStateExited(IState exited)
        {
            IsHiding = false;
        }

        protected virtual void OnDisable()
        {
            _hidingState.Entered -= OnHideStateEntered;
            _hidingState.Exited -= OnHideStateEntered;
        }
    }
}