using CGT;
using CGT.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FightToTheLast
{
    public class EnemyAIPatrol : EnemyAIState
    {
        [SerializeField] protected Transform _toMove;

        [Tooltip("Holds the waypoints for the patrol route")]
        [SerializeField] protected Transform _holdsRoute;

        public override void Init()
        {
            base.Init();
            FindPaths();
        }

        protected virtual void FindPaths()
        {
            _paths.Clear();

            foreach (Transform waypointTrans in _holdsRoute)
            {
                Vector3 waypointPos = waypointTrans.position;
                NavMeshPath toThatPos = new NavMeshPath();
                _navAgent.CalculatePath(waypointPos, toThatPos);
                _paths.Add(toThatPos);
            }

            IList<NavMeshPath> reversedPath = _paths.ReversedCopy();
            _paths.AddRange(reversedPath);

            bool thereIsNoPath = _paths.Count == 0;
            if (thereIsNoPath)
            {
                Debug.LogError($"No waypoints found in Transform {_holdsRoute.name}");
            }

        }

        protected IList<NavMeshPath> _paths = new List<NavMeshPath>();

        public override void Enter(IState enteringFrom = null)
        {
            base.Enter(enteringFrom);
            _currentPathIndex = -1;
            // ^ So that it gets set to the first point when calling GoToNextWaypoint
            GoToNextWaypoint();
        }

        protected NavMeshPath _currentPath;

        protected int _currentPathIndex;

        protected virtual void GoToNextWaypoint()
        {
            _navAgent.isStopped = false;
            DecideNextWaypoint();
            _navAgent.SetPath(_currentPath);
        }

        protected virtual void DecideNextWaypoint()
        {
            _currentPathIndex++;

            if (_currentPathIndex >= _paths.Count)
            {
                _currentPathIndex = 0;
            }

            _currentPath = _paths[_currentPathIndex];
        }

        public override void ExecUpdate()
        {
            base.ExecUpdate();
            if (_navAgent.remainingDistance <= _navAgent.stoppingDistance + 0.01f)
            {
                GoToNextWaypoint();
            }

            Debug.Log($"Remaining distance of {_navAgent.name} is {_navAgent.remainingDistance}");
        }

        public override void Exit()
        {
            base.Exit();
            _navAgent.isStopped = true;
        }

    }
}