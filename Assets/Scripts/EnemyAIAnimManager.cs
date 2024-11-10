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
        [SerializeField] protected State _patrolState;

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

        protected virtual void OnEnable()
        {

        }

        protected virtual void Update()
        {
            ResetAnimBools();

            if (_stateMachine.HasStateActive(_patrolState))
            {
                _animator.SetBool(_walkingKey, true);
            }
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