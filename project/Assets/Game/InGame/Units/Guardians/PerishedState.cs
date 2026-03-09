using UnityEngine;

namespace BarqueOfRa.Game.Units.Guardians.Melee
{
    public class PerishedState : GuardianMeleeState
    {
        [SerializeField] EffectBundle perishEffectBundle;

        void Awake()
        {
            stateID = StateID.Perished;
        }

        public override void OnStateEnter(GuardianMelee guardian)
        {
            if (perishEffectBundle != null && perishEffectBundle.TryGetComponent(out EffectBundle effectBundle))
            {
                effectBundle.Play();
            }
        }
    }
}
