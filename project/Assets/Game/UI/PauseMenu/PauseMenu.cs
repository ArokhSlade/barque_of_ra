using UnityEngine;
using UnityEngine.InputSystem;

namespace BarqueOfRa.Game.UI
{
    public class PauseMenu : FullScreenMenu
    {   
        [SerializeField] InGame inGame;

        bool isOpen = false;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Toggle()
        {
            if (isOpen)
            {
                CloseAllMenusAboveAndThis();
            }
            else
            {
                Open();
            }
        }

        void CloseAllMenusAboveAndThis()
        {
            while (isOpen)
            {
                UI.Instance.TopMenu.Close();
            }
        }


        public override void Open()
        {
            base.Open();
            UI.Instance.RequestPause(this);
            isOpen = true;
        }

        public override void Close()
        {
            isOpen = false;
            UI.Instance.RequestUnpause(this);
            base.Close();
        }

        public void OnResumeButtonPressed()
        {
            Debug.Log("OnResumeButtonPressed");
            Close();
        }

        public void OnSettingsButtonPressed()
        {
            Debug.Log("OnSettingsButtonPressed");
            UI.Instance?.OpenSettingsMenu();
        }

        public void OnRetryButtonPressed()
        {
            Debug.Log("OnRetryButtonPressed");
            inGame.ReloadLevel();
        }

        public void OnBackToMMButtonPressed()
        {
            Debug.Log("OnBackToMMButtonPressed");
            inGame.ReturnToMenu();
        }
    }    
}
