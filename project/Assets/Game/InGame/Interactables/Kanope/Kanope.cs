using BarqueOfRa.Game;
using UnityEngine;

public class Kanope : MonoBehaviour
{

    [SerializeField]
    private int soulAmount = 1;
    [SerializeField] GameObject VFX_Ghosties;
    GameObject temp;
    Barque barque;
    void OnTriggerEnter(Collider other)
    {
        barque = other.GetComponent<KanopeCollector>().Barque;

        PlayAnimation();
        AudioManager.Instance.PlaySound("kanope_collected");
    }

    void PlayAnimation()
    {
        GetComponent<Animation>().Play();
    }
    void SpawnVFX()
    {
        temp = Instantiate(VFX_Ghosties, transform.position, Quaternion.identity);
    }
    void AddingSouls()
    {
        Debug.Log("Added Trigger State " + soulAmount);
        barque.SoulCabin.AddSouls(soulAmount);
    }
    public void Disappear()
    {
        Destroy(temp);
        Destroy(gameObject);
    }
}
