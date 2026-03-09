using UnityEngine;

namespace BarqueOfRa.Test.BalanceCurveTest
{
    public interface MeleeUnit
    {
        public struct InitData
        {
            public float attackSeconds;
            public int attackDamage;
            public DamageTaker damageTaker;
            public Barque_BalanceCurveTest barque;
            public AnimationCurve attackDamagePerDeploymentRatio;

            public InitData(float attackSeconds, int attackDamage, DamageTaker damageTaker, Barque_BalanceCurveTest barque, AnimationCurve attackDamagePerDeploymentRatio)
            {
                this.attackSeconds = attackSeconds;
                this.attackDamage = attackDamage;
                this.damageTaker = damageTaker;
                this.barque = barque;
                this.attackDamagePerDeploymentRatio = attackDamagePerDeploymentRatio;
            }
        }

        public InitData AttackStateInitData { get; }
    }
}