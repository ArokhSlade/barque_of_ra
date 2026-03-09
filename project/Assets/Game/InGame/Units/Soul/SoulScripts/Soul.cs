using UnityEngine;

namespace BarqueOfRa.Units
{
    public class Soul : MonoBehaviour
    {
        private void Awake()
        {
            Alive = true;
        }

        public bool Alive
        {
            get
            {
                return gameObject.activeSelf;
            }

            set
            {
                gameObject.SetActive(value);
            }
        }
    }
}