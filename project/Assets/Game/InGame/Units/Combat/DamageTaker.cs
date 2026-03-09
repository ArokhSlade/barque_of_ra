using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageTaker : MonoBehaviour
{
    Health health;

    void Awake()
    { 
        health = GetComponent<Health>();
    }

    public void TakeDamage(int amount)
    {
        health.RemoveHealth(amount);
    }
}
