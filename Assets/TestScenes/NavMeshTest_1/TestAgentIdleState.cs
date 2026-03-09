using UnityEngine;

namespace Tests.NavMeshTest
{
    public class TestAgentIdleState : TestAgentState
    {        
        void Awake()
        {
            this.stateID = StateID.Idle;
        }
    }
}