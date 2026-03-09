using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] float maxSpeed = 10f;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navAgent.destination = target.position;
        navAgent.speed = maxSpeed;
    }
}
