using BarqueOfRa.Game;
using BarqueOfRa.Game.UI;
using UnityEngine;


public class HUD: MonoBehaviour
{
    public static HUD Instance
    {
        get 
        {
            if (s_instance == null) 
            {
                if (destroyed)
                {
                    Debug.LogWarning("no HUD found -> probably okay because it's been destroyed already.");
                }
                else
                {
                    Debug.LogError("no HUD found but it hasn't been destroyed either.");
                }

            }
            return s_instance;
        }
    }
    static bool destroyed = false;
    
    private static HUD s_instance;
    
    [SerializeField] InGame inGame;
    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] BattleStartScreen battleStartScreen;
    [SerializeField] BattleWonScreen battleWonScreen;
    [SerializeField] LevelWonScreen levelWonScreen;
    [SerializeField] BarqueHealthUI barqueHealthUI;
    [SerializeField] UnitScalingBuffUI unitScalingBuffUI;
    [SerializeField] HealthbarsHUD healthbarsHUD;

    public HealthbarsHUD HealthbarsHUD => healthbarsHUD;

    void Awake()
    {
        s_instance = this;
    }    

    public void ShowGameOverScreen()
    {
        gameOverScreen.Open();
    }

    //TODO(Gerald 2025 08 07) delete this.
    public void OpenPauseMenu()
    {
        //pauseMenu.Open();
    }

    public void ShowBattleStartScreen()
    {
        battleStartScreen.Open();
    }

    public void ShowBattleWonScreen()
    {
        battleWonScreen.Open();
    }

    public void OnLevelCompleted(int NumberOfSouls,string currentLevel)
    {
        levelWonScreen.LoadText(NumberOfSouls,currentLevel);
        levelWonScreen.Open();
    }

    public void OnLevelWonConfirmed()
    {
        inGame.OnLevelWon();
    }

    public void OnHardModeRequested()
    {
        inGame.OnHardModeRequested();
    }

    public void UpdateBarqueHealthUI(int newBarqueHP)
    {
        barqueHealthUI.UpdateHP(newBarqueHP);
    }
    
    public void UpdateUnitScalingBuffUI(float powerBonusRatio)
    {
        unitScalingBuffUI.UpdateBuffLabel(powerBonusRatio);
    }

    public void Reset()
    {
        gameOverScreen.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        battleStartScreen.gameObject.SetActive(false);
        battleWonScreen.gameObject.SetActive(false);
        levelWonScreen.gameObject.SetActive(false);
    }
    public void ResetingBattleWonScreenIndex()
    {
        battleWonScreen.ResetBattleWonIndex();
    }
    public void ReInizializeTimer(string TimerType)
    {
        battleStartScreen.Init(TimerType);
    }
    public void OnDestroy()
    {
        destroyed = true;
    }

}
