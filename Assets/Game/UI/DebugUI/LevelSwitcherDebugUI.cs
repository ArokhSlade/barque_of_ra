using TMPro;
using UnityEngine;

namespace BarqueOfRa.Game
{
    public class LevelSwitcherDebugUI : MonoBehaviour
    {
        [SerializeField] TMP_Text levelLabel;
        [SerializeField] LevelSwitcher levelSwitcher;
        [SerializeField] InGame inGame;

        private void Update()
        {
            UpdateUI();
        }
        public void UpdateUI()
        {
            int levelNumber = levelSwitcher.LevelCounter;
            levelLabel.text = "level: " + levelNumber;
        }

        public void DEBUG_SwitchToNextLevel()
        {
            inGame.Unload();
            inGame.Load();
        }
    }
}
