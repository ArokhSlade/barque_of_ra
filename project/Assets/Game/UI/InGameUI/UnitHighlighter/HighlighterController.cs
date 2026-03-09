using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class HighlighterController : MonoBehaviour
{

    [SerializeField] LineRenderer LineIndicator;
    [SerializeField] Material LineMaterial;
    [SerializeField] float LineThickness;
    [SerializeField] GameObject LightingImpact;
    VisualEffect EndTrailEffect;
    [SerializeField] Color RejectedColour,AcceptedColor;

    Vector3 SnapPoint;

    bool PossiblePlacement;

    private void Awake()
    {
        
        LineIndicator.startWidth = LineThickness;
        LineIndicator.endWidth = LineThickness;
        LineIndicator.sharedMaterial = LineMaterial;
        EndTrailEffect = LightingImpact.GetComponent<VisualEffect>();

        EndTrailEffect.playRate = 0.8f;
        EndTrailEffect.Stop();
    }

    public void updatingSnapPoint(Vector3 NewSnapCoord,bool isFree) { LineIndicator.enabled = true; PossiblePlacement = isFree;  if (NewSnapCoord == Vector3.zero) { SnapPoint = new Vector3(SnapPoint.x,-30,SnapPoint.z); updatingLinePosition(); return; }  SnapPoint = NewSnapCoord; updatingLinePosition(); }
    void updatingLinePosition()
    {
        if (LineIndicator.enabled == false)
        {
            EndTrailEffect.Stop();

            return;
        }

        if (!PossiblePlacement)
        {
            LineMaterial.SetColor("_EmissionColor", RejectedColour * 10);

            EndTrailEffect.SetVector4("Color01", RejectedColour * 10);
            EndTrailEffect.SetVector4("Color2", RejectedColour * 10);

        }
        else
        {
            LineMaterial.SetColor("_EmissionColor", AcceptedColor * 10);
            EndTrailEffect.SetVector4("Color01",AcceptedColor * 10);
            EndTrailEffect.SetVector4("Color2", AcceptedColor * 10);

        }
        LineIndicator.SetPosition(0,transform.position);
        LineIndicator.SetPosition(1 , SnapPoint);
        EndTrailEffect.gameObject.transform.position = SnapPoint;
        EndTrailEffect.Play();


    }
     public void DisableLineIndicator() { LineIndicator.enabled = false;
        EndTrailEffect.Stop();
    }
}
