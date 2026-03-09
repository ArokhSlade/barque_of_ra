using UnityEngine;

namespace BarqueOfRa.Game.Units.Enemies.Melee
{
    public class AttackGuardianState : EnemyMeleeState
    {
        [SerializeField] EffectBundle dealDamageEffectBundle;

        EnemyMelee enemy;

        void Awake()
        {
            stateID = StateID.AttackGuardian;
        }

        override public void OnStateUpdate(EnemyMelee enemy)
        {
            this.enemy = enemy;
        }

        public override void OnStateExit(EnemyMelee enemy)
        {
            base.OnStateExit(enemy);

            if (dealDamageEffectBundle.TryGetComponent(out EffectBundle effectBundle))
            {
                effectBundle.Reset();
            }
        }

        public void AttackImpact()
        {
            if (enemy == null) return;

            if (enemy.GuardianUnderAttack == null) return;
            if (!enemy.GuardianUnderAttack.TryGetComponent(out DamageTaker damageTaker)) return;

            damageTaker.TakeDamage(enemy.AttackDamage);

            if (dealDamageEffectBundle != null && dealDamageEffectBundle.TryGetComponent(out EffectBundle effectBundle))
            {
                effectBundle.Play();
            }
        }
    }
}
