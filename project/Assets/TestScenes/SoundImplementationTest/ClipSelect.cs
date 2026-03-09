using UnityEngine;

public class ClipSelect : MonoBehaviour
{
    
    [SerializeField] AudioClip yap;
    [SerializeField] AudioClip gameOver;
    [SerializeField] AudioClip grunt;
    [SerializeField] AudioClip bark;
    
    [SerializeField] AudioSource audioSource;
    
    public enum SoundID
    {
        None = 0,
        Yap = 10,
        Grunt = 20,
        Bark = 30,
        GameOver = 100
    }

    public void Yap()
    {
        PlaySound(SoundID.Yap);
    }

    public void Grunt()
    {
        PlaySound(SoundID.Grunt);
    }

    public void Bark()
    {
        PlaySound(SoundID.Bark);
    }

    public void PlaySound(SoundID soundID)
    {
        switch(soundID)
        {
            case SoundID.Yap:
                audioSource.clip = yap;
                break;
            case SoundID.Grunt:
                audioSource.clip = grunt;
                break;
            case SoundID.Bark:
                audioSource.clip = bark;
                break;
            case SoundID.GameOver:
            case SoundID.None:
            default:
                audioSource.clip = gameOver;
                break;                
        }
        audioSource.Play();
    }

}
