using Tests.NavMeshTest;
using UnityEngine;
using UnityEngine.AI;

namespace Tests.NavMeshTest
{
    public class TestAgentApproachState : TestAgentState
    {   
        TestAgent testAgent;
        NavMeshAgent navMeshAgent;
        bool initialized = false;

        public float repathSeconds = .5f;
        float repathSecondsRemaining;
        
        void Awake()
        {
            this.stateID = StateID.Approaching;
        }
        
        override public void OnStateEnter(TestAgent testAgent)
        {
            base.OnStateEnter(testAgent);

            repathSecondsRemaining = repathSeconds;
            if (!testAgent.TryGetComponent(out testAgent))
            {
                Debug.LogError($"{testAgent.gameObject} does not have a TestAgent component!");
                return;
            }                
                
            if (!testAgent.TryGetComponent(out navMeshAgent))
            {
                Debug.LogError($"{testAgent.gameObject} does not provide a NavMeshAgent!");
                return;
            }

            navMeshAgent.destination = testAgent.Target.position;
            Debug.LogWarning("navAgent Destination set");
            Debug.Log($"navMeshAgent.destination = {navMeshAgent.destination}");
            Debug.Log($"testAgent.Target.position = {testAgent.Target.position}");
            initialized = true;
        }

        override public void OnStateUpdate(TestAgent testAgent)
        {
            base.OnStateUpdate(testAgent);

            if (!initialized)
            {
                Debug.LogError("${this} not initialized!");
                return;
            }
                
                
            if (testAgent.Target == null) 
            { return; }
                
            repathSecondsRemaining -= Time.deltaTime;
            if (repathSecondsRemaining > 0f) 
            {
                return;
            }

            navMeshAgent.destination = testAgent.Target.position;
            Debug.LogWarning("navAgent Destination set");
            Debug.Log($"navMeshAgent.destination = {navMeshAgent.destination}");
            Debug.Log($"testAgent.Target.position = {testAgent.Target.position}");
            repathSecondsRemaining = repathSeconds;            
        }
            
        override public void OnStateExit(TestAgent testAgent)
        {
            base.OnStateExit(testAgent);

            initialized = false;
            navMeshAgent.ResetPath();
            Debug.LogWarning("path reset");
            Debug.Log($"navMeshAgent.destination = {navMeshAgent.destination}");
        }
    }
}