using UnityEngine;
using BarqueOfRa.Game.Units.Enemies;


public class TriggeredEnemySpawner : TriggeredSpawner
{
    [SerializeField] TransformTargetProviderAsset transformTargetProvider;

    //TODO(Gerald 2025 07 22): maybe find a way to auto-initialize the barque, maybe by InGame object?
    [SerializeField] Barque barque;

    Transform target;

    protected override void Start()
    {
        base.Start();
        Debug.LogError("TriggeredEnemySpawner script used. wasn't this replaced by TriggeredIntervalSpawner?");
        if (transformTargetProvider == null)
        {
            Debug.Log($"No TransformTargetProvider set on {this}!");
            return;
        }

        target = transformTargetProvider.AcquireTargetTransform();
    }

    protected override void Spawn()
    {
        if (target == null)
        {
            Debug.LogError($"No target has been acquired for {this}!");
            return;
        }

        GameObject spawnedObject = Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);

        if (!spawnedObject.TryGetComponent(out EnemyMelee enemy))
        {
            Debug.LogError($"{spawnedObject} does not provide an Enemy component!");
            return;
        }

        enemy.Initialize(target.transform, barque);
    }

    protected override void OnDrawGizmos()
    {
        spawnerDebugColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        connectionDebugColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);

        base.OnDrawGizmos();
    }
}
