using UnityEngine;
using UnityEngine.AI;

namespace BarqueOfRa.Experimental.Units.Enemies
{
    public class ApproachState_ : UnitState_<EnemyMelee_, EnemyMelee_.StateID>
    {
        override public EnemyMelee_.StateID ID => EnemyMelee_.StateID.Approach;
        
        NavMeshAgent navMeshAgent;

        [SerializeField] float repathSeconds = .5f;
        float repathSecondsRemaining;
        
        
        override public void OnStateEnter(EnemyMelee_ enemyMelee)
        {
            base.OnStateEnter(enemyMelee);
            
            Debug.Assert(repathSeconds > 0f, "repath Seconds must be greater than 0!");
            
            ApproachStateOwner_.InitData initData = enemyMelee.ApproachStateInitData;
            
            navMeshAgent = initData.navMeshAgent;
            Debug.Assert(navMeshAgent != null, "navMeshAgent was null");
            
            navMeshAgent.destination = enemyMelee.Target.position;
            repathSecondsRemaining = repathSeconds;
        }

        override public void OnStateUpdate(EnemyMelee_ enemyMelee)
        {
            base.OnStateUpdate(enemyMelee);
                
            //TODO(Gerald 2025 07 27) don't reach into owner, have owner provide it -> ApproachStateOwner interface
            if (enemyMelee.Target == null) 
            { return; }
                
            repathSecondsRemaining -= Time.deltaTime;
            if (repathSecondsRemaining > 0f) 
            {
                return;
            }

            navMeshAgent.destination = enemyMelee.Target.position;
            repathSecondsRemaining = repathSeconds;            
        }
            
        override public void OnStateExit(EnemyMelee_ enemyMelee)
        {
            base.OnStateExit(enemyMelee);

            navMeshAgent.ResetPath();
        }        
    }
}