using Tests.NavMeshTest;
using UnityEngine;
using UnityEngine.AI;

namespace BarqueOfRa.Game.Units.Enemies.Melee
{
    public class ApproachState : EnemyMeleeState
    {           
        NavMeshAgent navMeshAgent;
        bool initialized = false;

        public float repathSeconds = .5f;
        float repathSecondsRemaining;
        
        void Awake()
        {
            this.stateID = StateID.Approach;
        }
        
        override public void OnStateEnter(EnemyMelee enemyMelee)
        {
            base.OnStateEnter(enemyMelee);

            repathSecondsRemaining = repathSeconds;
            if (!enemyMelee.TryGetComponent(out enemyMelee))
            {
                Debug.LogError($"{enemyMelee.gameObject} does not have a EnemyMelee component!");
                return;
            }                
                
            if (!enemyMelee.TryGetComponent(out navMeshAgent))
            {
                Debug.LogError($"{enemyMelee.gameObject} does not provide a NavMeshAgent!");
                return;
            }

            navMeshAgent.destination = enemyMelee.Target.position;
            //Debug.LogWarning("navAgent Destination set");
            //Debug.Log($"navMeshAgent.destination = {navMeshAgent.destination}");
            //Debug.Log($"enemyMelee.Target.position = {enemyMelee.Target.position}");
            initialized = true;
        }

        override public void OnStateUpdate(EnemyMelee enemyMelee)
        {
            base.OnStateUpdate(enemyMelee);

            if (!initialized)
            {
                Debug.LogError("${this} not initialized!");
                return;
            }
                
                
            if (enemyMelee.Target == null) 
            { return; }
                
            repathSecondsRemaining -= Time.deltaTime;
            if (repathSecondsRemaining > 0f) 
            {
                return;
            }

            navMeshAgent.destination = enemyMelee.Target.position;
            //Debug.LogWarning("navAgent Destination set");
            //Debug.Log($"navMeshAgent.destination = {navMeshAgent.destination}");
            //Debug.Log($"enemyMelee.Target.position = {enemyMelee.Target.position}");
            repathSecondsRemaining = repathSeconds;            
        }
            
        override public void OnStateExit(EnemyMelee enemyMelee)
        {
            base.OnStateExit(enemyMelee);

            initialized = false;
            navMeshAgent.ResetPath();
            //Debug.LogWarning("path reset");
            //Debug.Log($"navMeshAgent.destination = {navMeshAgent.destination}");
        }
    }
}