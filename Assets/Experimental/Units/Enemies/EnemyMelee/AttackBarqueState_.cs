using UnityEngine;
using UnityEngine.AI;
using BarqueOfRa.Game.Units.Enemies;

namespace BarqueOfRa.Experimental.Units.Enemies
{
    public class AttackBarqueState_ : UnitState_<EnemyMelee_, EnemyMelee_.StateID>
    {
        override public EnemyMelee_.StateID ID => EnemyMelee_.StateID.AttackBarque;
                
        /// <summary>
        /// make sure the collider has the correct layer overrides set!
        /// </summary>
        [SerializeField] Collider hitBox;
		[SerializeField] float speed = 20f;
        [SerializeField] int damage = 3;
        Barque barque;
		Vector3 target;
        NavMeshAgent navMeshAgent;
        EnemyMelee_ enemyMelee;

        override public void OnStateEnter(EnemyMelee_ enemyMelee)
        {
            this.enemyMelee = enemyMelee; //NOTE(Gerald 2025 07 27): needed in OnTriggerEnter()
            
            AttackBarqueStateOwner_.InitData initData = enemyMelee.AttackBarqueStateInitData;
            
            barque = initData.barque;
            Debug.Assert(barque != null, "barque was null");
            
            navMeshAgent = initData.navMeshAgent;
            Debug.Assert(navMeshAgent != null, "navMeshAgent was null");
            
			target = barque.transform.position;
            hitBox.enabled = true;
            navMeshAgent.enabled = false;
        }

        override public void OnStateUpdate(EnemyMelee_ enemyMelee)
        {   
			float distanceRemaining = (target - enemyMelee.transform.position).magnitude;
			Vector3 direction = (target - enemyMelee.transform.position).normalized;
			enemyMelee.transform.position += direction * speed * Time.deltaTime;
        }
        
        override public void OnStateExit(EnemyMelee_ enemyMelee)
        {
            hitBox.enabled = false;
        }

        void OnTriggerEnter(Collider other)
        {
            DamageTaker barqueDamageTaker;

            if (other.CompareTag("Boat") && other.TryGetComponent(out barqueDamageTaker))
            {
                Debug.Log("barque hit!");
                barqueDamageTaker.TakeDamage(damage);
                enemyMelee.OnBarqueHit();
            }
        }
    }
}