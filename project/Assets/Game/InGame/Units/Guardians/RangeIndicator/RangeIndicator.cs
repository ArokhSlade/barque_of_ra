using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class RangeIndicator : MonoBehaviour
{
    [SerializeField] InputActionReference RangeToggle;

    [SerializeField] bool OnlyHoverView;
    [SerializeField] bool IsEnabled;

    public Action<string> OnStateChanged;

    [SerializeField] SpriteRenderer IndicatorGraphic;
    [SerializeField]
    [Range(1, 12)] float Scale;
    [SerializeField] Color InCombatColor, OutCombatColor;

    private void Start()
    {

        RangeToggle.action.started += changeToggleRangeUI;
        OnStateChanged += changeRangeIndictorColor;
        IndicatorGraphic.gameObject.transform.localScale = new Vector3(Scale, Scale, Scale);
        changeRangeIndictorColor("Idle");


    }
    void changeToggleRangeUI(InputAction.CallbackContext context)
    {
        IsEnabled = !IsEnabled;
        IndicatorGraphic.enabled = IsEnabled;
    }
    public void changeRangeIndictorColor(string Combat)
    {
        if (Combat == "Attack")
        {
            IndicatorGraphic.color = InCombatColor;
        }
        else if (Combat == "Idle")
        {
            IndicatorGraphic.color = OutCombatColor;
        }
        // else { this.gameObject.SetActive(false); }
    }
}
