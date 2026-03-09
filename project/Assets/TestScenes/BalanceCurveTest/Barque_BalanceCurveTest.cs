using BarqueOfRa.Game;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace BarqueOfRa.Test.BalanceCurveTest
{
    [RequireComponent (typeof(Navigator))]
    [RequireComponent (typeof(SoulCabin_BalanceCurveTest))]
    public class Barque_BalanceCurveTest : MonoBehaviour
    {
        public static Barque_BalanceCurveTest Instance
        {
            get
            {
                if (s_instance == null)
                {
                    Debug.LogError("no Barque_BalanceCurveTest found");
                }
                return s_instance;
            }
        }

        private static Barque_BalanceCurveTest s_instance;

        Navigator navigator;
        SoulCabin_BalanceCurveTest soulCabin;
        [SerializeField] DefenseSlots defenseSlots;

        public SoulCabin_BalanceCurveTest SoulCabin => soulCabin;

        public int SoulsCount => soulCabin.SoulsLeft;
        public int GuardiansCount => defenseSlots.GuardiansCount;

        void Awake()
        {
            s_instance = this;
            navigator = GetComponent<Navigator>();
            soulCabin = GetComponent<SoulCabin_BalanceCurveTest>();
        }

        public void Unload()
        {
            navigator.DisableNavAgent();
        }

        public void Initialize(Level level)
        {
            navigator.Initialize(level);
        }
    }
}