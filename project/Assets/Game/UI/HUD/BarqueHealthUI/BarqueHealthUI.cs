using TMPro;
using UnityEngine;

public class BarqueHealthUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpLabel;
    public void UpdateHP(int newHP)
    {
        hpLabel.text = $"{newHP}";
    }
}
