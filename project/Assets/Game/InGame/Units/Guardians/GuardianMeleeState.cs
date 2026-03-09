using UnityEngine;

namespace BarqueOfRa.Game.Units.Guardians.Melee
{
    public class GuardianMeleeState : MonoBehaviour
    {
        public enum StateID
        {
            None = 0,
            Idle = 10,
            Attack = 20,
            Perished = 100,
        }

        [SerializeField] protected StateID stateID;
        public StateID ID => stateID;

        virtual public void OnStateEnter(GuardianMelee guardian)
        {
            //Debug.Log($"Entering {this} : {stateID}");
        }

        virtual public void OnStateUpdate(GuardianMelee guardian)
        {
            //Debug.Log($"Updating {this} : {stateID}");
        }

        virtual public void OnStateExit(GuardianMelee guardian)
        {
            //Debug.Log($"Exiting {this} : {stateID}");
        }
    }
}