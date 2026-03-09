using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Tests.NavMeshTest
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class TestAgent : MonoBehaviour
    {
        [SerializeField] public Transform Target;
        NavMeshAgent navMeshAgent;
        Animator animator;


        [SerializeField] List<TestAgentState> statesList;

        /// <summary>
        /// READ-ONLY
        /// </summary>
        [SerializeField]TestAgentState thinkState;

        Dictionary<TestAgentState.StateID, TestAgentState> states = new();


        

        void Awake()
        {
            foreach (var state in statesList)
            {
                states.Add(state.stateID, state);
            }

            thinkState = states[TestAgentState.StateID.Idle];

            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            thinkState.OnStateUpdate(this);
            Think();
        }

        bool HasArrived()
        {
            bool hasArrived = true;
            hasArrived &= navMeshAgent != null;

            float tolerance = 0.1f;
            hasArrived &= navMeshAgent.remainingDistance <= (navMeshAgent.stoppingDistance + tolerance);
            if (hasArrived)
            {
                Debug.Log($"has arrived: {transform.position} -|-> {navMeshAgent.destination}");
            }
            return hasArrived;
            //return NavMeshAgentUtilities.HasReachedDestination(navMeshAgent);
        }

        void Think()
        {
            Debug.Log("Think()ing...");
            switch (thinkState.stateID)
            {
                case TestAgentState.StateID.Idle:
                    if (Target != null)
                    {
                        Transition(TestAgentState.StateID.Approaching);
                        animator.SetTrigger("Approach");
                        Debug.Log("Idle --> Approach");
                    }
                    else
                    {
                        Debug.Log("Idle --> Idle");
                    }
                    break;
                case TestAgentState.StateID.Approaching:
                    if (HasArrived())
                    {
                        Transition(TestAgentState.StateID.Idle);
                        animator.SetTrigger("Idle");
                        Debug.Log("Approach --> Idle");
                    } 
                    else
                    {
                        Debug.Log("Approach --> Approach");
                    }
                    break;
                default:
                    break;
            }
            void Transition(TestAgentState.StateID stateID)
            {
                thinkState.OnStateExit(this);
                thinkState = states[stateID];
                thinkState.OnStateEnter(this);
            }
        }
    }
}