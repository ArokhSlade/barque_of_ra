using System;
using UnityEngine;

public class NarrativeTrigger : MonoBehaviour
{
    [SerializeField]
    private string narrativeObjectTag;

    [SerializeField]
    private string[] narrativeElements;

    [SerializeField]
    private Color GizmoColor = Color.white;

    public static Action<string[]> OnNarrativeStarted;
    public static Action OnNarrativeEnded;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(narrativeObjectTag))
        {
            OnNarrativeStarted?.Invoke(narrativeElements);
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(narrativeObjectTag))
        {
            OnNarrativeEnded?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Gizmos.color = GizmoColor;
        Gizmos.DrawCube(transform.position + collider.center, collider.size);
    }

}
