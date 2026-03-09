using UnityEngine;


public class TestDetector : MonoBehaviour
{
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] LayerMask targetMask;

    void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereCollider.radius, targetMask, QueryTriggerInteraction.Collide);
        string message = "overlaps: ";
        foreach (var collider in colliders)
        {
            message += collider;
        }
        if (colliders.Length != 0)
        {
            message = $" count: {colliders.Length}" + message;
            Debug.Log(message);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"entered: {other}");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"exited: {other}");
    }
}
