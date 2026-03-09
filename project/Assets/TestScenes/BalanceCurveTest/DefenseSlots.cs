using System.Collections.Generic;
using UnityEngine;
using BarqueOfRa.Game.UI;

namespace BarqueOfRa.Test.BalanceCurveTest
{
    public class DefenseSlots : MonoBehaviour
    {
        [SerializeField] List<DefenseSlot> slots = new();
        public int GuardiansCount => countGuardians();

        int countGuardians()
        {
            int result = 0;

            foreach (DefenseSlot slot in slots)
            {
                if (slot.GetComponentInChildren<Guardian>() != null)
                {
                    result += 1;
                }
            }

            return result;
        }
    

    }

}
