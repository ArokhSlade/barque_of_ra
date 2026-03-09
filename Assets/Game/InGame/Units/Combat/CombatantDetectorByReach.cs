using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CombatantDetectorByReach : MonoBehaviour
{
    [SerializeField] List<DamageTaker> damageTakersInReach = new();
    [SerializeField] LayerMask detectionLayers;

    SphereCollider sphereCollider;

    public float Range => sphereCollider.radius;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        // Check if ANY kind of component extending Collider is present.
        if (!TryGetComponent(out Collider collider)) Debug.LogError($"{gameObject} does not provide a Collider component!");

        collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out DamageTaker damageTaker)) return;

        Debug.Log($"DamageTaker came into reach! {damageTakersInReach}");
        damageTakersInReach.Add(damageTaker);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out DamageTaker damageTaker)) return;

        Debug.Log($"DamageTaker went out of reach! {damageTakersInReach}");
        damageTakersInReach.Remove(damageTaker);
    }

    public List<DamageTaker> GetDamageTakersInReach()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereCollider.radius, detectionLayers,QueryTriggerInteraction.Collide);
        List<DamageTaker> damageTakers = new();
        bool isDamageTakerInReach = false;
        foreach (var collider in colliders)
        {
            DamageTaker damageTaker = collider.GetComponent<DamageTaker>();
            if (damageTaker != null)
            {
                damageTakers.Add(damageTaker);
                isDamageTakerInReach = true;
            }
        }
        if (isDamageTakerInReach) 
        { 
            Debug.Log($"in reach: {damageTakers}"); 
        }
        return damageTakers;
    }
}
