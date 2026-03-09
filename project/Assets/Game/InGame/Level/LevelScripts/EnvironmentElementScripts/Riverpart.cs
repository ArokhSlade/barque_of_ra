using UnityEngine;

public class Riverpart : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material riverMaterial = meshRenderer.materials[0];
        if (riverMaterial != null && riverMaterial.color.a > 0)
        {
            riverMaterial.color = new Color(riverMaterial.color.r, riverMaterial.color.g, riverMaterial.color.b, 0);
        }
    }
}
