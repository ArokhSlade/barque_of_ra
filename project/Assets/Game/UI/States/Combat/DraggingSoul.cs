using UnityEngine;

namespace BarqueOfRa.Game.UI.States.Combat
{
    public class DraggingSoul : CombatUIState
    {
        public override CombatUIState StateUpdate()
        {
            base.StateUpdate();

            CombatUIState nextState = this;

            if (owner.SelectedSoul != null && owner.SoulDummyInstance != null)
            {
                if (wasHighlightableHit)
                {
                    owner.SoulDummyInstance.transform.position = highlightableHit.point + owner.DummyFloatOffset;

                    #region Shady 
                    owner.SelectedSlot = owner.Barque.DefenseSlots.ClosestSlotToPosition(highlightableHit, 10); // uses defense slot script to get closes unit slots to raycast point position for highlightablehit // returns null when boat is moving and when the snapping threshold is reached  
                    if (owner.HighlighterInstance && owner.SelectedSlot)
                    {

                        owner.HighlighterInstance.GetComponent<HighlighterController>().updatingSnapPoint(owner.SelectedSlot.transform.position,owner.SelectedSlot.IsFree);
                    }
                    #endregion 
                    //TODO(Gerald 2025 08 04): review this logic
                    if (slotHit.collider != null)
                    {
                      //  owner.SelectedSlot = slotHit.collider.GetComponent<DefenseSlot>();
                    }

                    if (owner.SelectedSlot != null)
                    {
                        owner.SelectedSlot.SetHoverOverState();
                        owner.LastSelectedSlot = owner.SelectedSlot;
                    }

                    else if (owner.SelectedSlot == null && owner.LastSelectedSlot != null)
                    {
                        owner.LastSelectedSlot.ResetMaterial();
                        owner.LastSelectedSlot = null;
                    }
                    #region Shady 
                    else if (owner.SelectedSlot == null) // check if owner slot is null and disables highlighter line 
                    {
                        owner.HighlighterInstance.GetComponent<HighlighterController>().DisableLineIndicator();
                    }
                    #endregion
                }
            }

            if (owner.PositiveClickReleased)
            {
                if (owner.SoulDummyInstance != null)
                {
                    Destroy(owner.HighlighterInstance);
                    Destroy(owner.SoulDummyInstance);
                }

                if (owner.SelectedSoul == null)
                {
                    Debug.LogError($"UIState is DraggingSoul, but SelectedSoul is null");
                    nextState = owner.NeutralState;
                }
                else
                {
                    bool freeSlotSelected = false;
                    //if (wasSlotHit)
                    //{

                        freeSlotSelected = owner.SelectedSlot != null && owner.SelectedSlot.IsFree;
                        if (freeSlotSelected) 
                        {
                            owner.ShowUnitSelectUI();
                            nextState = owner.SelectingUnitState;
                            freeSlotSelected = true;
                        }
                    //}
                    if (!freeSlotSelected)
                    {
                        owner.CancelSummon();
                        nextState = owner.NeutralState;
                    }
                }
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