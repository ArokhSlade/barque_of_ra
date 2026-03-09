using UnityEngine;


namespace Tests.NavMeshTest
{
    
    
    public class TestAgentState : MonoBehaviour
    {
        public enum StateID
        {
            None = 0,
            Idle = 10,
            Approaching = 20,
        }


        [SerializeField] public StateID stateID { get; protected set; }

        
        virtual public void OnStateEnter(TestAgent testAgent)
        {
            Debug.Log($"Entering {this} : {stateID}");
        }

        virtual public void OnStateUpdate(TestAgent testAgent)
        {
            Debug.Log($"Updating {this} : {stateID}");
        }

        virtual public void OnStateExit(TestAgent testAgent)
        {
            Debug.Log($"Exiting {this} : {stateID}");
        }   
    }
}