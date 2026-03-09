using UnityEngine;

public class OnCameraTriggers : MonoBehaviour
{
    enum ZoomingFunction { ZoomingIn , ZoomingOut }
    [SerializeField] ZoomingFunction ZoomingFunctions;
    [SerializeField] CameraControllerBehaviour CamControl;
    [SerializeField] Color GizmoColor = Color.blue;

    private void Start()
    {
        CamControl = FindAnyObjectByType<CameraControllerBehaviour>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Boat"))
        {
            if (ZoomingFunctions == ZoomingFunction.ZoomingIn)
            {

                CamControl.InCombat = false;
                CamControl.Zooming();
            }
            else
            {
                CamControl.InCombat = true;
                CamControl.Zooming();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            if(ZoomingFunctions == ZoomingFunction.ZoomingIn)
            {

                CamControl.InCombat = true;
                CamControl.Zooming();
            }
            else
            {
                CamControl.InCombat = false;
                CamControl.Zooming();
            }
        }
    }

    private void OnDrawGizmos()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Gizmos.color = GizmoColor;
        Gizmos.DrawCube(transform.position + collider.center, collider.size);
    }

}
