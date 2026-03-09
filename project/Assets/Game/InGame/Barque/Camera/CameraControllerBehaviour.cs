using Unity.Cinemachine;
using UnityEngine;

public class CameraControllerBehaviour : MonoBehaviour
{
    public bool InCombat { get; set; }


    [SerializeField] float StartingZoom;

    [SerializeField] float MaxZoomIn;
    [SerializeField] float MaxZoomOut;


    [Tooltip("Large numbers make camera zooming speed slower and smaller make it more responsive ")]
    [SerializeField][Range(0.1f, 20)] float zoomingSpeed;

    CinemachineFollowZoom ZoomingComponent;

    Animator animator;

    private void Start()
    {
        ZoomingComponent = GetComponent<CinemachineFollowZoom>();
        animator = GetComponent<Animator>();
        ZoomingComponent.Width = StartingZoom;
        ZoomingComponent.Damping = zoomingSpeed;
    }
    public void Zooming()
    {
        if (InCombat)
        {

            animator.SetTrigger("InCombat");

        }
        else
        {
            animator.SetTrigger("OutCombat");
        }
    }
}
