using UnityEngine;

namespace BarqueOfRa.Test.AudioImplementationTest
{
    public class AudioDog : MonoBehaviour
    {
        [SerializeField] ClipSelect audioClips;
        [SerializeField] Animator animator;

        bool isAlive = true;
        public void TakeDamage()
        {
            if (isAlive)
            {
                audioClips.Yap();
            }
        }
        public void Die()
        {
            if (isAlive)
            {
                isAlive = false;
                audioClips.Grunt();
            }
        }

        public void AttackAudio()
        {
            audioClips.Bark();
        }

        public void Attack()
        {
            animator.SetTrigger("OnAttack");
        }
    }
}
