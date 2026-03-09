using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AttackPoint : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 1);

        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
