using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitScalingBuffUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buffLabel;
    [SerializeField] Image Icon;

    bool Active;
    public bool ShowingBuffScale { set { Active = value; DisplayingValue(); } }

    public void DisplayingValue()
    {
        Icon.enabled = Active;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(Active);
    }
    public void UpdateBuffLabel(float powerBonusRatio)
    {   

        float buffPercentage = 0f + powerBonusRatio;
        buffPercentage *= 100f;
        int buffPercentageInt = (int)buffPercentage;
        buffLabel.text = $"{buffPercentageInt}%";
    }
}
