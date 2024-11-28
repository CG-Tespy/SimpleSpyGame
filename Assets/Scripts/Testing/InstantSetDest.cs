using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleSpyGame
{
    public class InstantSetDest : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected Transform _destTransform;

        protected virtual void Awake()
        {
            CharacterController charaController = agent.GetComponent<CharacterController>();

            if (charaController != null)
            {
                charaController.enabled = false;
            }

            agent.isStopped = false;
            agent.SetDestination(_destTransform.position);
        }
    }
}