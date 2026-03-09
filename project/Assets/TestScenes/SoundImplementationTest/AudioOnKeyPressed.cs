using UnityEngine;
using UnityEngine.InputSystem;


namespace BarqueOfRa.Test.AudioImplementationTest
{
    public class AudioOnKeyPressed : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;

        [SerializeField] InputActionReference actionHandledInUpdateRef;
        [SerializeField] InputActionReference actionHandledByEventRef;

        InputAction actionHandledInUpdate;
        InputAction actionHandledByEvent;
    
        bool actionHandledInUpdatePerformed => actionHandledInUpdate.WasPerformedThisFrame();
    
        void Awake()
        {
            actionHandledInUpdate = actionHandledInUpdateRef;
            actionHandledByEvent = actionHandledByEventRef;
            actionHandledInUpdate.actionMap.Enable();
        }

        void OnEnable()
        {
            actionHandledByEvent.performed += Confirm;
        }

        void OnDisable()
        {
            actionHandledByEvent.performed -= Confirm;
        }

        void Confirm(InputAction.CallbackContext context)
        {
            Debug.Log("Action Handled by Event");
            audioSource.Play();
        }

        void Update()
        {   
            if (actionHandledInUpdatePerformed)
            {
                Debug.Log("Action Handled in Update");
                audioSource.Play();
            }
        }
    }
}

