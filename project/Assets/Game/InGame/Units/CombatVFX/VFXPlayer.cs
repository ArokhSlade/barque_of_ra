using UnityEngine;
using UnityEngine.VFX;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField] VisualEffect VFX_SwordImpact, VFX_Slash, VFX_LightningBlitz, VFX_LightningImpact, VFX_BirdAttack;
    [SerializeField] GameObject VFX_Death;
    [SerializeField] GameObject VFX_MultipleClawAttack;

    public void PlaySlashVFX()
    {
        VFX_Slash.gameObject.SetActive(true);
        VFX_Slash.Play();
    }

    public void PlaySwordImpactVFX()
    {
        VFX_SwordImpact.gameObject.SetActive(true);
        VFX_SwordImpact.Play();
    }

    public void PlayClawAttackVFX()
    {
        VFX_MultipleClawAttack.SetActive(true);

        foreach (VisualEffect VFX in VFX_MultipleClawAttack.GetComponentsInChildren<VisualEffect>())
        {
            VFX.Play();
        }
    }

    public void PlayBirdAttackVFX()
    {
        VFX_BirdAttack.gameObject.SetActive(true);
        VFX_BirdAttack.Play();
    }

    public void PlayLightningBlitzVFX()
    {
        VFX_LightningBlitz.gameObject.SetActive(true);
        VFX_LightningBlitz.Play();
    }

    public void PlayLightningImpactVFX()
    {
        VFX_LightningImpact.gameObject.SetActive(true);
        VFX_LightningImpact.Play();
    }

    public void PlayDeathVFX()
    {
        Vector3 SpawnPosition = this.transform.parent.position + VFX_Death.transform.localPosition;
        GameObject temp = Instantiate(VFX_Death, SpawnPosition, Quaternion.identity);
        temp.gameObject.SetActive(true);
        temp.transform.position = SpawnPosition;
    }

    public void DisableVFX()
    {
        if (VFX_Death)
        {
            VFX_Death.gameObject.SetActive(false);

        }
        if (VFX_SwordImpact)
        {
            VFX_SwordImpact.gameObject.SetActive(false);

        }
        if (VFX_MultipleClawAttack)
        {
            VFX_MultipleClawAttack.gameObject.SetActive(false);

        }
    }
}
