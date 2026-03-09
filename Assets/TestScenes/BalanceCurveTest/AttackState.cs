using UnityEngine;
using UnityEngine.InputSystem;

namespace BarqueOfRa.Test.BalanceCurveTest
{
    public class AttackState : GuardianMeleeState
    {
        float attackSeconds;
        float attackSecondsLeft;

        // readonly
        [SerializeField] int scaledAttackDamage;

        // readonly
        [SerializeField] int baseAttackDamage;
        DamageTaker targetDamageTaker;

        AnimationCurve attackDamagePerDeploymentRatio;
        Barque_BalanceCurveTest barque;

        void Awake()
        {
            this.stateID = StateID.Attack;
        }

        override public void OnStateEnter(GuardianMelee_BalanceCurveTest guardianMelee)
        {
            MeleeUnit.InitData initData = guardianMelee.AttackStateInitData;
            attackSeconds = initData.attackSeconds;
            Debug.Assert(attackSeconds > 0f, "attack seconds must be greater than 0!");

            baseAttackDamage = initData.attackDamage;
            Debug.Assert(baseAttackDamage > 0f, "attack damage must be greater than 0!");

            targetDamageTaker = initData.damageTaker;
            Debug.Assert(targetDamageTaker != null, "target DamageTaker was null");

            barque = initData.barque;
            attackDamagePerDeploymentRatio = initData.attackDamagePerDeploymentRatio;

            attackSecondsLeft = attackSeconds;
        }

        override public void OnStateUpdate(GuardianMelee_BalanceCurveTest guardianMelee)
        {
            scaledAttackDamage = guardianMelee.ScaledAttackDamage; 

            if (attackSecondsLeft <= 0f)
            {
                // TODO(Gerald, 2025 07 23): should be triggered by animation event
                targetDamageTaker.TakeDamage(scaledAttackDamage);

                AudioManager.Instance.PlaySound("infantry_attacks");
                attackSecondsLeft = attackSeconds;
            }
            else
            {
                attackSecondsLeft -= Time.deltaTime;
            }
        }

        override public void OnStateExit(GuardianMelee_BalanceCurveTest guardianMelee)
        {
            targetDamageTaker = null;
        }
    }
}
