using UnityEngine;

public class UIProjectionTest : MonoBehaviour
{
    [SerializeField] Transform worldTransform;
    [SerializeField] RectTransform uiTransform;


    Vector3 worldToScreenPos(Vector3 worldPos)
    {
        Camera cam = Camera.main;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
        return screenPos;
    }

    void Update()
    {
        uiTransform.position = worldToScreenPos(worldTransform.position);
    }
}
