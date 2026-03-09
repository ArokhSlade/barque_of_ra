using System;
using System.Collections.Generic;
using UnityEngine;
using BarqueOfRa;
using BarqueOfRa.Units;


namespace BarqueOfRa.Test.BalanceCurveTest
{
    [RequireComponent(typeof(Health))]
    public class SoulCabin_BalanceCurveTest : MonoBehaviour
    {
        [SerializeField] Transform soulsContainer;
        [SerializeField] BarqueOfRa.Test.BalanceCurveTest.PlayerInput playerInput;
        [SerializeField] int guardianSoulCost = 1;
        public int CurrentHealth => health.CurrentHealth;

        List<Soul> soulsOnBoard = new();
        Health health;
        int healthValuePerSoul;

        public int SoulsLeft => healthValuePerSoul != 0 ? health.CurrentHealth / healthValuePerSoul : 0;

        public static Action<int> OnSoulAdd;

        void Start()
        {
            if (playerInput == null)
            {
                Debug.LogError($"boat: Player Input not assigend");
            }
            else
            { 
                playerInput.unitSummoned.AddListener(OnUnitSummoned);
            }

            health = GetComponent<Health>();
            health.Initialize();
            health.HealthUpdated.AddListener(OnHealthUpdated);
            health.HealthExhausted.AddListener(OnHealthExhausted);

            soulsOnBoard.AddRange(soulsContainer.GetComponentsInChildren<Soul>());
            healthValuePerSoul = Mathf.CeilToInt((float)health.CurrentHealth / soulsOnBoard.Count);

            OnSoulAdd += AddSouls;
        }

        void OnDestroy()
        {
            playerInput?.unitSummoned.RemoveListener(OnUnitSummoned);

            health.HealthUpdated.RemoveListener(OnHealthUpdated);
            health.HealthExhausted.RemoveListener(OnHealthExhausted);
        }

        void AddSouls(int soulAmount)
        {
            int remainingSouls = soulAmount;
            int addedSouls = 0;

            for (int i = 0; i < soulAmount; i++)
            {
                if (i >= soulsOnBoard.Count)
                {
                    continue;
                }

                for (int j = 0; j < soulsOnBoard.Count; j++)
                {
                    if (remainingSouls > 0)
                    {
                        if (!soulsOnBoard[j].Alive)
                        {
                            soulsOnBoard[j].Alive = true;
                            remainingSouls--;
                            addedSouls++;
                        }
                    }

                    else
                    {
                        continue;
                    }

                }
            }
            health.AddHealth(addedSouls);
        }

        void OnHealthUpdated(int health)
        {
            UpdateAliveSouls();
        }

        void OnHealthExhausted()
        {
            LoseGame();
        }

        void OnUnitSummoned()
        {
            health.RemoveHealth(guardianSoulCost * healthValuePerSoul);
            Debug.Log($"On Unit Summoned: health is now {health.CurrentHealth}");
        }

        void UpdateAliveSouls()
        {
            int totalCapacityIndex = soulsOnBoard.Count - 1;
            int capacityOfAliveSouls = Mathf.CeilToInt((float)CurrentHealth / healthValuePerSoul);
            int cutoffIndexForAliveSouls = soulsOnBoard.Count - capacityOfAliveSouls;

            for (int i = totalCapacityIndex; i >= 0; i--)
            {
                if (i >= cutoffIndexForAliveSouls)
                {
                    soulsOnBoard[i].Alive = true;
                }
                else
                {
                    soulsOnBoard[i].Alive = false;
                }
            }
        }

        void LoseGame()
        {
            Debug.Log("You Lost the Game!");
        }
    }
}
