using BarqueOfRa.Game;
using UnityEngine;

[System.Serializable]
public class BarqueAttackPointProvider
{
    [SerializeField] AttackPoint2.AttackPointID attackPointID;

    public Transform AcquireTargetTransform()
    {
        if (InGame.Instance == null)
        {
            Debug.LogError("InGame.Instance required");
            return null;
        }
        Barque barque = InGame.Instance.Barque;
        if (InGame.Instance == null)
        {
            Debug.LogError("missing Barque");
            return null;
        }
        AttackPoint2 attackPoint = barque.GetAttackPoint(attackPointID);

        return attackPoint.transform;
    }
}
