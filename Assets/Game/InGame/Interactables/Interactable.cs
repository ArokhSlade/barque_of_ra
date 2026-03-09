using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Interactable : MonoBehaviour
{
    [SerializeField]
    private string triggerObjectTag;

    [SerializeField]
    private float debugInteractRadius = 1;

    [SerializeField]
    private Color debugColor = Color.cyan;

    private void Start()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.radius = debugInteractRadius;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(triggerObjectTag))
        {
            return;
        }

        Interact(other.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(transform.position, debugInteractRadius);
    }

    private void OnValidate()
    {
        GetComponent<SphereCollider>().radius = debugInteractRadius;
    }

    protected virtual void Interact(GameObject collisionObject) 
    {
        Debug.Log("Interacting with: " + gameObject.name);
    }
}
