using UnityEngine;
using UnityEngine.VFX;

public class AnimationEvent : MonoBehaviour
{

    [SerializeField] VisualEffect vfx; 

    
    public void AttackHit()
    {
        vfx.Play();
    }

    public void TriggerHitSpark()
    {
        vfx.Play();
    }
}
