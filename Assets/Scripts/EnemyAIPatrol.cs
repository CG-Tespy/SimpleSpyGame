using CGT;
using CGT.Utils;
using DG.Tweening;
using NaughtyAttributes;
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

        [Header("States To Transition To")]
        [SerializeField] protected State _onWaypointReached;
        [SerializeField] protected State _onTargetFound;

        [Header("For Debugging")]
        [ReadOnly] [SerializeField] protected Transform _targetWaypoint;
        [ReadOnly] [SerializeField] protected List<Transform> _waypoints = new List<Transform>();
        [ReadOnly] [SerializeField] protected int _targetWaypointIndex = -1;

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

        public override void Enter(IState enteringFrom = null)
        {
            base.Enter(enteringFrom);
            // We want to go to the nearest waypoint before deciding on a series of paths. 
            // After all, we might've entered this state after we got done chasing something
            _navAgent.speed = Settings.PatrolSpeed;
            _navAgent.stoppingDistance = Settings.WaypointStoppingDistance;
            _navAgent.isStopped = false;
            TargetTheNextWaypoint();
            UpdatePathToWaypoint();
            TurnBeforeGoingDownPath(ThenGoToTargetWaypoint);
        }

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

            _targetSpotted = null;
            CheckForTarget();

            if (_targetSpotted != null && _onTargetFound != null)
            {
                _controller.Target = _targetSpotted;
                TransitionTo(_onTargetFound);
                return;
            }

            bool closeEnough = _navAgent.remainingDistance <= Settings.WaypointStoppingDistance + 0.01f;
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

        protected virtual void CheckForTarget()
        {
            Collider[] targetsFound = Physics.OverlapSphere(AgentPos, Settings.VisionRange, Settings.TargetLayers);
            bool targetWithinTheRightDistance = targetsFound.Length > 0;
            if (!targetWithinTheRightDistance)
            {
                return;
            }

            Transform targetToConsider = targetsFound[0].transform;
            Vector3 towardsTarget = (targetToConsider.position - AgentPos).normalized;

            bool inVisionConeArea = Vector3.Angle(AgentTrans.forward, towardsTarget) < Settings.VisionAngle / 2;

            if (inVisionConeArea)
            {
                bool isViewObstructed = Physics.Raycast(AgentPos, towardsTarget,
                    Settings.VisionRange, Settings.ObstacleLayers);

                if (!isViewObstructed)
                {
                    _targetSpotted = targetToConsider;
                    return;
                }
            }
        }

        protected Transform _targetSpotted;

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
            _targetSpotted = null;
            _navAgent.isStopped = PausedForTurning = true;
        }

        private void OnDrawGizmos()
        {
            if (_toMove != null && SightOrigin != null)
            {
                Vector3 startPoint = SightOrigin.position;
                Vector3 endPoint = startPoint + (_toMove.forward * Settings.VisionRange);
                Color prevColor = Gizmos.color;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(startPoint, endPoint);
                Gizmos.color = prevColor;   
            }
        }

    }
}