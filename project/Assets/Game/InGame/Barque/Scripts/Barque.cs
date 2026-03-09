using BarqueOfRa.Game;
using BarqueOfRa.Game.Units;
using BarqueOfRa.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Navigator))]
[RequireComponent(typeof(SoulCabin))]
public class Barque : MonoBehaviour
{
    Navigator navigator;
    SoulCabin soulCabin;
    public UnityEvent Died;
    public UnityEvent<int> SoulCountChanged;
    public UnityEvent<float> PowerBonusRatioChanged;

    [SerializeField] AnimationCurve attackDamagePerDeploymentRatio;

    [SerializeField] List<AttackPoint2> attackPoints = new();

    [SerializeField] DefenseSlots defenseSlots;
    public DefenseSlots DefenseSlots => defenseSlots;

    public SoulCabin SoulCabin => soulCabin;

    public int SoulsCount => soulCabin.SoulsLeft;
    public int GuardiansCount => defenseSlots.GuardiansCount;

    public List<Guardian> AliveGuardians => DefenseSlots.GetAllGuardians();
    public List<Soul> AliveSouls
    {
        get
        {
            List<Soul> souls = new List<Soul>();
            for (int A = 0; A < 8; A++)
            {
                souls.Add(this.transform.Find("SoulsContainer").transform.GetChild(A).GetComponent<Soul>());
            }
            return souls;
        }
    }

    float oldPowerBonusRatio = 1f;

    public AttackPoint2 GetAttackPoint(AttackPoint2.AttackPointID attackPointID)
    {
        AttackPoint2 result = null;

        foreach(var attackPoint in attackPoints)
        {
            if (attackPoint.ID == attackPointID)
            {
                result = attackPoint;
                break;
            }
        }
        return result;
    }

    void Awake()
    {
        navigator = GetComponent<Navigator>();
        soulCabin = GetComponent<SoulCabin>();
        soulCabin.AliveSoulsUpdated.AddListener(OnSoulCabinAliveSoulsUpdated);
    }

    void OnDestroy()
    {
        soulCabin.AliveSoulsUpdated.RemoveListener(OnSoulCabinAliveSoulsUpdated);
    }

    public void Initialize(Level level)
    {
        navigator.Initialize(level);
    }

    public void OnHealthExhausted()
    {
        Died?.Invoke();
    }

    public void OnSoulCabinAliveSoulsUpdated(int souls)
    {
        SoulCountChanged?.Invoke(souls);
    }

    public float PowerBonusRatio
    {
        get
        {
            int guardiansCount = GuardiansCount;
            int soulsCount = SoulsCount;
            int totalUnitsCount = guardiansCount + soulsCount;
            float soulsPerTotalRatio = (float)soulsCount / totalUnitsCount;
            float powerBonusRatio = attackDamagePerDeploymentRatio.Evaluate(soulsPerTotalRatio);

            if (powerBonusRatio != oldPowerBonusRatio)
            {
                Debug.Log($"{soulsCount} / ( {soulsCount} + {guardiansCount} ) = {soulsPerTotalRatio} -> 1.0 + {powerBonusRatio}");
                PowerBonusRatioChanged?.Invoke(powerBonusRatio);
                oldPowerBonusRatio = powerBonusRatio;
            }

            return powerBonusRatio;
        }
    }
}
