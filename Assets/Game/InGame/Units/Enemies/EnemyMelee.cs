#nullable enable

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BarqueOfRa.Game.Units.Enemies.Melee;
using UnityEngine.Events;

namespace BarqueOfRa.Game.Units.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Health))]
    public class EnemyMelee : Enemy
    {
        public UnityEvent<EnemyMelee> Died;

        [SerializeField] int attackDamage;
        [SerializeField] float attackCooldown;
        [SerializeField] float attackRange;
        [SerializeField] float detectionRange;
        [SerializeField] LayerMask combatTargetsMask;

        [SerializeField] Animator animator;
        [SerializeField] List<EnemyMeleeState> statesList;

        [SerializeField] public Transform Target;

        [SerializeField] Barque barque; //NOTE(Gerald, 2025 07 25): set in Initialize(), e.g. by spawner
        [SerializeField] EnemyMeleeState thinkState;

        NavMeshAgent navMeshAgent;
        Health health;
        Dictionary<EnemyMeleeState.StateID, EnemyMeleeState> states = new();

        //TODO(Gerald 2025 07 26) rethink where this belongs. it's connected to atack guardian state or not?
        Guardian? closestDetectableGuardian;

        //AttackGuardian State variables
        //TODO(Gerald 2025 07 25) move this out into the attackguardian state. and replace with a public method / property that get's called from there
        public Guardian? GuardianUnderAttack { get; private set; }

        public bool IsDead { get; private set; } = false;

        public Barque Barque => barque;
        public float AttackCooldown => attackCooldown;
        public int AttackDamage => attackDamage;
        public float AttackRange => attackRange;

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

            animator.SetFloat("SpeedMulti", 1 / attackCooldown);

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
            thinkState = states[EnemyMeleeState.StateID.Idle];
        }

        void FixedUpdate()
        {
            UpdateDetectableGuardians();
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

        public void OnAttackPointReached()
        {
            if (!TryGetComponent(out AudioSource audioSource)) return;

            audioSource.Play();
        }

        // AttackBarqueState related
        public void OnBarqueHit()
        {
            Die();
        }

        // TODO(Gerald, 2025 07 28):
        // quick-and-simple solution for now.
        // I think it helps to have a single place where Destroy is called,
        // so it's easier to debug if it happens when it shouldn't or the other way around
        // we may need to rename it, because there are different kinds of dying.
        // 1:
        // when killed in battle while in dog form.
        // in that case, the dying animation should be played and the game object should stay active but not interactable for the most part.
        // 2:
        // after having hit the boat in lightning form.
        // at least, if lightning bolt is still controlled by this script.
        // might also be a different object entirely that was spawned during transformation.
        // in which case enemy probably did Die() at the time of transformation, but perhaps with a different animation.
        // 3:
        // if, for whatever reason, we have left-over units that should be cleaned up
        // because they are not relevant for the game anymore.
        private void Die()
        {
            Died?.Invoke(this);
            gameObject.SetActive(false);
            IsDead = true;
            Destroy(gameObject);
        }

        void UpdateDetectableGuardians()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, combatTargetsMask, QueryTriggerInteraction.Collide);

            if (colliders.Length <= 0)
            {
                closestDetectableGuardian = null;
                return;
            }

            float minDistanceSquared = float.MaxValue;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Guardian currentGuardian))
                {
                    float currentDistanceSquared = (currentGuardian.transform.position - transform.position).sqrMagnitude;
                    if (currentDistanceSquared < minDistanceSquared)
                    {
                        minDistanceSquared = currentDistanceSquared;
                        closestDetectableGuardian = currentGuardian;
                    }
                }
            }
        }

        void UpdateAttackableUnits()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, combatTargetsMask, QueryTriggerInteraction.Collide);

            if (colliders.Length <= 0)
            {
                GuardianUnderAttack = null;
                return;
            }

            float minGuardianDistanceSquared = float.MaxValue;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Guardian guardian))
                {
                    float currentDistanceSquared = (guardian.transform.position - transform.position).sqrMagnitude;
                    if (currentDistanceSquared < minGuardianDistanceSquared)
                    {
                        minGuardianDistanceSquared = currentDistanceSquared;

                        GuardianUnderAttack = guardian;
                    }
                }
            }
        }

        void OnHealthExhausted()
        {
            animator.SetTrigger("Death");
            Transition(EnemyMeleeState.StateID.Perished);

            AnimatorClipInfo[] currentClip = animator.GetCurrentAnimatorClipInfo(0);

            if (currentClip.Length > 0)
            {
                Invoke("Die", currentClip[0].clip.length);
            }
        }

        void SubscribeToHealthExhausted()
        {
            health.HealthExhausted.AddListener(OnHealthExhausted);
        }

        void UnsubscribeFromHealthExhausted()
        {
            health.HealthExhausted.RemoveListener(OnHealthExhausted);
        }

        public void SetTarget(Transform target)
        {
            Target = target;
            //Debug.Log($"Set Target: {Target}");
        }

        private void OnDrawGizmos()
        {
            DrawGizmoNavMeshPath();
            DrawGizmoDetectionRange();
            DrawGizmoAttackRange();
        }

        private void DrawGizmoNavMeshPath()
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

        public void Think()
        {
            switch (thinkState.ID)
            {
                case EnemyMeleeState.StateID.Idle:
                    if (closestDetectableGuardian != null)
                    {
                        Transition(EnemyMeleeState.StateID.AttackGuardian);
                        animator.SetTrigger("AttackGuardian");
                    }
                    else
                    {
                        if (Target != null)
                        {
                            Transition(EnemyMeleeState.StateID.Approach);
                            animator.SetTrigger("Approach");
                        }
                        // NOTE(Gerald 2025 07 22): didn't realize we had TargetProviders.
                        // leaving this as a fallback for now.
                        else if (barque != null)
                        {
                            Debug.LogWarning("no Target set. trying to set barque as target.");
                            SetTarget(barque.transform);
                            Transition(EnemyMeleeState.StateID.Approach);
                            animator.SetTrigger("Approach");
                        }
                        else
                        {
                            Debug.LogWarning("no barque set.");
                        }
                    }
                    break;
                case EnemyMeleeState.StateID.Approach:
                    //TODO(Gerald, 2025 07 22): if (AttackedByGuardian) {...}
                    if (GuardianUnderAttack != null)
                    {
                        Transition(EnemyMeleeState.StateID.AttackGuardian);
                        animator.SetTrigger("AttackGuardian");
                    }
                    else if (NavMeshAgentUtilities.HasReachedDestination(navMeshAgent))
                    {
                        Transition(EnemyMeleeState.StateID.AttackBarque);
                        animator.SetTrigger("AttackBarque");
                    }
                    break;
                case EnemyMeleeState.StateID.AttackGuardian:
                    if (GuardianUnderAttack == null)
                    {
                        Transition(EnemyMeleeState.StateID.Approach);
                        animator.SetTrigger("Approach");
                    }
                    break;
                case EnemyMeleeState.StateID.AttackBarque:
                    break;
                case EnemyMeleeState.StateID.Perished:
                    break;
                default:
                    Debug.LogError("unexpected state in enemy melee!");
                    break;
            }
        }

        void Transition(EnemyMeleeState.StateID stateID)
        {
            thinkState.OnStateExit(this);
            var oldState = thinkState.ID;
            thinkState = states[stateID];
            thinkState.OnStateEnter(this);
            //Debug.Log($"{oldState} --> {thinkState.stateID}");
        }
    }
}
