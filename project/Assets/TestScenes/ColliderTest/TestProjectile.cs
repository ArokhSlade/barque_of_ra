using UnityEngine;
using UnityEngine.Events;

namespace BarqueOfRa.Tests.ColliderTest
{
    public class TestProjectile : MonoBehaviour
    {
        public UnityEvent TriggerEnterEvent;
        
        void OnEnable()
        {
            TriggerEnterEvent?.AddListener(HandleTriggerEnterEvent);
        }
        
        void OnDisable()
        {
            TriggerEnterEvent?.RemoveListener(HandleTriggerEnterEvent);
        }
        
        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter {other}");
            TriggerEnterEvent.Invoke();
        }
        
        void HandleTriggerEnterEvent()
        {
            Debug.Log($"TriggerEnterEvent handled");
        }
        
    }   
        
}