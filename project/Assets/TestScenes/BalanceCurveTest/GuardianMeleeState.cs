using UnityEngine;
using BarqueOfRa.Game.Units.Enemies;


namespace BarqueOfRa.Test.BalanceCurveTest
{   
    public class GuardianMeleeState : MonoBehaviour
    {
        public enum StateID
        {
            None = 0,
            Idle = 10,
            Attack = 20,
        }

        [SerializeField] protected StateID stateID;
        public StateID ID => stateID;

        
        virtual public void OnStateEnter(GuardianMelee_BalanceCurveTest guardianMelee)
        {
            //Debug.Log($"Entering {this} : {stateID}");
        }

        virtual public void OnStateUpdate(GuardianMelee_BalanceCurveTest guardianMelee)
        {
            //Debug.Log($"Updating {this} : {stateID}");
        }

        virtual public void OnStateExit(GuardianMelee_BalanceCurveTest guardianMelee)
        {
            //Debug.Log($"Exiting {this} : {stateID}");
        }   
    }
}