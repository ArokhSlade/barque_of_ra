using BarqueOfRa.Game.Units.Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace BarqueOfRa.Game.Interactables
{
    public class BattlePylon : MonoBehaviour
    {
        List<EnemyMelee> enemyMelees = new();

        [SerializeField] GameObject barrierVFX;

        [HideInInspector]
        public Waypoint AssociatedWaypoint{get; set;}

        bool spawnTimeWindowClosed = false;        
        
        bool allEnemyMeleesDead()
        {
            if (enemyMelees.Count == 0)
            {
                return true;
            }

            foreach (EnemyMelee enemyMelee in enemyMelees)
            {
                if (!enemyMelee.IsDead)
                {
                    return false;
                }
            }
            return true;
        }


        private void OnBattleFinished()
        {
            DisableBarrierVFX();
            AssociatedWaypoint.OnBattleFinished();
        }

        public void OnSpawnTimeWindowClosed()
        {
            spawnTimeWindowClosed = true;
        }

        public void AddSpawnedEnemy(EnemyMelee enemyMelee)
        {
            enemyMelee.Died.AddListener(OnEnemyDied);
            enemyMelees.Add(enemyMelee);
        }

        public void OnEnemyDied(EnemyMelee enemyMelee)
        {
            enemyMelees.Remove(enemyMelee);
            enemyMelee.Died.RemoveListener(OnEnemyDied);
            
            if (spawnTimeWindowClosed && allEnemyMeleesDead())
            {
                OnBattleFinished();
            }
        }

        void DisableBarrierVFX()
        {
            barrierVFX.SetActive(false);

        }
    }

}
