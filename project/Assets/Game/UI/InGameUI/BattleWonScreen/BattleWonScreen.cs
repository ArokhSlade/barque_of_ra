using UnityEngine;
using BarqueOfRa.Game.UI;
using Utility;
using System.Collections.Generic;
using BarqueOfRa.Game;
using NUnit.Framework;

public class BattleWonScreen : Popup
{
    [SerializeField] UnscaledTimer timer;

    [SerializeField] int BattleWonIndex;
    [SerializeField] GameObject BG;
    [SerializeField] GameObject[] BattlePopups = new GameObject[3];
    protected override void Awake()
    {
        BG.gameObject.SetActive(false);
        BattlePopups[0].SetActive(false);
        BattlePopups[1].SetActive(false);
        BattlePopups[2].SetActive(false);
    }
    public void ResetBattleWonIndex()
    {
        BattleWonIndex = 0;
    }
    public override void Open()
    {
        base.Open();
        timer.Reset();

        if (BattleWonIndex >= 2)
        {
            BG.gameObject.SetActive(true);
            BattlePopups[BattleWonIndex].SetActive(true);
            BattlePopups[BattleWonIndex].GetComponent<Animation>().Play();

            return;
        }
        BG.gameObject.SetActive(true);
        BattlePopups[BattleWonIndex].SetActive(true);
        BattlePopups[BattleWonIndex].GetComponent<Animation>().Play();
        BattleWonIndex++;

        

    }

    void Update()
    {
        timer.Update();
    }

    public override void Close()
    {
        BG.gameObject.SetActive(false);
        BattlePopups[0].SetActive(false);
        BattlePopups[1].SetActive(false);
        BattlePopups[2].SetActive(false);
        base.Close();
    }
}
