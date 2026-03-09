using UnityEngine;

namespace BarqueOfRa.Game.Units.Guardians.Melee
{
    public class AttackState : GuardianMeleeState
    {
        [SerializeField] EffectBundle dealDamageEffectBundle;

        GuardianMelee guardian;

        void Awake()
        {
            stateID = StateID.Attack;
        }

        override public void OnStateUpdate(GuardianMelee guardian)
        {
            this.guardian = guardian;
        }

        public override void OnStateExit(GuardianMelee guardian)
        {
            base.OnStateExit(guardian);

            if (dealDamageEffectBundle.TryGetComponent(out EffectBundle effectBundle))
            {
                effectBundle.Reset();
            }
        }

        public void AttackImpact()
        {
            if (guardian == null) return;

            if (guardian.EnemyUnderAttack == null) return;
            if (!guardian.EnemyUnderAttack.TryGetComponent(out DamageTaker damageTaker)) return;

            damageTaker.TakeDamage(guardian.ScaledAttackDamage);

            if (dealDamageEffectBundle != null && dealDamageEffectBundle.TryGetComponent(out EffectBundle effectBundle))
            {
                effectBundle.Play();
            }
        }
    }
}
