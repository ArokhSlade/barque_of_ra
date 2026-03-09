using UnityEngine;

namespace BarqueOfRa.Game.Units.Guardians.Melee
{
    public class IdleState : GuardianMeleeState
    {        
        void Awake()
        {
            this.stateID = StateID.Idle;
        }
    }
}