using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyRanged : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] CombatantDetectorByReach combatantDetector;
    [SerializeField] int rangedAttackDamage;
    [SerializeField] float rangedAttackCooldownTime;

    Health health;
    float remainingRangedAttackCooldownTime;
    public Transform Target { get; private set; }

    void Start()
    {
        health = GetComponent<Health>();

        SubscribeToHealthExhausted();

        if (combatantDetector == null) Debug.LogError($"{gameObject} has no CombatantDetectorByReach set!");
    }

    void FixedUpdate()
    {
        if (remainingRangedAttackCooldownTime > 0) remainingRangedAttackCooldownTime -= Time.deltaTime;

        if (combatantDetector.GetDamageTakersInReach().Count > 0)
        {
            if (remainingRangedAttackCooldownTime > 0) return;

            foreach (DamageTaker damageTaker in combatantDetector.GetDamageTakersInReach())
            {
                // Only target DamageTaker that does not have the same tag as this gameObject, if a tag is set on the latter
                if (!gameObject.CompareTag("Untagged")
                && gameObject.CompareTag(damageTaker.tag)) continue;

                // TODO-CS: Select only the closest DamageTaker with line of sight

                GameObject spawnedObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform.parent);

                if (!spawnedObject.TryGetComponent(out Projectile projectile))
                {
                    Debug.LogError($"{spawnedObject} does not provide a Projectile component!");
                    return;
                }

                projectile.Init(damageTaker.transform);

                remainingRangedAttackCooldownTime = rangedAttackCooldownTime;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Target == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, Target.transform.position);
    }

    void OnDestroy()
    {
        UnsubscribeFromHealthExhausted();
    }

    void OnHealthExhausted()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    void SubscribeToHealthExhausted()
    {
        health.HealthExhausted.AddListener(OnHealthExhausted);
    }

    void UnsubscribeFromHealthExhausted()
    {
        health.HealthExhausted.AddListener(OnHealthExhausted);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
}
