using System;

namespace CGT
{
    public interface IState
    {
        /// <summary>
        /// For things a state needs set up before ever being entered
        /// </summary>
        void Init();

        void Enter();
        event Action<IState> Entered;

        void ExecEarlyUpdate();
        void ExecUpdate();
        void ExecLateUpdate();
        void ExecFixedUpdate();

        void Exit();
        event Action<IState> Exited;
    }
}