using BarqueOfRa.Units;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Guardian : MonoBehaviour, IUnit
{
    public UnitType UnitType => unit.Type;

    /// <summary>
    ///  summon the unit at the correct offset from the ground
    /// </summary>
    public Vector3 Offset => unit.Offset;
    
    Unit unit;

    private void Reset()
    {
        unit = GetComponent<Unit>();
    }

}
