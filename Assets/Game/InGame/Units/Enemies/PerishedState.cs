using BarqueOfRa.Game.Units.Enemies;
using BarqueOfRa.Game.Units.Enemies.Melee;
using UnityEngine;

public class PerishedState : EnemyMeleeState
{
    [SerializeField] EffectBundle perishEffectBundle;

    void Awake()
    {
        stateID = StateID.Perished;
    }

    public override void OnStateEnter(EnemyMelee enemy)
    {
        if (perishEffectBundle != null && perishEffectBundle.TryGetComponent(out EffectBundle effectBundle))
        {
            effectBundle.Play();
        }
    }
}
