namespace BarqueOfRa.Experimental.Units
{
    public interface AttackStateOwner_
    {
        public struct InitData
        {
            public float attackSeconds;
            public int attackDamage;
            public DamageTaker damageTaker;

            public InitData(float attackSeconds, int attackDamage, DamageTaker damageTaker)
            {
                this.attackSeconds = attackSeconds;
                this.attackDamage = attackDamage;
                this.damageTaker = damageTaker;
            }
        }

        public InitData AttackStateInitData { get; }
    }
}