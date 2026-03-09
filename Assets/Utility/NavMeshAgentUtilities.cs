using UnityEngine.AI;

public class NavMeshAgentUtilities
{
    // According to source, this is the most reliable way to determine if a NavMeshAgent reached its target,
    // no if Auto Braking is active or not:
    // https://discussions.unity.com/t/how-can-i-tell-when-a-navmeshagent-has-reached-its-destination/52403/5
    public static bool HasReachedDestination(NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent == null) return false;
        if (navMeshAgent.pathPending) return false;
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) return false;
        if (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude != 0) return false;

        return true;
    }
}