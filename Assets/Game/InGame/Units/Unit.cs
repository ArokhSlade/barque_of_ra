using BarqueOfRa.Units;
using UnityEngine;


namespace BarqueOfRa.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] UnitType type;
        [SerializeField] Vector3 offset;

        public UnitType Type => type;
        public Vector3 Offset => offset;

    }

    public enum UnitType
    {
        None = 0,
        Soul = 10,
        GuardianMelee = 50,
        GuardianMelee_Brawler = 51,
        GuardianMelee_Assassin = 52,
        GuardianMelee_Tank = 53,
        GuardianRanged = 70
    }

    public interface IUnit
    {
        public UnitType UnitType { get; }
        public Vector3 Offset { get; }
    }
}