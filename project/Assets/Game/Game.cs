using BarqueOfRa.Game.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace BarqueOfRa.Game
{   
    public class Game : MonoBehaviour
    {
        bool hardModeUnlocked = false;
        public bool HardModeUnlocked => hardModeUnlocked;

        [SerializeField] AudioListener menuAudioListener;
        [SerializeField] Camera menuCamera;

        [SerializeField] InGame inGame;
        [SerializeField] Transform loadingScreen;
        [SerializeField] MainMenu mainMenu;

        private void Awake()
        {
            menuAudioListener = GetComponent<AudioListener>();
        }

        void Start() 
        {
            UI.UI.Instance.ShowMainMenuBackground();
            LoadPlayerData();
            LoadMainMenu();
        }
        
        public void Quit()
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        
        public void StartGame()
        {
            UI.UI.Instance.HideMainMenuBackground();
            inGame.SelectTutorialLevel();

            inGame.ChangePlanningTimeToNormal();

            menuAudioListener.enabled = false;
            menuCamera.enabled = false;
            inGame.Load();
        }        

        public void StartHardMode()
        {
            UI.UI.Instance.HideMainMenuBackground();
            inGame.SelectHardModeLevel();


            menuAudioListener.enabled = false;
            menuCamera.enabled = false;
            inGame.ChangePlanningTimeToHard();
            inGame.Load();
        }        

        public void AbortSession()
        {
            inGame.Unload();
            menuAudioListener.enabled = true;
            menuCamera.enabled = true;
            LoadMainMenu();
        }

        void LoadMainMenu()
        {
            mainMenu.Setup(hardModeUnlocked);
            mainMenu.Open();
        }

        public void UnlockHardMode()
        {
            hardModeUnlocked = true;
            if(HardModeUnlocked)
            {
                inGame.ChangePlanningTimeToHard();

            }
            SavePlayerData();
        }

        void SavePlayerData()
        {
            PlayerData data = new PlayerData(hardModeUnlocked);
            WritePlayerData(data);
        }

        void LoadPlayerData()
        {
            PlayerData data = ReadPlayerData();
            hardModeUnlocked = data.hardModeUnlocked;
        }

        public static string playerDataFileName = "player_data.dat";

        public PlayerData ReadPlayerData()
        {
            string path = Application.persistentDataPath + "/" + playerDataFileName;

            bool hardModeUnlocked = false;
            if (File.Exists(path))
            {
                using (var stream = File.Open(path, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        hardModeUnlocked = reader.ReadBoolean();
                    }
                }
            }
            else
            {

                Debug.LogWarning("Save file not found in " + path);
            }
            PlayerData data = new PlayerData(hardModeUnlocked);

            return data;
        }

        public void WritePlayerData(PlayerData data)
        {
            string path = Application.persistentDataPath + "/" + playerDataFileName;

            using (var stream = File.Open(path, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    writer.Write(data.hardModeUnlocked);
                }
            }
        }

        [System.Serializable]
        public struct PlayerData
        {
            public bool hardModeUnlocked;

            public PlayerData(bool hardModeUnlocked)
            {
                this.hardModeUnlocked = hardModeUnlocked;
            }
        }
    }    
}
