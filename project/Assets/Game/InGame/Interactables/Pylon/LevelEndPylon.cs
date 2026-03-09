using UnityEngine;

namespace BarqueOfRa.Game.Interactables
{
    public class LevelEndPylon : MonoBehaviour
    {
        [SerializeField] InGame inGame;

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Barque>())
            {
                inGame?.OnBarqueEnteredLevelEndPylon();
            }
        }

        public void Setup(InGame inGame)
        {
            this.inGame = inGame;
        }

    }

}
