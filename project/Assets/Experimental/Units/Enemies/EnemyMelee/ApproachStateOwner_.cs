using UnityEngine.AI;

namespace BarqueOfRa.Experimental.Units.Enemies
{
    public interface ApproachStateOwner_
    {
        public struct InitData
        {
            public NavMeshAgent navMeshAgent;

            public InitData(NavMeshAgent navMeshAgent)
            {
                this.navMeshAgent = navMeshAgent;
            }
        }

        public InitData ApproachStateInitData { get; }
    }
}