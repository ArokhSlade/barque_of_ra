using BarqueOfRa.Game.UI;
using Unity.Cinemachine;
using UnityEngine;

namespace BarqueOfRa.Game
{   
    public enum LaunchProfile
    {
        None = 0,
        LevelScene = 10,
        GameScene = 20
    }
    public class InGame : MonoBehaviour
    {
        public static InGame Instance => s_instance;
        public PlayerCombatInput PlayerInput => playerInput;
        public Barque Barque => barque;

        [SerializeField] GameObject barquePrefab;
        [SerializeField] CinemachineCamera cinemachineCamera;
        [SerializeField] Barque barque;
        [SerializeField] PlayerCombatInput playerInput;
        [SerializeField] Transform unitCatalogue;
        [SerializeField] HUD hud;
        [SerializeField] LevelSwitcher levelSwitcher;
        [SerializeField] Level level;
        [SerializeField] Game game;
        [SerializeField] LaunchProfile launchProfile = LaunchProfile.GameScene;

        static InGame s_instance;
        void Awake()
        {
            if (s_instance != null)
            {
                Debug.LogError("An Instance of InGame already exists");

            }
            s_instance = this;
            switch (launchProfile)
            {
                case LaunchProfile.GameScene:
                    if (game == null)
                    {
                        Debug.LogError("Launch Profile is Game Scene, but reference to Game is missing");
                    }
                    break;
                case LaunchProfile.LevelScene:
                    if (level == null)
                    {
                        Debug.LogError("Launch Profile is Level Scene, but reference to Level is missing");
                        return;
                    }
                    if (barque == null)
                    {
                        Debug.LogError("Launch Profile is Level Scene, but reference to Barque is missing");
                        return;
                    }
                    break;
                case LaunchProfile.None:
                default:
                    Debug.LogError("invalid Launch Profile");
                    break;
            }
        }

        void Start()
        {
            switch(launchProfile)
            {
                case LaunchProfile.GameScene:
                    break;
                case LaunchProfile.LevelScene:
                    barque.SoulCountChanged.AddListener(UpdateBarqueHealthUI);
                    barque.PowerBonusRatioChanged.AddListener(UpdateUnitScalingBuffUI);
                    UpdateBarqueHealthUI(barque.SoulCabin.CurrentHealth);
                    UpdateUnitScalingBuffUI(barque.PowerBonusRatio);
                    level.Setup(this);
                    cinemachineCamera.Follow = barque.transform;
                    barque.Initialize(level);
                    AudioManager.Instance.PlayMusic("sailing_theme");
                    AudioManager.Instance.PlaySound("boat_moves");
                    break;
                case LaunchProfile.None:
                default:
                    Debug.LogError("invalid Launch Profile");
                    break;
            }
        }

        public void ReloadLevel()
        {
            Unload();
            Load();
        }

        public void Unload()
        {
            gameObject.SetActive(false);
            UnloadLevel();
            UnloadBarque();
            UI.UI.Instance.Reset();
            AudioManager.Instance.StopMusic("boss_battle_theme");
            AudioManager.Instance.StopMusic("battle_theme");
            AudioManager.Instance.StopMusic("sailing_theme");
            AudioManager.Instance.StopSoundEffect("boat_moves");
        }

        public void Load()
        {
            gameObject.SetActive(true);
            //NOTE(Gerald 2025 08 10): level depends on barque, spawners cannot acquire target without barque.
            LoadBarque();
            LoadLevel();
            level.Setup(this);
            cinemachineCamera.Follow = barque.transform;
            barque.Initialize(level);
            AudioManager.Instance.PlayMusic("sailing_theme");
            AudioManager.Instance.PlaySound("boat_moves");

            hud.ResetingBattleWonScreenIndex();

        }

        void LoadBarque()
        {
            var barqueInstance = Instantiate(barquePrefab, transform);
            barque = barqueInstance.GetComponent<Barque>();
            barque.Died.AddListener(OnGameLost);
            UpdateBarqueHealthUI(barque.SoulCabin.CurrentHealth);
            barque.SoulCountChanged.AddListener(UpdateBarqueHealthUI);
            UpdateUnitScalingBuffUI(1f);
            barque.PowerBonusRatioChanged.AddListener(UpdateUnitScalingBuffUI);
        }
        
        void UnloadBarque()
        {
            barque.Died.RemoveListener(OnGameLost);
            barque.SoulCountChanged.RemoveListener(UpdateBarqueHealthUI);
            barque.PowerBonusRatioChanged.RemoveListener(UpdateUnitScalingBuffUI);
            Destroy(barque.gameObject);
            barque = null;
        }

        void UnloadLevel()
        {
            levelSwitcher.UnloadLevel();
            level = null;
        }

        void LoadLevel()
        {
            level = levelSwitcher.LoadLevel();
        }

        public void OnBarqueEnteredLevelEndPylon()
        {
            hud.OnLevelCompleted(barque.SoulCabin.CurrentHealth, level.Name);        
        }

        public void SelectTutorialLevel()
        {
            levelSwitcher.LevelCounter = levelSwitcher.TutorialLevelIndex;

        }

        public void SelectHardModeLevel()
        {
            levelSwitcher.LevelCounter = levelSwitcher.HardModeLevelIndex;
        }
        public void ChangePlanningTimeToHard()
        {
            hud.ReInizializeTimer("Hard");
        }
        public void ChangePlanningTimeToNormal()
        {
            hud.ReInizializeTimer("Normal");

        }
        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Unpause()
        {
            Time.timeScale = 1f;
        }

        public void OnGameLost()
        {
            HUD.Instance.ShowGameOverScreen();
        }

        public void UpdateBarqueHealthUI(int newHP)
        {
            HUD.Instance.UpdateBarqueHealthUI(newHP);
        }
        
        public void UpdateUnitScalingBuffUI(float powerBonusRatio)
        {
            HUD.Instance.UpdateUnitScalingBuffUI(powerBonusRatio);
        }

        public void OnLevelWon()
        {
            game?.UnlockHardMode();
            ReturnToMenu();
            UI.UI.Instance.OpenCreditsScreen();
        }

        public void OnHardModeRequested()
        {
            game?.UnlockHardMode();
            SelectHardModeLevel();
            ReloadLevel();
        }

        public void ReturnToMenu()
        {
            game?.AbortSession();
        }
    }    
}
