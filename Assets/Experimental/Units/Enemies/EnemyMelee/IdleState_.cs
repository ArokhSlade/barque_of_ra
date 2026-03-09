namespace BarqueOfRa.Experimental.Units.Enemies
{
    public class IdleState_ : IdleState_<EnemyMelee_, EnemyMelee_.StateID>
    {
        override public EnemyMelee_.StateID ID => EnemyMelee_.StateID.Idle;
    }
}