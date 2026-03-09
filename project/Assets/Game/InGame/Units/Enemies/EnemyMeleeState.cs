using UnityEngine;


namespace BarqueOfRa.Game.Units.Enemies.Melee
{
    public class EnemyMeleeState : MonoBehaviour
    {
        public enum StateID
        {
            None = 0,
            Idle = 10,
            Approach = 20,
            AttackGuardian = 30,
            AttackBarque = 50,
            Perished = 100,
        }


        [SerializeField] protected StateID stateID;
        public StateID ID => stateID;


        virtual public void OnStateEnter(EnemyMelee enemy)
        {
            //Debug.Log($"Entering {this} : {stateID}");
        }

        virtual public void OnStateUpdate(EnemyMelee enemy)
        {
            //Debug.Log($"Updating {this} : {stateID}");
        }

        virtual public void OnStateExit(EnemyMelee enemy)
        {
            //Debug.Log($"Exiting {this} : {stateID}");
        }
    }
}