#nullable enable

using UnityEngine;
using BarqueOfRa.Game.Units.Guardians.Melee;
using BarqueOfRa.Game.Units.Enemies;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace BarqueOfRa.Game.Units.Guardians
{
    [RequireComponent(typeof(Health))]
    public class GuardianMelee : Guardian
    {
        [SerializeField] AnimationCurve attackDamagePerDeploymentRatio;

        [SerializeField] public int attackDamage;
        [SerializeField] public float attackCooldown;
        [SerializeField] public float attackRange;
        [SerializeField] public RangeIndicator RangeRadiusUI;
        [SerializeField] float detectionRange;
        [SerializeField] LayerMask combatTargetsMask;

        //NOTE(Gerald 2025 08 02): readonly
        [SerializeField] int scaledAttackDamage;
        public int ScaledAttackDamage => scaledAttackDamage;

        [SerializeField] Animator animator;
        [SerializeField] List<GuardianMeleeState> statesList;
        [SerializeField] GuardianMeleeState thinkState;

        [SerializeField] Barque barque;
        public Barque Barque => barque;

        public float AttackCooldown => attackCooldown;
        public int AttackDamage => attackDamage;
        public float AttackRange => attackRange;

        [SerializeField] InputActionReference DEBUG_logScaledDamageActionRef;
        InputAction DEBUG_logScaledDamageAction;
        public bool DebugActionPressed
        {
            get
            {
                if (DEBUG_logScaledDamageAction != null)
                {
                    return DEBUG_logScaledDamageAction.WasCompletedThisFrame();
                }
                return false;
            }
        }

        Health health;
        Dictionary<GuardianMeleeState.StateID, GuardianMeleeState> states = new();

        Enemy? closestDetectableEnemy;
        public Enemy? EnemyUnderAttack { get; private set; }

        void Awake()
        {
            health = GetComponent<Health>();
            RangeRadiusUI = GetComponent<RangeIndicator>();

            SubscribeToHealthExhausted();

            if (animator == null)
            {
                Debug.LogError($"{gameObject} has no Animator set!");
                return;
            }

            animator.SetFloat("SpeedMulti", 1 / attackCooldown);
        }

        void Start()
        {
            if (barque == null)
            {
                barque = InGame.Instance.Barque;
            }

            foreach (var state in statesList)
            {
                states.Add(state.ID, state);
            }
            thinkState = states[GuardianMeleeState.StateID.Idle];
        }

        void FixedUpdate()
        {
            UpdateDamageBasedOnSoulCount();
            UpdateDetectableEnemies();
            UpdateAttackableUnits();

            thinkState.OnStateUpdate(this);
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

        bool IsEnemyInRange() => closestDetectableEnemy != null;

        bool IsInAttackRange(Transform other) => (other.position - transform.position).magnitude < attackRange;

        void UpdateDetectableEnemies()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, combatTargetsMask, QueryTriggerInteraction.Collide);

            if (colliders.Length <= 0)
            {
                closestDetectableEnemy = null;
                return;
            }

            float minDistanceSquared = float.MaxValue;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Enemy currentEnemy))
                {
                    float currentDistanceSquared = (currentEnemy.transform.position - transform.position).sqrMagnitude;
                    if (currentDistanceSquared < minDistanceSquared)
                    {
                        minDistanceSquared = currentDistanceSquared;
                        closestDetectableEnemy = currentEnemy;
                    }
                }
            }
        }

        void UpdateAttackableUnits()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, combatTargetsMask, QueryTriggerInteraction.Collide);

            if (colliders.Length <= 0)
            {
                EnemyUnderAttack = null;
                return;
            }

            float minGuardianDistanceSquared = float.MaxValue;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Enemy enemy))
                {
                    float currentDistanceSquared = (enemy.transform.position - transform.position).sqrMagnitude;
                    if (currentDistanceSquared < minGuardianDistanceSquared)
                    {
                        minGuardianDistanceSquared = currentDistanceSquared;

                        EnemyUnderAttack = enemy;
                    }
                }
            }
        }

        void UpdateDamageBasedOnSoulCount()
        {
            int oldScaledAttackDamge = scaledAttackDamage;

            float powerBonusRatio = barque.PowerBonusRatio;

            scaledAttackDamage = attackDamage + (int)((float)attackDamage * powerBonusRatio);
            if (scaledAttackDamage != oldScaledAttackDamge || DebugActionPressed)
            {
                Debug.Log($"Scaled Damage Bonus: {powerBonusRatio} => {attackDamage} -> {scaledAttackDamage}");
            }
        }

        void OnHealthExhausted()
        {
            animator.SetTrigger("Death");
            Transition(GuardianMeleeState.StateID.Perished);

            AnimatorClipInfo[] currentClip = animator.GetCurrentAnimatorClipInfo(0);

            if (currentClip.Length > 0)
            {
                Invoke("RemovingGameobject", currentClip[0].clip.length);
            }
        }

        void RemovingGameobject()
        {
            Destroy(gameObject);
        }

        void SubscribeToHealthExhausted()
        {
            health.HealthExhausted.AddListener(OnHealthExhausted);
        }

        void UnsubscribeFromHealthExhausted()
        {
            health.HealthExhausted.RemoveListener(OnHealthExhausted);
        }

        public void Think()
        {
            if (closestDetectableEnemy != null)
            {
                #region Shady
                // remove line 141 if guardians can use navmesh again
                if (health.CurrentHealth > 0)
                {
                    transform.LookAt(new Vector3(closestDetectableEnemy.transform.position.x, transform.position.y, closestDetectableEnemy.transform.position.z));

                }
                #endregion
            }

            switch (thinkState.ID)
            {
                case GuardianMeleeState.StateID.Idle:
                    if (EnemyUnderAttack != null)
                    {
                        Transition(GuardianMeleeState.StateID.Attack);
                        animator.SetTrigger("Attack");
                    }
                    break;
                case GuardianMeleeState.StateID.Attack:
                    if (EnemyUnderAttack == null)
                    {
                        Transition(GuardianMeleeState.StateID.Idle);
                        animator.SetTrigger("Idle");
                    }
                    break;
                case GuardianMeleeState.StateID.Perished:
                    break;
                default:
                    Debug.LogError("unexpected state in guardian melee!");
                    break;
            }
        }

        void Transition(GuardianMeleeState.StateID stateID)
        {
            thinkState.OnStateExit(this);
            var oldState = thinkState.ID;

            RangeRadiusUI.OnStateChanged?.Invoke(states[stateID].ID.ToString());

            thinkState = states[stateID];
            thinkState.OnStateEnter(this);
        }
    }
}