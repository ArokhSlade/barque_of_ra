using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public UnityEvent<int> HealthUpdated;
    public UnityEvent HealthExhausted;

    [SerializeField] Image HealthBar;

    [SerializeField] TextMeshProUGUI HealthCountDisplayer;
    [SerializeField] TextMeshProUGUI MaxHealthDisplayer;

    [SerializeField] int maxHealth = 10;
    [SerializeField] int currentHealth;
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
 


    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        currentHealth = maxHealth;

        UpdateUI();
    }
    public void UpdateUI()
    {
        if (HealthBar)
        {
            HealthBar.fillAmount = (float)CurrentHealth / maxHealth;
           
        }
        if (HealthCountDisplayer && MaxHealthDisplayer)
        {
            HealthCountDisplayer.text = currentHealth.ToString();
            MaxHealthDisplayer.text = maxHealth.ToString();
        }

    }

    public void RemoveHealth(int amount)
    {
        Func<int, int, int> subtraction = (x, y) => x - y;

        ModifyHealth(amount, subtraction);
        
    }

    public void AddHealth(int amount)
    {
        Func<int, int, int> addition = (x, y) => x + y;

        ModifyHealth(amount, addition);

    }

    void ModifyHealth(int amount, Func<int, int, int> operation)
    {
        int newHealth = operation(CurrentHealth, amount);

        if (newHealth <= 0)
        {
            HealthExhausted?.Invoke();
        }

        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);

        HealthUpdated?.Invoke(newHealth);
      

    }
}
