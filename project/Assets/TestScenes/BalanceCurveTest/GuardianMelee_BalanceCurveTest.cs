using UnityEngine;
using BarqueOfRa.Game.Units;
using System.Collections.Generic;
using BarqueOfRa.Game.Units.Enemies;
using UnityEngine.InputSystem;

namespace BarqueOfRa.Test.BalanceCurveTest
{
    [RequireComponent(typeof(Health))]
    public class GuardianMelee_BalanceCurveTest : Guardian, IUnitBrain, MeleeUnit
    {
        [SerializeField] AnimationCurve attackDamagePerDeploymentRatio;
        
        [SerializeField] int attackDamage;
        [SerializeField] float attackSeconds;
        [SerializeField] float attackRange;
        [SerializeField] float detectionRange;
        [SerializeField] LayerMask combatTargetsMask;

        //NOTE(Gerald 2025 08 02): readonly
        [SerializeField] int scaledAttackDamage;
        public int ScaledAttackDamage => scaledAttackDamage;

        [SerializeField] Animator animator;
        [SerializeField] List<GuardianMeleeState> statesList;
        [SerializeField] GuardianMeleeState state;

        [SerializeField] Barque_BalanceCurveTest barque;
        public Barque_BalanceCurveTest Barque => barque;

        [SerializeField] InputActionReference DEBUG_logScaledDamageActionRef;
        InputAction DEBUG_logScaledDamageAction;
        public bool DebugActionPressed => DEBUG_logScaledDamageAction.WasCompletedThisFrame();

        
        Health health;
        Dictionary<GuardianMeleeState.StateID, GuardianMeleeState> states = new();
        //float remainingMeleeAttackCooldownTime = 0;

        Enemy closestEnemy = null;
        Enemy targetEnemy;


        public MeleeUnit.InitData AttackStateInitData => new() { 
            attackSeconds = attackSeconds, 
            attackDamage = attackDamage, 
            damageTaker = targetEnemy?.GetComponent<DamageTaker>(), 
            barque = barque, 
            attackDamagePerDeploymentRatio = attackDamagePerDeploymentRatio
        };

        void Awake()
        {
            DEBUG_logScaledDamageAction = DEBUG_logScaledDamageActionRef;
            health = GetComponent<Health>();
            SubscribeToHealthExhausted();

            if (animator == null) { Debug.LogError($"{gameObject} has no Animator set!"); }

            animator.SetFloat("SpeedMulti", 1 / attackSeconds);
        }

        void Start()
        {
            if (barque == null)
            {
                barque = Barque_BalanceCurveTest.Instance;
            }

            foreach (var state in statesList)
            {
                states.Add(state.ID, state);
            }

            state = states[GuardianMeleeState.StateID.Idle];
        }

        void FixedUpdate()
        {
            UpdateDamageBasedOnSoulCount();
            UpdateClosestEnemy(detectionRange);
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

        void OnDrawGizmos()
        {
            DrawGizmoDetectionRange();
            DrawGizmoAttackRange();
        }

        private void DrawGizmoDetectionRange()
        {
            Gizmos.color = new Color(0, 1, 0, 0.2f);

            Gizmos.DrawSphere(transform.position, detectionRange);
        }

        private void DrawGizmoAttackRange()
        {
            Gizmos.color = new Color(1, 1, 0, 0.2f);

            Gizmos.DrawSphere(transform.position, attackRange);
        }

        bool IsEnemyInRange() => closestEnemy != null;

        bool IsInAttackRange(Transform other) => (other.position - transform.position).magnitude < attackRange;

        void UpdateClosestEnemy(float range)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, combatTargetsMask, QueryTriggerInteraction.Collide);
            List<Enemy> guardians = new();

            Enemy closestEnemy = null;
            float minDistanceSquared = float.MaxValue;

            foreach (var collider in colliders)
            {
                Enemy currentEnemy;
                if (collider.TryGetComponent(out currentEnemy))
                {
                    float currentDistanceSquared = (currentEnemy.transform.position - transform.position).sqrMagnitude;
                    if (currentDistanceSquared < minDistanceSquared)
                    {
                        minDistanceSquared = currentDistanceSquared;
                        closestEnemy = currentEnemy;
                    }
                }
            }
            this.closestEnemy = closestEnemy;
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

        void UpdateDamageBasedOnSoulCount()
        {
            int guardiansCount = barque.GuardiansCount;
            int soulsCount = barque.SoulsCount;
            int totalUnitsCount = guardiansCount + soulsCount;
            float soulsPerTotalRatio = (float)soulsCount / totalUnitsCount;
            float powerBonusPercentage = attackDamagePerDeploymentRatio.Evaluate(soulsPerTotalRatio);
            int oldScaledAttackDamge = scaledAttackDamage;

            scaledAttackDamage = attackDamage + (int)((float)attackDamage * powerBonusPercentage);
            if (scaledAttackDamage != oldScaledAttackDamge || DebugActionPressed)
            {
                Debug.Log($"{soulsCount} / ( {soulsCount} + {guardiansCount} ) = {soulsPerTotalRatio} -> 1.0 + {powerBonusPercentage} => {attackDamage} -> {scaledAttackDamage}");
            }
        }

        public void Think()
        {
            switch (state.ID)
            {
                case GuardianMeleeState.StateID.Idle:
                    if (IsEnemyInRange())
                    {
                        targetEnemy = closestEnemy;
                        #region Shady 
                        // remove line 141 if guardians can use navmesh again 
                        transform.LookAt(new Vector3(targetEnemy.transform.position.x, transform.position.y, targetEnemy.transform.position.z));

                        #endregion
                        Transition(GuardianMeleeState.StateID.Attack);
                        animator.SetTrigger("Attack");
                    }
                    break;
                case GuardianMeleeState.StateID.Attack:
                    if (targetEnemy == null)
                    {

                        Transition(GuardianMeleeState.StateID.Idle);
                        animator.SetTrigger("Idle");
                    }
                    else if (!IsInAttackRange(targetEnemy.transform))
                    {
                        Transition(GuardianMeleeState.StateID.Idle);
                        animator.SetTrigger("Idle");
                    }
                    break;
                default:
                    Debug.LogError("unexpected state in enemy melee!");
                    break;
            }
        }

        void Transition(GuardianMeleeState.StateID stateID)
        {
            state.OnStateExit(this);
            var oldState = state.ID;
            state = states[stateID];
            state.OnStateEnter(this);
            //Debug.Log($"{oldState} --> {state.stateID}");
        }
    }

}