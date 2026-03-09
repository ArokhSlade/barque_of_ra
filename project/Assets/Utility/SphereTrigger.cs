using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereTrigger : MonoBehaviour
{
    public event Action Triggered;

    [SerializeField] string triggerOnlyTag;
    [SerializeField] Color gizmoColor = Color.white;

    void OnTriggerEnter(Collider other)
    {
        if (triggerOnlyTag.Length != 0)
        {
            if (!other.gameObject.CompareTag(triggerOnlyTag)) return;
        }

        Triggered?.Invoke();
    }

    void OnDrawGizmos()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position + sphereCollider.center, sphereCollider.radius);
    }
}
