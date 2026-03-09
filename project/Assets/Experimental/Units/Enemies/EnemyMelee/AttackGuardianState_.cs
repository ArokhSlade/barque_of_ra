namespace BarqueOfRa.Experimental.Units.Enemies
{
    public class AttackGuardianState_ : AttackState_<EnemyMelee_, EnemyMelee_.StateID>
    {
        override public EnemyMelee_.StateID ID => EnemyMelee_.StateID.AttackGuardian;
    }
}