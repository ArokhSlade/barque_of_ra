using UnityEngine;
using BarqueOfRa.Game.UI;

namespace BarqueOfRa.Game
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] InGame inGame;
        
        public void Setup(InGame inGame)
        {
            this.inGame = inGame;
        }

        public void OpenPopup(TutorialPopup popup)
        {
            inGame.Pause();
            popup.Open();
        }
        
        public void ClosePopup(TutorialPopup popup)
        {
            popup.Close();
            inGame.Unpause();
        }
    }

}
