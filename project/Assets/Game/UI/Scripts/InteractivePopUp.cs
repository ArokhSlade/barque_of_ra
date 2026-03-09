using BarqueOfRa.Game.UI.States.Combat;
using BarqueOfRa.Units;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

namespace BarqueOfRa.Game.UI
{
    [RequireComponent(typeof(Animation))]
    public class InteractivePopUp : ModalMenu
    {
        public enum playerGameInputs
        {
            None = 0,
            MovingSouls = 5,
            SummoningUnit = 10,
            RepositioningUnits = 15
        }
   
        [Serializable]
        public struct Goals
        {
           
            [SerializeField] public playerGameInputs TutorialGoal;

            [SerializeField] public List <UnitType> SummonedTyp;

            [SerializeField] public int NumberOfUnits;
            [SerializeField] public List<GameObject> TextReplacement;
        }

      

       

        public bool Activated,Skippable;

        [SerializeField] Goals TutorialCompletion;
        public Goals GOALS => TutorialCompletion;

        [SerializeField] UnscaledTimer LifeSpan;
        [SerializeField]float SkipFadeDuration , NormalFadeDuration;
        [SerializeField] GameObject HintIndicator;
        [SerializeField] TextMeshProUGUI ObjectiveTacker;

        TutorialManager TM;
        Animation animations;
        public float SKIPDURATION => SkipFadeDuration;
        public float LIFESPAN => NormalFadeDuration;



        [Header("General Player Action and Interaction")]
        [SerializeField] GameObject TargetofInteraction;


        
        private void Awake()
        {
            LifeSpan.Reset();

        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Init(TutorialManager TM)
        {
            this.TM = TM;
            animations = GetComponent<Animation>();
            
            if(!TargetofInteraction)
            {

                if (GOALS.TutorialGoal == playerGameInputs.None)
                {
                    if (Skippable)
                    {


                    }
                    else
                    {

                    }
                }
                else if (GOALS.TutorialGoal == playerGameInputs.MovingSouls)
                {
                    List<Soul> listofSouls = new List<Soul>();
                    foreach (Soul soul in FindObjectsByType<Soul>(FindObjectsSortMode.InstanceID))
                    {
                        listofSouls.Add(soul);
                    }
                    foreach (Soul Asoul in listofSouls)
                    {
                        if (Asoul.gameObject.name != "Ra")
                        {
                            TargetofInteraction = Asoul.gameObject;
                            break;
                        }
                    }
                }
                else if (GOALS.TutorialGoal == playerGameInputs.SummoningUnit)
                {
                    TargetofInteraction = TM.SlotManager.gameObject.transform.Find("FakeSlots").transform.Find("Fake_right").gameObject;
                }
                else if (GOALS.TutorialGoal == playerGameInputs.RepositioningUnits)
                {
                    for (int i = 0; i < 12; i++)
                    {

                        if (TM.SlotManager.slots[i].TryGetOccupantGuardian())
                        {
                            TargetofInteraction = TM.SlotManager.slots[i].TryGetOccupantGuardian().gameObject;
                            break;
                        }
                    }
                }
            }

            Show();
        }
        private void Update()
        {

            if (TargetofInteraction)
            {

                HintIndicator.transform.position = Camera.main.WorldToScreenPoint(TargetofInteraction.transform.position);

            }

            LifeSpan.Update();

           

        }
        public void CheckSummoningTypGoal(UnitType UnitSummoned)
        {

            if (!Activated) { return; }

            if (TutorialCompletion.TutorialGoal == playerGameInputs.SummoningUnit)
            {

                this.gameObject.transform.GetChild(0).transform.Find("Start").transform.gameObject.SetActive(false);

                if (TutorialCompletion.NumberOfUnits > 0)
                {
                    int index = 0;
                    for(int i = 0; i < TutorialCompletion.SummonedTyp.Count;i++)
                    {
                        if(UnitSummoned == TutorialCompletion.SummonedTyp[i])
                        {
                            index = i;
                        }
                        else
                        {
                            TutorialCompletion.TextReplacement[i].gameObject.SetActive(false);
                        }
                    }

                    TutorialCompletion.TextReplacement[index].gameObject.SetActive(true);

                    
                    TutorialCompletion.NumberOfUnits -= 1;
                    ObjectiveTacker.text = (3 - TutorialCompletion.NumberOfUnits) + " / " + TutorialCompletion.SummonedTyp.Count;

                    if (TutorialCompletion.NumberOfUnits == 0)
                    {
                        Invoke("Hide", LifeSpan.duration);
                        Debug.Log("Toturial Completed");
                    }
                }
            }

        }
        public void CheckingSoulDragging()
        {
            if (!Activated) { return; }
            if (TutorialCompletion.TutorialGoal == playerGameInputs.MovingSouls)
            {
                Hide();

                Debug.Log("Toturial CompletedDrag"  + this.gameObject.name);
            }

        }

        public void CheckingUnitSwaps()
        {
            if (!Activated) { return; }
            if (TutorialCompletion.TutorialGoal == playerGameInputs.RepositioningUnits)
            {
                Hide();

                Debug.Log("Toturial Completed");
            }
        }
        private void OnEnable()
        {

        }
        private void OnDisable()
        {


        }
        void Skip(InputAction.CallbackContext context)
        {
            if (Skippable)
            {
                Debug.Log("Skipped");
                Hide();
            }
        }
        public override void Show()
        {
            base.Show();
            animations.clip = animations.GetClip("ToturialInteractive");
            animations.Play();
            // Play Animation of InteractivePopUp
        }
        public override void Hide()
        {

            Activated = false;
            Debug.Log("Hide init");
            animations.clip = GetComponent<Animation>().GetClip("ClosingTutorialPopUp");
            animations.Play();
            // starting fading it out Animation
        }

        private void CompletingTheToturial()
        {

            // called by animation event
            TM.ActivatingNextPopUp();
            if(Skippable && TargetofInteraction)
            {
                TargetofInteraction.gameObject.SetActive(false);
            }
            base.Hide();
            
        }

    }
}
