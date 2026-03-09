using UnityEngine;

[CreateAssetMenu(fileName = "TransformTargetProviderByReference", menuName = "TargeterProviders/ByReference")]
class TransformTargetProviderByReference : TransformTargetProviderAsset
{
    [SerializeField] Transform targetTransform;

    public override Transform AcquireTargetTransform()
    {
        if (targetTransform == null)
        {
            Debug.LogError($"No reference set!");
            return null;
        }

        return targetTransform;
    }
}
