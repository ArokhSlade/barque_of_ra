using UnityEditor;
using UnityEngine;

public class TriggeredSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] SphereTrigger trigger;
    [SerializeField] protected Color spawnerDebugColor = Color.red;
    [SerializeField] protected float spawnerDebugRadius = 0.5f;
    [SerializeField] protected Color connectionDebugColor = Color.red;
    [SerializeField] protected float connectionDebugThickness = 5;

    protected virtual void Start()
    {
        SubscribeToTrigger();
    }

    void OnDestroy()
    {
        UnsubscribeFromTrigger();
    }

    void SubscribeToTrigger()
    {
        if (trigger == null)
        {
            Debug.LogError($"No spawn trigger set on {this}!");
            return;
        }

        trigger.Triggered += Spawn;
    }

    void UnsubscribeFromTrigger()
    {
        trigger.Triggered -= Spawn;
    }

    protected virtual void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
    }

    protected virtual void OnDrawGizmos()
    {
        DebugDrawSpawnPoint();
        DebugDrawTriggerConnection();
    }

    protected virtual void DebugDrawSpawnPoint()
    {
        Gizmos.color = spawnerDebugColor;
        Gizmos.DrawSphere(transform.position, spawnerDebugRadius);
    }

    private void DebugDrawTriggerConnection()
    {
        if (trigger != null)
        {
            Vector3 triggerPosition = trigger.transform.position;
#if UNITY_EDITOR
            Handles.DrawBezier(transform.position, triggerPosition, transform.position, triggerPosition, connectionDebugColor, null, connectionDebugThickness);
#endif
        }
    }
}
