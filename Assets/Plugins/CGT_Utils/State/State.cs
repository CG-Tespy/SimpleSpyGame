using System;
using UnityEngine;

namespace CGT
{
    public abstract class State : MonoBehaviour, IState
    {
        /// <summary>
        /// For things the states need to set up before they're ever entered.
        /// </summary>
        public virtual void Init()
        {

        }

        public virtual void Enter(IState enteringFrom = null)
        {
            //Debug.Log($"Entered state {this.name}");
            Entered(this);
        }

        public event Action<IState> Entered = delegate { };

        public virtual void ExecEarlyUpdate()
        {

        }

        public virtual void ExecUpdate()
        {

        }

        public virtual void ExecLateUpdate()
        {

        }

        public virtual void ExecFixedUpdate()
        {

        }

        public virtual void Exit()
        {
            //Debug.Log($"Exited state {this.name}");
            Exited(this);
        }

        public event Action<IState> Exited = delegate { };

        public virtual void Update()
        {
            // This exists only so we can enable and disable this component in the inspector
        }

        protected virtual void TransitionTo(IState state)
        {
            this.Exit();
            state.Enter();
        }
    }
}