using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BarqueOfRa.Game.Units.Enemies;

namespace BarqueOfRa.Experimental.Units.Enemies
{
    using EnemyMeleeState_ = UnitState_<EnemyMelee_, EnemyMelee_.StateID>;

    [RequireComponent(typeof(NavMeshAgent))]    
    [RequireComponent(typeof(Health))]
    public class EnemyMelee_ : Enemy, AttackStateOwner_, ApproachStateOwner_, AttackBarqueStateOwner_
    {        
        public enum StateID
        {
            None,
            Idle,
            Approach,
            AttackGuardian,
            AttackBarque
        }

        [SerializeField] int attackDamage;
        [SerializeField] float attackSeconds;
        [SerializeField] float attackRange;
        [SerializeField] float detectionRange;
        [SerializeField] LayerMask combatTargetsMask;
        
        [SerializeField] Animator animator;
        [SerializeField] List<EnemyMeleeState_> statesList;

        [SerializeField] public Transform Target;

        [SerializeField] Barque barque; //NOTE(Gerald, 2025 07 25): set in Initialize(), e.g. by spawner
        [SerializeField] EnemyMeleeState_ state;
        
        NavMeshAgent navMeshAgent;
        Health health;
        Dictionary<StateID, EnemyMeleeState_> states = new();

                
        //TODO(Gerald 2025 07 26) rethink where this belongs. it's connected to atack guardian state or not?
        Guardian closestGuardian = null;

        //AttackGuardian State variables
        //TODO(Gerald 2025 07 25) move this out into the attackguardian state. and replace with a public method / property that get's called from there
        Guardian targetGuardian;

        public AttackStateOwner_.InitData AttackStateInitData => new() { 
            attackSeconds = attackSeconds, 
            attackDamage = attackDamage, 
            damageTaker = targetGuardian?.GetComponent<DamageTaker>() 
        };
        
        public AttackBarqueStateOwner_.InitData AttackBarqueStateInitData => new() { 
            barque = barque, 
            navMeshAgent = navMeshAgent
        };
        
        public ApproachStateOwner_.InitData ApproachStateInitData => new() { 
            navMeshAgent = navMeshAgent
        };

        public Barque Barque => barque;
        public Guardian TargetGuardian => targetGuardian;
        public float AttackCooldown => attackSeconds;
        public int AttackDamage => attackDamage;
        public float AttackRange => attackRange;
        

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

            health = GetComponent<Health>();
            SubscribeToHealthExhausted();

            if (animator == null)
            {
                Debug.LogError($"{gameObject} has no Animator set!");
            }
        }
        
        public void Initialize(Transform target, Barque barque)
        {
            SetTarget(target);
            this.barque = barque;
        }

        void Start()
        {
            foreach (var state in statesList)
            {
                states.Add(state.ID, state);
            }
            state = states[StateID.Idle];
            
        }

        void FixedUpdate()
        {
            UpdateClosestGuardian(detectionRange);
            state.OnStateUpdate(this);
        }

        void Update()
        {
            Think();
        }
        
        void OnDestroy()
        {
            UnsubscribeFromHealthExhausted();
        }
        
        // AttackBarqueState related
        public void OnBarqueHit()
        {
            gameObject.SetActive(false);
        }        
        
        // AttackGuardian related
        bool IsGuardianInRange() => closestGuardian != null;
        
        void UpdateClosestGuardian(float range)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, combatTargetsMask, QueryTriggerInteraction.Collide);
            List<Guardian> guardians = new();

            Guardian closestGuardian = null;
            float minDistanceSquared = float.MaxValue;

            foreach (var collider in colliders)
            {
                Guardian currentGuardian;
                if (collider.TryGetComponent(out currentGuardian))
                {
                    float currentDistanceSquared = (currentGuardian.transform.position - transform.position).sqrMagnitude;
                    if (currentDistanceSquared < minDistanceSquared)
                    {
                        minDistanceSquared = currentDistanceSquared;
                        closestGuardian = currentGuardian;
                    }
                }
            }
            this.closestGuardian = closestGuardian;
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
            //Debug.Log($"Set Target: {Target}");
        }

        //TODO(Gerald, 2025 07 22): implement. using a detector (trigger collider)
        bool HasReachedBoatAttackRegion()
        {
            return false;
        }

        bool IsInAttackRange(Transform other) => (other.position - transform.position).magnitude < attackRange;

        bool HasArrived()
        {
            bool hasArrived = true;
            hasArrived &= navMeshAgent != null;
            hasArrived &= !navMeshAgent.pathPending;

            hasArrived &= navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;

            float tolerance = 0.1f;
            hasArrived &= navMeshAgent.remainingDistance <= (navMeshAgent.stoppingDistance + tolerance);
            if (hasArrived)
            {
                //Debug.Log($"has arrived: {transform.position} -|-> {navMeshAgent.destination}");
            }
            return hasArrived;
            //return NavMeshAgentUtilities.HasReachedDestination(navMeshAgent);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (!TryGetComponent(out NavMeshAgent agent)) return;

            NavMeshPath path = agent.path;

            if (path.corners.Length < 2) return; // Need 2 corners to draw line

            for (int i = 1; i < path.corners.Length; i++)
            {
                Vector3 lastPosition = path.corners[i - 1];
                Vector3 currentPosition = path.corners[i];

                Gizmos.DrawLine(lastPosition, currentPosition);
            }
        }

        public void Think()
        {
            switch(state.ID)
            {
                case StateID.Idle:
                    if (IsGuardianInRange())
                    {
                        targetGuardian = closestGuardian;
                        Transition(StateID.AttackGuardian);
                        animator.SetTrigger("AttackGuardian"); 
                    }
                    else 
                    {
                        if (Target != null)
                        {
                            Transition(StateID.Approach);
                            animator.SetTrigger("Approach");
                        }
                        // NOTE(Gerald 2025 07 22): didn't realize we had TargetProviders.
                        // leaving this as a fallback for now.
                        else if (barque != null)
                        {
                            Debug.LogWarning("no Target set. trying to set barque as target.");
                            SetTarget(barque.transform);
                            Transition(StateID.Approach);
                            animator.SetTrigger("Approach");
                        }
                        else
                        {
                            Debug.LogWarning("no barque set.");
                        }
                    }
                    break;
                case StateID.Approach:
                    //TODO(Gerald, 2025 07 22): if (AttackedByGuardian) {...}
                    if (IsGuardianInRange())
                    {
                        targetGuardian = closestGuardian; //TODO(Gerald 2025 07 25): this should happen inside attack guardian states enter method
                        Transition(StateID.AttackGuardian);
                        animator.SetTrigger("AttackGuardian");
                    } 
                    else if (HasArrived())
                    {
                        Transition(StateID.AttackBarque);
                        
                        animator.SetTrigger("Idle"); //NOTE(Gerald, 2025 07 22): cannot happen for boat on water. so we probably arrived near target.
                    }
                    else if (HasReachedBoatAttackRegion())
                    {
                        // Transition(StateID.Transforming);
                        // TODO(Gerald, 2025 07 22) animator.animator.SetTrigger("Transform");
                    }
                    break;
                case StateID.AttackGuardian:
                    if (targetGuardian == null)
                    {
                        Transition(StateID.Approach);
                        animator.SetTrigger("Approach");
                    }
                    else if (!IsInAttackRange(targetGuardian.transform))
                    {
                        Transition(StateID.Approach);
                        animator.SetTrigger("Approach");
                    }
                    break;
                case StateID.AttackBarque:
                    break;
                default:
                    Debug.LogError("unexpected state in enemy melee!");
                    break;
            }
        }
        
        void Transition(StateID stateID)
        {
            state.OnStateExit(this);
            var oldState = state.ID;
            state = states[stateID];
            state.OnStateEnter(this);
            //Debug.Log($"{oldState} --> {thinkState.stateID}");
        }
    }
}
