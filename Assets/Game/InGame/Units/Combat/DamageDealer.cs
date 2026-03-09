using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    List<DamageTaker> damageTakersInRange = new();

    public void RegisterDamageTaker(DamageTaker damageTaker)
    {
        damageTakersInRange.Add(damageTaker);
    }

    public void UnregisterDamageTaker(DamageTaker damageTaker)
    {
        damageTakersInRange.Remove(damageTaker);
    }

    public void DealDamage(int amount)
    {
        foreach (DamageTaker damageTaker in damageTakersInRange)
        {
            damageTaker.TakeDamage(amount);
        }
    }
}
