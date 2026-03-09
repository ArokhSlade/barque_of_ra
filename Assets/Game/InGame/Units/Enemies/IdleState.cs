using UnityEngine;

namespace BarqueOfRa.Game.Units.Enemies.Melee
{
    public class IdleState : EnemyMeleeState
    {        
        void Awake()
        {
            this.stateID = StateID.Idle;
        }
    }
}