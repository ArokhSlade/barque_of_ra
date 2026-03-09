using UnityEngine.AI;

namespace BarqueOfRa.Experimental.Units.Enemies
{
    public interface AttackBarqueStateOwner_
    {
        public struct InitData
        {
            public Barque barque;
            public NavMeshAgent navMeshAgent;
            public InitData(Barque barque, NavMeshAgent navMeshAgent)
            {
                this.barque = barque;
                this.navMeshAgent = navMeshAgent;
            }
        }

        public InitData AttackBarqueStateInitData { get; }
    }
}