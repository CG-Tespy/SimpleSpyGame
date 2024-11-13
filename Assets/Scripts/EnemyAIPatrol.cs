using CGT;
using CGT.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using NaughtyAttributes;

namespace FightToTheLast
{
    public class EnemyAIPatrol : EnemyAIState
    {
        [SerializeField] protected Transform _toMove;

        [Tooltip("Holds the waypoints for the patrol route")]
        [SerializeField] protected Transform _holdsRoute;

        [SerializeField] protected State _onWaypointReached;

        [Header("For Debugging")]
        [ReadOnly]
        [SerializeField] protected Transform _targetWaypoint;

        public override void Init()
        {
            base.Init();
            _toWaypoint = new NavMeshPath();
            PrepWaypointSequence();

        }

        protected NavMeshPath _toWaypoint;

        protected virtual void PrepWaypointSequence()
        {
            foreach (Transform waypointTrans in _holdsRoute)
            {
                _waypoints.Add(waypointTrans);
            }

            IList<Transform> reversedWaypoints = _waypoints.ReversedCopy();
            reversedWaypoints.RemoveAt(0);

            if (reversedWaypoints.Count > 1)
            {
                reversedWaypoints.RemoveAt(reversedWaypoints.Count - 1);
            }

            _waypoints.AddRange(reversedWaypoints);
        }


        protected IList<Transform> _waypoints = new List<Transform>();

        public override void Enter(IState enteringFrom = null)
        {
            base.Enter(enteringFrom);
            // We want to go to the nearest waypoint before deciding on a series of paths. 
            // After all, we might've entered this state after we got done chasing something

            TargetTheNextWaypoint();
            UpdatePathToWaypoint();
            TurnBeforeGoingDownPath(ThenGoToTargetWaypoint);
        }

        protected int _targetWaypointIndex = -1;

        protected virtual void UpdatePathToWaypoint()
        {
            _toWaypoint = new NavMeshPath();
            NavMesh.CalculatePath(AgentPos, _targetWaypoint.position, NavMesh.AllAreas, _toWaypoint);
        }

        protected virtual void TurnBeforeGoingDownPath(TweenCallback onDoneTurning)
        {
            PausedForTurning = true;
            Vector3 nextCorner = _toWaypoint.corners[1];
            // ^Not sure why this only works when the index is 1, but hey
            AgentTrans.DOLookAt(nextCorner, Settings.PatrolTurnDur)
                .OnComplete(onDoneTurning);
        }

        public virtual bool PausedForTurning { get; set; }

        protected virtual void ThenGoToTargetWaypoint()
        {
            PausedForTurning = false;
            _navAgent.isStopped = false;
            _navAgent.SetPath(_toWaypoint);
        }

        public override void ExecUpdate()
        {
            base.ExecUpdate();
            bool closeEnough = _navAgent.remainingDistance <= 0.01f;
            if (closeEnough && !PausedForTurning)
            {
                if (_onWaypointReached != null)
                {
                    TransitionTo(_onWaypointReached);
                }
                else
                {
                    TargetTheNextWaypoint();
                    UpdatePathToWaypoint();
                    TurnBeforeGoingDownPath(ThenGoToTargetWaypoint);
                }
            }
        }

        protected virtual void TargetTheNextWaypoint()
        {
            _targetWaypointIndex++;

            if (_targetWaypointIndex >= _waypoints.Count)
            {
                _targetWaypointIndex = 0;
            }

            _targetWaypoint = _waypoints[_targetWaypointIndex];
        }

        public override void Exit()
        {
            base.Exit();

            _navAgent.isStopped = PausedForTurning = true;
        }

        private void OnDrawGizmos()
        {
            if (_toMove != null)
            {
                Vector3 startPoint = _toMove.position;
                Vector3 endPoint = startPoint + (_toMove.forward * 5);
                Color prevColor = Gizmos.color;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(startPoint, endPoint);
                Gizmos.color = prevColor;   
            }
        }

    }
}