using UnityEngine;

namespace BarqueOfRa.Test.BalanceCurveTest
{
    public class IdleState : GuardianMeleeState
    {        
        void Awake()
        {
            this.stateID = StateID.Idle;
        }
    }
}