using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BarqueOfRa.Units;
using BarqueOfRa.Game.UI;
using BarqueOfRa.Game;

[RequireComponent(typeof(Health))]
public class SoulCabin : MonoBehaviour
{
    public UnityEvent<int> AliveSoulsUpdated;

    [SerializeField] Transform soulsContainer;
    [SerializeField] PlayerCombatInput playerInput;
    [SerializeField] int guardianSoulCost = 1;
    public int CurrentHealth => health.CurrentHealth;

    List<Soul> soulsOnBoard = new();
    Health health;
    int healthValuePerSoul;

    public int SoulsLeft => healthValuePerSoul != 0 ? health.CurrentHealth / healthValuePerSoul : 0;

    void Awake()
    {
        health = GetComponent<Health>();
        health.Initialize();
        health.HealthUpdated.AddListener(OnHealthUpdated);

    }

    void Start()
    {
        playerInput = InGame.Instance.PlayerInput;
        playerInput.UnitSummoned.AddListener(OnUnitSummoned);

        soulsOnBoard.AddRange(soulsContainer.GetComponentsInChildren<Soul>());
        healthValuePerSoul = Mathf.CeilToInt((float)health.CurrentHealth / soulsOnBoard.Count);
    }

    void OnDestroy()
    {
        playerInput?.UnitSummoned.RemoveListener(OnUnitSummoned);

        health.HealthUpdated.RemoveListener(OnHealthUpdated);
    }

    public void AddSouls(int soulAmount)
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

    void OnHealthUpdated(int newHealth)
    {
        UpdateAliveSouls();
        AliveSoulsUpdated?.Invoke(newHealth);
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
}
