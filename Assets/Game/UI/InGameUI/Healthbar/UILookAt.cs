using UnityEngine;

public class UILookAt : MonoBehaviour
{
    Transform CameraTransform;

    [SerializeField] float OffsetXRotation;
    [SerializeField] float OffsetYRotation;


    private void Start()
    {
        CameraTransform = Camera.main.transform;
    }
    void Update()
    {
        transform.LookAt(new Vector3(CameraTransform.position.x + OffsetYRotation, CameraTransform.position.y + OffsetXRotation, CameraTransform.position.z));
    }
    
}
