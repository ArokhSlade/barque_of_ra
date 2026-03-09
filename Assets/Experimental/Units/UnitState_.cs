using UnityEngine;


namespace BarqueOfRa.Experimental.Units
{   
    public abstract class UnitState_<TUnitType, TEnum> : MonoBehaviour where TEnum : System.Enum
    {
        abstract public TEnum ID { get; }

        virtual public void OnStateEnter(TUnitType unit)
        {
            //Debug.Log($"Entering {this} : {idName(ID)}");
        }

        virtual public void OnStateUpdate(TUnitType unit)
        {
            //Debug.Log($"Updating {this} : {idName(ID)}");
        }

        virtual public void OnStateExit(TUnitType unit)
        {
            //Debug.Log($"Exiting {this} : {idName(ID)}");
        }

        
    }
}