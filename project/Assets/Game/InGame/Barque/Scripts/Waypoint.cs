using BarqueOfRa.Game;
using BarqueOfRa.Game.Interactables;
using BarqueOfRa.Game.Units;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Action OnBarqueStopEnded;

    public Action OnBarqueStopStarted;

    [SerializeField] private bool isBossBattle = false;

    [SerializeField] private bool isBarqueStop = false;

    [SerializeField] private bool setBarqueSpeed = false;

    [SerializeField] private float barqueSpeed = 8f;

    [SerializeField] private Navigator barqueNavigator;

    [SerializeField] private float stopTime = 3f;

    [SerializeField] private Color gizmoColor = Color.white;

    [SerializeField] private float debugDrawRadius = 5.0f;
    
    [SerializeField] BattleWonScreen battleWonScreen;

    [SerializeField] UnitScalingBuffUI UnitScalingDisplayer;

    [SerializeField] bool WarnPlayer;

    [SerializeField] BattlePylon associatedPylon;
    public BattlePylon AssociatedPylon => associatedPylon;

    private bool hasController = false;
    private string battleTheme;

    

    private void Awake()
    {
        //NOTE(Gerald 2025 07 28): if it's a barque stop, it needs to have a pylon
        Debug.Assert(!isBarqueStop || associatedPylon != null, "No Pylon associated with this Waypoint");

        Collider collider = GetComponent<Collider>();
        if (collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
        }

        OnBarqueStopStarted += OnPlanningStarted;        

        if (associatedPylon != null)
        {
            associatedPylon.AssociatedWaypoint = this;
        }

        //TODO(Gerald 2025 08 11): clean up audio spaghtetti, pay the tech debt.
        battleTheme = isBossBattle ? "boss_battle_theme" : "battle_theme";
    }

    private void Start()
    {
        OnBarqueStopStarted += InGame.Instance.Barque.DefenseSlots.EnablingUnitSlot;
        UnitScalingDisplayer = FindAnyObjectByType<UnitScalingBuffUI>();
        UnitScalingDisplayer.ShowingBuffScale = false;

    }

    private void OnDestroy()
    {
        if (InGame.Instance.Barque != null && InGame.Instance.Barque.DefenseSlots != null)
        {
            OnBarqueStopStarted -= InGame.Instance.Barque.DefenseSlots.EnablingUnitSlot;
            OnBarqueStopStarted -= OnPlanningStarted;
        }
    }
    public void ActivateUnitScalingDisplayer()
    {

    }
    private void AssureBarqueController(GameObject passingObject)
    {
        if (barqueNavigator == null)
        {
            barqueNavigator = passingObject.GetComponent<Navigator>();
            if (barqueNavigator != null)
            {
                hasController = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;


        Gizmos.DrawSphere(transform.position, debugDrawRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boat"))
        {
            AssureBarqueController(other.gameObject);
            if (isBarqueStop && hasController)
            {

                OnBarqueStopStarted?.Invoke();
                barqueNavigator.CanMove = false;                
                AudioManager.Instance.PlaySound("boat_still");
                AudioManager.Instance.StopSoundEffect("boat_moves");
                StartCoroutine(WaitStopTime());
            }

            if (hasController && setBarqueSpeed)
            {
                barqueNavigator.SetBarqueSpeed(barqueSpeed);
            }
        }
    }

    private void OnPlanningStarted()
    {

        // re enable the unit slots 

        // play sound effect 
        UnitScalingDisplayer.ShowingBuffScale = true;

        HUD.Instance.ShowBattleStartScreen();
        AudioManager.Instance.PauseMusic("sailing_theme");
        
        AudioManager.Instance.PlayMusic(battleTheme);

        // put up the UI counter 

        // play sound effect 
    }
   
    private IEnumerator WaitStopTime()
    {

        yield return new WaitForSecondsRealtime(stopTime);
        if (hasController)
        {
            AudioManager.Instance.PlaySound("boat_moves");
            AudioManager.Instance.StopSoundEffect("boat_still");            
            OnBarqueStopEnded?.Invoke();
            associatedPylon.OnSpawnTimeWindowClosed();
        }
    }

    public void OnBattleFinished()
    {

        AudioManager.Instance.StopMusic(battleTheme);
        //TODO: play battle won / gate reached theme

        CelebrationTrigger.Trigger?.Invoke();

        UnitScalingDisplayer.ShowingBuffScale = false;



        Invoke("OpenWinningPopUp", 1f);
    }
    void OpenWinningPopUp()
    {
        AudioManager.Instance.PlaySound("battle_won");
        HUD.Instance.ShowBattleWonScreen();
        Invoke("SailingStarts", 5);
    }
    void SailingStarts()
    {
        barqueNavigator.CanMove = true;        
        InGame.Instance.Barque.DefenseSlots.DisablingUnitSlots();
        AudioManager.Instance.UnpauseMusic("sailing_theme");
    }
}
