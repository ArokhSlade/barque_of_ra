using UnityEngine;

public abstract class TransformTargetProviderAsset : ScriptableObject, ITransformTargetProvider
{
    public abstract Transform AcquireTargetTransform();
}


interface ITransformTargetProvider
{
    Transform AcquireTargetTransform();
}