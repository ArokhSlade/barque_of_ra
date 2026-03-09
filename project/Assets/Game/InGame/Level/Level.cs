using BarqueOfRa.Game.Interactables;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace BarqueOfRa.Game
{
    [RequireComponent(typeof(LevelData))]
    public class Level : MonoBehaviour
    {
        [SerializeField] Transform barqueSpawnPoint;
        [SerializeField] LevelEndPylon levelEndPylon;
        [SerializeField] Tutorial tutorial;
        [SerializeField] LevelData levelData;
        [SerializeField] string levelName;

        public string Name => levelName;


        public Transform BarqueSpawnPoint => barqueSpawnPoint;
        public LevelData LevelData => levelData;

        public List<Waypoint> BarqueWaypoints => levelData.BarqueWaypoints;

        void Awake()
        {
            if (levelData == null)
            {
                levelData = GetComponent<LevelData>();
            }
        }

        public void Setup(InGame inGame)
        {
            if (levelEndPylon == null)
            { 
                Debug.LogWarning($"no pylon set for this level: {this}");
            }
            levelEndPylon?.Setup(inGame);
            tutorial?.Setup(inGame);
        }

        

    }    
}
