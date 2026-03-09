using UnityEngine;
using UnityEngine.VFX;

public class EffectBundle : MonoBehaviour
{
    [SerializeField] VisualEffect[] visualEffects;
    [SerializeField] ParticleSystem[] particleSystems;
    [SerializeField] string soundEffectName;

    public void Play()
    {
        foreach (VisualEffect visualEffect in visualEffects)
        {
            visualEffect.gameObject.SetActive(true);
            visualEffect.Play();
        }

        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
        }

        if (soundEffectName.Length > 0)
        {
            AudioManager.Instance.PlaySound(soundEffectName);
        }
    }

    public void Reset()
    {
        foreach (VisualEffect visualEffect in visualEffects)
        {
            visualEffect.Stop(); // TODO-CS: Is this good or bad?
            visualEffect.gameObject.SetActive(false);
        }

        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Stop();
            particleSystem.gameObject.SetActive(false);
        }
    }
}
