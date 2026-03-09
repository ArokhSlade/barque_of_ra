using BarqueOfRa.Units;
using UnityEngine;

namespace BarqueOfRa.Game.UI.States.Combat
{
    public class Neutral : CombatUIState
    {           
        public override CombatUIState StateUpdate()
        {
            base.StateUpdate();

            CombatUIState nextState = this;
            
            if (owner.PositiveClickDown)
            {
                Debug.Log(this + ": PositiveClickDown");
                if (wasSoulHit)
                {
                    Debug.Log(this + ": wasSoulHit");

                    owner.SelectedSoul = soulHit.collider.GetComponent<Soul>();
                    Debug.Assert(owner.SelectedSoul != null, "soul was hit but no soul component found!");
                    if (owner.Barque.SoulCabin.SoulsLeft <= 1)
                    {
                        owner.DenySummoningLastSoul();
                    }
                    else
                    {
                        TutorialManager.OnSoulsMoved?.Invoke();

                        owner.SoulDummyInstance = GameObject.Instantiate(owner.SoulDummyPrefab, soulHit.point, Quaternion.identity);
                        owner.PrepareSummon();
                        nextState = owner.DraggingSoulState;
                    }
                }
                else if (wasGuardianHit)
                {
                    owner.SelectedGuardian = guardianHit.collider.GetComponent<Guardian>();
                    Debug.Log(this + ": wasGuardianHit");
                    owner.PrepareMove();
                    nextState = owner.DraggingGuardianState;
                }               
            }

            return nextState;
        }
    }
}