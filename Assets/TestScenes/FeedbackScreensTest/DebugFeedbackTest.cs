using UnityEngine;
using UnityEngine.InputSystem;

public class DebugFeedbackTest : MonoBehaviour
{


    [SerializeField] InputActionReference debugActionRef;
    InputAction debugAction;

    void Awake()
    {
        debugAction = debugActionRef;
        debugAction.performed += TestBattleWonScreen;
    }

    void TestBattleWonScreen(InputAction.CallbackContext context)
    {
        HUD.Instance.ShowBattleWonScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
