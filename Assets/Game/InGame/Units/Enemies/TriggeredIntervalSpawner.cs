using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BarqueOfRa.Game.Units.Enemies;
using BarqueOfRa.Game.Interactables;
using BarqueOfRa.Game;

public class TriggeredIntervalSpawner : TriggeredSpawner
{
    [SerializeField] TransformTargetProviderAsset transformTargetProvider;
    [SerializeField] AttackPoint2.AttackPointID attackPointID;

    [SerializeField] Barque barque;

    [SerializeField] Transform target;

    [SerializeField]
    private int numberOfEnemiesPerSpawn;

    [SerializeField]
    private float spawnInterval = 3.0f;

    [SerializeField]
    private float preparationTime = 10f;

    [SerializeField]
    private Waypoint associatedWaypoint;

    private bool shouldSpawn = true;

    private List<GameObject> spawnedObjects;

    [SerializeField]
    private float despawnTime = 3f;

    private BattlePylon associatedPylon;

    void Awake()
    {
        if (associatedWaypoint)
        {
            associatedPylon = associatedWaypoint.AssociatedPylon;
        }
    }


    protected override void Start()
    {
        base.Start();
        barque = InGame.Instance.Barque;

        spawnedObjects = new List<GameObject>();

        if (transformTargetProvider == null)
        {
            Debug.LogError($"No TransformTargetProvider set on {this}!");
            return;
        }

        AttackPoint2 barqueAttackPoint = barque.GetAttackPoint(attackPointID);
        if (barqueAttackPoint == null)
        {
            Debug.LogError($"barque attackpoint {attackPointID} not found");
        }

        target = barqueAttackPoint.transform;
        Debug.Log($"{this} target = {target}");

        if (associatedWaypoint != null)
        {
            associatedWaypoint.OnBarqueStopEnded += StopSpawning;
        }
    }

    protected override void Spawn()
    {
        StartCoroutine(WaitPrepTime());
    }

    private void SpawnNumberOfEnemies()
    {
        for (int i = 0; i < numberOfEnemiesPerSpawn; i++)
        {
            SpawnSingleEnemy();
        }
    }

    private void SpawnSingleEnemy()
    {
        if (target == null)
        {
            Debug.LogError($"No target has been acquired for {this}!");
            return;
        }

        GameObject spawnedObject = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
        spawnedObjects.Add(spawnedObject);

        if (!spawnedObject.TryGetComponent(out EnemyMelee enemy))
        {
            Debug.LogError($"{spawnedObject} does not provide an EnemyMelee component!");
            return;
        }

        enemy.Initialize(target.transform, barque);
        associatedPylon.AddSpawnedEnemy(enemy);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(spawnInterval);
        if (shouldSpawn)
        {
            SpawnNumberOfEnemies();
            StartCoroutine(Respawn());
        }

        //else
        //{
        //    StartCoroutine(WaitDespawn());
        //}
    }

    private void StopSpawning()
    {
        shouldSpawn = false;
    }

    private IEnumerator WaitDespawn()
    {
        yield return new WaitForSeconds(despawnTime);
        DespawnSpawnedObjects();
    }

    private IEnumerator WaitPrepTime()
    {
        yield return new WaitForSecondsRealtime(preparationTime);
        SpawnNumberOfEnemies();
        StartCoroutine(Respawn());
    }

    private void DespawnSpawnedObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
            {
                GameObject spawnedObject = spawnedObjects[i];
                spawnedObjects.RemoveAt(i);
                Destroy(spawnedObject);
            }
    }

    protected override void OnDrawGizmos()
    {
        spawnerDebugColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        connectionDebugColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);

        base.OnDrawGizmos();
    }
}
