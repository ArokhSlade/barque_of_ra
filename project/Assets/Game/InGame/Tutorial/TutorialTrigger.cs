using UnityEngine;
using BarqueOfRa.Game.UI;
using BarqueOfRa.Game.UI.States.Combat;

namespace BarqueOfRa.Game
{
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] Tutorial tutorial;
        [SerializeField] TutorialPopup popup;
        [SerializeField] GameObject Marker;
        [SerializeField] TutorialManager FirstSection,Middlesection , SecondEncounter;
        void Start() 
        {
            if (Marker != null) 
            {
                Marker.SetActive(false);
            }
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Barque>() != null)
            {
                if(FirstSection)
                {
                    FirstSection.enabled = true;
                    FirstSection.ToturialStarted();
                    gameObject.SetActive(false);
                  
                }
                else if (Middlesection )
                {
                    Middlesection.enabled = true;
                    Middlesection.ToturialStarted();
                    gameObject.SetActive(false);

                }
                else if (SecondEncounter)
                {
                    SecondEncounter.enabled = true;
                    SecondEncounter.ToturialStarted();
                    gameObject.SetActive(false);

                }

                //tutorial popup triggers once only
            }
        }
        
        //TODO(Gerald, 2025 07 14): ideas for improvement (more general):
        // - instead of OnTriggerEnter ... if other.GetComponent<Barque>()
        //   i could do a base class that listens for a Condition
        //   and that condition could be customized in a inherting class.
        //   maybe. kinda odd to have a class for every kind of condition?
        //   maybe not.
        //   like trigger zone with a who-entered parameter
        // - some kind of tutorial history to go back and re-read lessons
    }

}
