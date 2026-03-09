using BarqueOfRa.Game.UI;
using TMPro;
using UnityEngine;
public class BattleStartScreen : Popup
{
    public enum LevelDiff
    {
        Normal,
        Hardmode
    }
    [SerializeField] float Timer_Normal_ , Timer_Hardmode;
     public LevelDiff TimerType { get; set; }
    
    float timerCountDown;
    [SerializeField] TextMeshProUGUI TimerCounter;


    public void Init(string To)
    {
        Debug.LogError(To);
        if("Normal" == To)
        {

            Debug.LogError("To");

            timerCountDown = Timer_Normal_;
            TimerType = LevelDiff.Normal;
        }
        else if("Hard" == To)
        {

            TimerType = LevelDiff.Hardmode;
            timerCountDown = Timer_Hardmode;
        }
    }
    private void Update()
    {
        if (timerCountDown > 0)
        {
            timerCountDown -= Time.deltaTime;
            TimerCounter.text = Mathf.RoundToInt(timerCountDown).ToString();
        }
    }
    public override void Open()
    {
        base.Open();
        ReCalculate();

    }
    private void ReCalculate()
    {
        if (TimerType == LevelDiff.Normal)
        {
            timerCountDown = Timer_Normal_;
            Invoke("Close", Timer_Normal_);

        }
        else if (TimerType == LevelDiff.Hardmode)
        {
            timerCountDown = Timer_Hardmode;
            Invoke("Close", Timer_Hardmode);

        }
    }
}
