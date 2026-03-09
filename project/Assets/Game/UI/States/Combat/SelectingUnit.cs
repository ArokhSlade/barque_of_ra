using UnityEngine;

namespace BarqueOfRa.Game.UI.States.Combat
{
    public class SelectingUnit : CombatUIState
    {
        bool unitSelected = false;

        public override CombatUIState StateUpdate()
        {
            base.StateUpdate();

            CombatUIState nextState = this;

            if (unitSelected)
            {
                nextState = owner.NeutralState;
            }
            
            return nextState;
        }

        public override void Enter()
        {
            unitSelected = false;
        }

        public override void Exit()
        {
        }

        public void OnSummonSelectedUnit(int unitTypeIndex)
        {
            owner.SummonSelectedUnit(unitTypeIndex);
            unitSelected = true;
        }
    }

}