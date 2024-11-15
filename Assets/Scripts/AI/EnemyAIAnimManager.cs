using CGT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightToTheLast
{
    public class EnemyAIAnimManager : MonoBehaviour
    {
        [SerializeField] protected StateMachine _stateMachine;
        [SerializeField] protected Animator _animator;

        [Header("States")]
        [SerializeField] protected EnemyAIPatrol _patrolState;
        [SerializeField] protected State _idleState;
        [SerializeField] protected State _chaseState;

        [Header("Anim Param Names")]
        [SerializeField] protected string _idleKey = "idle";
        [SerializeField] protected string _walkingKey = "walking";
        [SerializeField] protected string _sprintingKey = "sprinting";
        [SerializeField] protected string _turningKey = "turning";
        [SerializeField] protected string _runningKey = "running";

        protected virtual void Awake()
        {
            _allKeys.Add(_idleKey);
            _allKeys.Add(_walkingKey);
            _allKeys.Add(_sprintingKey);
            _allKeys.Add(_turningKey);
            _allKeys.Add(_runningKey);
        }

        protected IList<string> _allKeys = new List<string>();

        protected virtual void Update()
        {
            ResetAnimBools();
            HandlePatrolAnim();
            HandleIdleAnim();
            HandleChaseAnim();
        }

        protected virtual void HandlePatrolAnim()
        {
            bool patrolling = _stateMachine.HasStateActive(_patrolState);

            if (!patrolling)
            {
                return;
            }

            bool movingWhilePatrolling = patrolling && !_patrolState.PausedForTurning;
            bool pauseDuringPatrol = patrolling && _patrolState.PausedForTurning;
            if (movingWhilePatrolling)
            {
                _animator.SetBool(_walkingKey, true);
            }
            else if (pauseDuringPatrol)
            {
                _animator.SetBool(_turningKey, true);
            }
        }

        protected virtual void HandleIdleAnim()
        {
            bool idling = _stateMachine.HasStateActive(_idleState);
            if (idling)
            {
                _animator.SetBool(_idleKey, true);
            }
        }

        protected virtual void HandleChaseAnim()
        {
            bool chasing = _stateMachine.HasStateActive(_chaseState);

            if (!chasing)
            {
                return;
            }

            _animator.SetBool(_runningKey, true);
        }

        protected virtual void ResetAnimBools()
        {
            foreach (string key in _allKeys)
            {
                _animator.SetBool(key, false);
            }
        }
    }
}