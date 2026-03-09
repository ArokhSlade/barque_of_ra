using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageTakerCollider : MonoBehaviour
{
    DamageTaker damageTaker;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void Init(DamageTaker damageTaker)
    {
        this.damageTaker = damageTaker;
    }

    void OnTriggerEnter(Collider other)
    {
        if (damageTaker == null) Debug.LogWarning($"{this} attached to {transform.parent} was not initialized with a DamageTaker component!");

        if (!other.TryGetComponent(out DamageDealer damageDealer)) return;

        damageDealer.RegisterDamageTaker(damageTaker);
    }

    void OnTriggerExit(Collider other)
    {
        if (damageTaker == null) Debug.LogWarning($"{this} attached to {transform.parent} was not initialized with a DamageTaker component!");

        if (!other.TryGetComponent(out DamageDealer damageDealer)) return;

        damageDealer.UnregisterDamageTaker(damageTaker);
    }
}
