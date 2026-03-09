using BarqueOfRa.Game.Units;
using BarqueOfRa.Units;
using BarqueOfRa.Game.UI;
using System;
using UnityEngine;
using static BarqueOfRa.Game.UI.InteractivePopUp;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.InputSystem;

namespace BarqueOfRa.Game.UI.States.Combat
{
    public class TutorialManager : MonoBehaviour
    {
        public static Action OnAnyPlayerActions;
        #region Action player event 


        public static Action OnSoulsMoved;
        public static Action<UnitType> OnUnitSummoned;
        public static Action OnUnitSwitched;
        public static Action OnUnitChangedPosition;

        [SerializeField] InputActionAsset OnSkipClicked;

        #endregion

        public bool ToturialCompletedFully;

        public DefenseSlots SlotManager;



        TutorialTrigger StartPoint;



        [SerializeField] InteractivePopUp[] PopUpList;
        int CurrentTutorialPopUpIndex;

        public  TutorialManager Instance => this;

        Neutral playerActions;

        
        private void OnEnable()
        {
            OnSoulsMoved += CompareSoulsMove;
            OnUnitChangedPosition += CompareReposition;
            OnUnitSummoned += CompareUnitSummon;

            OnSkipClicked.FindActionMap("Tutorial").FindAction("SkipToturial").started += Skipping;

            SlotManager = FindAnyObjectByType<DefenseSlots>();
        }
        private void OnDisable()
        {


            OnSkipClicked.FindActionMap("Tutorial").FindAction("SkipToturial").started -= Skipping;

            OnSoulsMoved -= CompareSoulsMove;
            OnUnitChangedPosition -= CompareReposition;
            OnUnitSummoned -= CompareUnitSummon;

        }
      
        public void CompareSoulsMove()
        {
            if (IsPopUpFinished())
            { return; }

            if (PopUpList[CurrentTutorialPopUpIndex].GOALS.TutorialGoal == playerGameInputs.MovingSouls) 
            {
                PopUpList[CurrentTutorialPopUpIndex].Hide();
                Debug.Log("Finishing moving souls");
            }

        }
        public void CompareUnitSummon(UnitType Utyp)
        {
            if (IsPopUpFinished())
            { return; }

            if (PopUpList[CurrentTutorialPopUpIndex].GOALS.TutorialGoal == playerGameInputs.SummoningUnit)
            {
                PopUpList[CurrentTutorialPopUpIndex].CheckSummoningTypGoal(Utyp);
                Debug.Log("Finishing SUmming souls");

            }
        }
        public void CompareReposition()
        {
            if (IsPopUpFinished())
            { return; }
                if (PopUpList[CurrentTutorialPopUpIndex].GOALS.TutorialGoal == playerGameInputs.RepositioningUnits)
            {
                PopUpList[CurrentTutorialPopUpIndex].Hide();
                Debug.Log("Finishing Reposition souls");

            }
        }
      
        private void Awake()
        {

            CurrentTutorialPopUpIndex = 0;
            playerActions = FindAnyObjectByType<Neutral>();
            SlotManager = FindAnyObjectByType<DefenseSlots>();

            this.enabled = false;
         
        }
        public void ToturialStarted()
        {

            if (ToturialCompletedFully) { return; }

            PopUpList[0].Activated = true;
            PopUpList[0].Init(this);

        }
       
  
        public void Skipping(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                if (PopUpList[CurrentTutorialPopUpIndex].Skippable)
                {

                    PopUpList[CurrentTutorialPopUpIndex].Hide();

                }

            }
              
        }
        public void ActivatingNextPopUp()
        {
           
            CurrentTutorialPopUpIndex++;

            if (!IsPopUpFinished())
            {
                PopUpList[CurrentTutorialPopUpIndex].Activated = true;
                PopUpList[CurrentTutorialPopUpIndex].Init(this);
            }
        }
        bool IsPopUpFinished()
        {
            if(CurrentTutorialPopUpIndex < PopUpList.Length)
            {
                return false;
            }
            ToturialCompletedFully = true;
            return true;
        }

        int TotalUnitCount()
        {
            int A = 0;
            foreach(DefenseSlot Slot in SlotManager.slots)
            {
                if(Slot.TryGetOccupantGuardian())
                {
                    A++;
                }
            }
            return A;
        }
    }
}

