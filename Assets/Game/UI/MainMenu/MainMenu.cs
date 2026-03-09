using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BarqueOfRa.Game.UI
{   
    public class MainMenu : FullScreenMenu
    {
        [SerializeField] Game game;
        [SerializeField] Transform buttons;
        [SerializeField] Popup confirmationPopup;
        [SerializeField] Popup difficultySelectPopup;
        [SerializeField] Button hardModeButton;
        [SerializeField] Color disabledColor;
        [SerializeField] TextMeshProUGUI hardModeLabel;

        public void Setup(bool hardModeVisible)
        {
            hardModeButton.interactable = hardModeVisible;            
            //if (!hardModeVisible)
            //{
            //    hardModeButton.transition = Selectable.Transition.None;
            //    hardModeLabel.color = disabledColor;
            //}
            //else
            //{
            //    hardModeButton.transition = Selectable.Transition.Animation;
            //    hardModeLabel.color = Color.white;
            //}
        }

        public override void Open()
        {
            base.Open();
            AudioManager.Instance.PlayMusic("main_theme");
        }

        public override void Close()
        {
            AudioManager.Instance.StopMusic("main_theme");
            base.Close();
        }

        public void OnStartButtonPressed()
        {
            difficultySelectPopup.Open();
        }

        public void OnNormalModeSelected()
        {
            Close();
            game?.StartGame();
        }

        public void OnHardModeSelected()
        {
            Close();
            game?.StartHardMode();
        }

        public void OnSettingsButtonPressed()
        {
            UI.Instance.OpenSettingsMenu();
        }

        public void OnCreditsButtonPressed()
        {
            UI.Instance.OpenCreditsScreen();
        }

        public void DisableButtons()
        {
            if (buttons == null)
            {
                Debug.LogError("buttons reference not assigned");
                return; 
            }

            foreach (Transform button in buttons)
            {
                button.GetComponent<Button>().interactable = false;
            }
        }

        public void EnableButtons()
        {
            if (buttons == null) 
            {
                Debug.LogError("buttons reference not assigned");
                return; 
            }

            foreach (Transform button in buttons)
            {
                button.GetComponent<Button>().interactable = true;
            }
        }

        public override void DisableInteraction()
        {
            DisableButtons();
        }

        public override void EnableInteraction()
        {
            EnableButtons();
        }

        public void OnQuitButtonPressed()
        {
            confirmationPopup.Open();
        }

        public void QuitGame()
        {
            game?.Quit();
        }
    }    
}
