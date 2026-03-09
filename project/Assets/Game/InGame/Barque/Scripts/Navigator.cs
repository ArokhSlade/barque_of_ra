using BarqueOfRa.Game;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Navigator : MonoBehaviour
{    
    private NavMeshAgent navAgent;
    
    private List<Waypoint> waypoints;
    private int currentWaypointIndex = 0;

    private bool canMove = false;

    Vector3 spawnPoint;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public bool CanMove {  
        set
        {
            canMove = value;
            Debug.Log("Can Move: " + canMove);
            if (!navAgent.isOnNavMesh)
            {
                Debug.LogError("Barque isnt on NavMesh");
                return;
            }
            if (canMove)
            {                
                navAgent.isStopped = false;
            }
            else
            {
                navAgent.isStopped = true;
            }
        }
        get 
        {
            if (!navAgent.isOnNavMesh)
            {
                Debug.LogError("Barque isnt on NavMesh");
            }
            return canMove; 
        } 
    }

    public void Initialize(Level level)
    {
        if (level.BarqueWaypoints == null || level.BarqueWaypoints.Count == 0)
        {
            Debug.LogError("missing waypoints");
        }

        waypoints = level.BarqueWaypoints;

        PlaceOnNavMeshNearestToSpawnPoint(level);
        currentWaypointIndex = 0;
        canMove = true;
    }

    void PlaceOnNavMeshNearestToSpawnPoint(Level level)
    {
        if (navAgent.enabled)
            {
                Debug.LogWarning("To avoid nav mesh bugs, Barque NavAgent will be disabled at the beginning of initialization.");
                navAgent.enabled = false;
            }

        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(level.BarqueSpawnPoint.position, out navMeshHit, navAgent.height * 2, NavMesh.AllAreas))
        {
            spawnPoint = navMeshHit.position;
            spawnPoint.y += .5f;
        }
        else
        {
            Debug.LogError("no valid nav mesh position found near barque spawn point");
            Debug.LogWarning("trying alternative method for determining barque spawn position");
            spawnPoint = level.BarqueSpawnPoint.position;
            spawnPoint.y += .5f;
        }
        gameObject.transform.position = spawnPoint;

        navAgent.enabled = true;

        if (!navAgent.isOnNavMesh)
        {
            Debug.LogError("Barque isnt on NavMesh");
            return;
        }

        navAgent.Warp(spawnPoint);
        Debug.Log("barque Agent initialized successfully!");
    }

    public void DisableNavAgent()
    {
        navAgent.enabled = false;
    }

    private void Update()  
    {
        if (CanMove)
        {
            CheckDistance();
        }
    }

    private void CheckDistance()
    {
        if (!navAgent.isOnNavMesh)
        {
            return;
        }

        if (navAgent.remainingDistance < 0.1)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        if (!navAgent.isOnNavMesh)
        {
            return;
        }

        currentWaypointIndex++;
        if (currentWaypointIndex < waypoints.Count)
        {
            navAgent.SetDestination(waypoints[currentWaypointIndex].transform.position);
        }

        else 
        {
            currentWaypointIndex = 0;
            CanMove = false;
        }
    }

    public void SetBarqueSpeed(float speed)
    {
        navAgent.speed = speed;
    }
    
}
