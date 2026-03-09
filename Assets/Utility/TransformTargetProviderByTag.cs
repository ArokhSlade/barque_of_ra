using UnityEngine;

[CreateAssetMenu(fileName = "TransformTargetProviderByTag", menuName = "TargetProviders/ByTag")]
class TransformTargetProviderByTag : TransformTargetProviderAsset
{
    [SerializeField] string tag;

    public override Transform AcquireTargetTransform()
    {
        GameObject taggedObject = GameObject.FindWithTag(tag);
        if (taggedObject == null)
        {
            Debug.LogError($"No GameObject with tag '{tag}' found!");
            return null;
        }

        return taggedObject.transform;
    }
}
