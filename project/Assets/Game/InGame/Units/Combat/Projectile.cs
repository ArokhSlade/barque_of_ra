using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int attackDamage;
    [SerializeField] CombatantDetectorByReach combatantDetector;

    Transform targetTransform;

    void Start()
    {
        if (combatantDetector == null) Debug.LogError($"{gameObject} has no CombatantDetector set!");
    }

    public void Init(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }

    void FixedUpdate()
    {
        if (combatantDetector.GetDamageTakersInReach().Count > 0)
        {
            foreach (DamageTaker damageTaker in combatantDetector.GetDamageTakersInReach())
            {
                damageTaker.TakeDamage(attackDamage);
            }

            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        if (targetTransform == null) return;

        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
    }
}
