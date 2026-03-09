using UnityEngine;
using UnityEngine.AI;

namespace BarqueOfRa.Game.Units.Enemies.Melee
{
    public class AttackBarqueState : EnemyMeleeState
    {
        bool initialized = false;

        /// <summary>
        /// make sure the collider has the correct layer overrides set!
        /// </summary>
        [SerializeField] Collider hitBox;
        [SerializeField] float speed = 20f;
        [SerializeField] int damage = 3;
        [SerializeField] EffectBundle dealBarqueDamageEffectBundle;

        Vector3 target;
        NavMeshAgent navMeshAgent;
        EnemyMelee enemy;

        void Awake()
        {
            stateID = StateID.AttackBarque;
        }

        override public void OnStateEnter(EnemyMelee enemy)
        {
            this.enemy = enemy;
            target = enemy.Barque.transform.position;
            enemy.TryGetComponent(out navMeshAgent);

            hitBox.enabled = true;
            navMeshAgent.enabled = false;

            initialized = true;

            if (dealBarqueDamageEffectBundle != null && dealBarqueDamageEffectBundle.TryGetComponent(out EffectBundle effectBundle))
            {
                effectBundle.Play();
            }
        }

        override public void OnStateUpdate(EnemyMelee enemy)
        {
            if (!initialized)
            {
                Debug.LogError($"{this} not initialized!");
                return;
            }

            float distanceRemaining = (target - enemy.transform.position).magnitude;
            Vector3 direction = (target - enemy.transform.position).normalized;
            enemy.transform.position += direction * speed * Time.deltaTime;
        }

        override public void OnStateExit(EnemyMelee enemy)
        {
            hitBox.enabled = false;
            initialized = false;
        }

        void OnTriggerEnter(Collider other)
        {
            DamageTaker barqueDamageTaker;

            if (other.CompareTag("Boat") && other.TryGetComponent(out barqueDamageTaker))
            {
                barqueDamageTaker.TakeDamage(damage);
                enemy.OnBarqueHit();
            }
        }
    }
}
