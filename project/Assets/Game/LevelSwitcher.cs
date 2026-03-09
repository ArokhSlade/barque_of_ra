using System.Collections.Generic;
using UnityEngine;

namespace BarqueOfRa.Game
{ 
    public class LevelSwitcher: MonoBehaviour
    {

        public int TutorialLevelIndex = 0;
        public int HardModeLevelIndex = 1;
        int maxLevelIndex = 1;

        [SerializeField] List<GameObject> levels;
        [SerializeField] Transform levelParent;

        int levelCounter = 0;

        public int LevelCounter
        {
            get
            {
                return levelCounter;
            }
            set
            {
                if (value < 0 || value > maxLevelIndex)
                {
                    Debug.LogError($"error, invalid level index requested: {value}");
                    return;
                }
                levelCounter = value;
            }
        }

        public Level currentLevel;
        public Level CurrentLevel => currentLevel;

        public void IncrementLevelCounter()
        {
            if (levels.Count == 0)
            {
                Debug.LogError("no levels available");
                return;
            }
            levelCounter++;
            levelCounter %= levels.Count;
        }

        public Level LoadLevel()
        {
            if (currentLevel != null)
            {
                Debug.LogError("requested LoadLevel but there is already a level loaded. aborting.");
                return null;
            }

            GameObject newLevel = Instantiate(levels[levelCounter], levelParent);
            currentLevel = newLevel.GetComponent<Level>();
            return currentLevel;
        }

        public void UnloadLevel()
        {
            if (currentLevel == null)
            {
                Debug.LogError("Unload level requested but there is no level to unload. aborting.");
                return;
            }
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }
    }
}
