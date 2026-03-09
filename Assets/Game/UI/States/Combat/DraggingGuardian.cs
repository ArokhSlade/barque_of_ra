using BarqueOfRa.Units;
using UnityEngine;

namespace BarqueOfRa.Game.UI.States.Combat
{
    public class DraggingGuardian : CombatUIState
    {
        public override CombatUIState StateUpdate()
        {
            base.StateUpdate();

            CombatUIState nextState = this;
            
            if (owner.GuardianDummyInstance)
            {
                owner.GuardianDummyInstance.transform.position = highlightableHit.point + owner.DummyFloatOffset;

                owner.GuardianDummyInstance.GetComponent<Health>().RemoveHealth(owner.GuardianDummyInstance.GetComponent<Health>().CurrentHealth - owner.GuardianDummySource.GetComponent<Health>().CurrentHealth);

                Debug.Log(owner.GuardianDummySource.GetComponent<Health>().CurrentHealth + "CurrentHealth us s ");


                // check for closes unit slot point once it is transitioned into active mode 
                // make sure to return null when player is transitioning 
                // adjust cancel when selectedslot null is and when not 
                //update snap point of highlighter
                // Maximum Snapping threshold (bonus)



                if (wasHighlightableHit)
                {
                    owner.GuardianDummyInstance.transform.position = highlightableHit.point + owner.DummyFloatOffset;
                    #region Shady
                    owner.SelectedSlot = owner.Barque.DefenseSlots.ClosestSlotToPosition(highlightableHit, 10); // a variable for to replace 10 that is snapping threshold
                    
                    if (owner.HighlighterInstance && owner.SelectedSlot)
                    {
                        owner.HighlighterInstance.GetComponent<HighlighterController>().updatingSnapPoint(owner.SelectedSlot.transform.position,true);
                    }
                    else if(owner.SelectedSlot == null)
                    {
                        owner.HighlighterInstance.GetComponent<HighlighterController>().DisableLineIndicator();
                    }
                    #endregion
                    if (slotHit.collider != null)
                    {
                        //owner.SelectedSlot = slotHit.collider.GetComponent<DefenseSlot>();

                    }

                    if (owner.SelectedSlot != null)
                    {
                        owner.SelectedSlot.SetHoverOverState();
                        owner.LastSelectedSlot = owner.SelectedSlot;
                    }

                    else if (owner.SelectedSlot == null && owner.LastSelectedSlot != null) // chekc this 
                    {
                        owner.LastSelectedSlot.ResetMaterial();
                        owner.LastSelectedSlot = null;
                    }
                }
            }

            if (owner.PositiveClickReleased)
            {
                Debug.Log($"Mouse released while UI at dragging Guardian, pointing at: {slotHit.collider}");

                if (owner.HighlighterInstance) {  Destroy(owner.HighlighterInstance); }

                bool moveSuccessful = false;

                if (owner.SelectedGuardian == null)
                {
                    Debug.LogWarning("No Guardian selected in state Dragging Guardian");
                }

                if (owner.SelectedSlot) // removed wasSlotHit
                {


                    // if(wasSlotHit))
                    //{
                    //if (owner.SelectedSlot = slotHit.collider.GetComponent<DefenseSlot>())
                    //{
                    if (owner.SelectedSlot.GetComponent<DefenseSlot>())
                    {
                        if (owner.SelectedSlot.IsFree)
                        {
                            Unit unit = owner.SelectedGuardian.GetComponent<Unit>();

                            if (unit == null)
                            {
                                Debug.LogError("guardian object missing unit component when trying to place");
                            }
                            else
                            {
                                unit.transform.position = owner.SelectedSlot.transform.position + unit.Offset;
                                unit.transform.parent = owner.SelectedSlot.transform;

                                owner.DisableGuardianDummy(owner.SelectedGuardian);


                                Debug.Log($"Move successful at {unit.transform.position}");

                                moveSuccessful = true;
                            }
                        }
                        else
                        {
                            Guardian oldOccupantGuardian = owner.SelectedSlot.TryGetOccupantGuardian();
                            if (oldOccupantGuardian != null)
                            {
                                owner.SwapUnits(oldOccupantGuardian.GetComponent<Unit>(), owner.SelectedGuardian.GetComponent<Unit>());
                                moveSuccessful = true;
                            }
                            else
                            {
                                owner.CancelMove();
                            }
                        }
                   }
                   else
                   {
                        Guardian oldOccupantGuardian = guardianHit.collider.GetComponent<Guardian>();
                        if (oldOccupantGuardian != null)
                        {
                            owner.SwapUnits(oldOccupantGuardian.GetComponent<Unit>(), owner.SelectedGuardian.GetComponent<Unit>());
                            moveSuccessful = true;
                        }
                        else
                        {
                           owner.CancelMove();
                        }

                    }
                //}
                }
                //else // mabe we need to make sure with this else that sleection slot is null and make sure movesucceful be false then 
                //{
                //    moveSuccessful = false;
                //}
                if (!moveSuccessful)
                {
                    owner.CancelMove();
                    
                }
                nextState = owner.NeutralState;
            }

            return nextState;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }
    }

}