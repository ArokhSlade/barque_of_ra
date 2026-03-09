using BarqueOfRa.Game.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsScreen : FullScreenMenu
{
    [SerializeField] InputActionReference cancelActionRef;
    [SerializeField] Animator animator;


    InputAction cancelAction;

    protected override void Awake()
    {
        base.Awake();
        cancelAction = cancelActionRef;
    }

    void OnEnable()
    {
        cancelAction.performed += OnCancelActionPerformed;
    }
    void OnDisable()
    {
        cancelAction.performed -= OnCancelActionPerformed;
    }

    public override void Open()
    {
        base.Open();
        
        animator.SetTrigger("OpenCredits");
    }

    public void OnCreditsFinished()
    {
        Close();
    }

    void OnCancelActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("cancel action performed");
        Close();
    }

}
