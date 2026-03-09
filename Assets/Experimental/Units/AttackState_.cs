using UnityEngine;

namespace BarqueOfRa.Experimental.Units
{

    public abstract class AttackState_<UnitType, TEnum> : UnitState_<UnitType, TEnum> 
        where UnitType:AttackStateOwner_
        where TEnum : System.Enum
    {        
        float attackSecondsLeft;
        float attackSeconds;
        int attackDamage;
        DamageTaker targetDamageTaker;

        override public void OnStateEnter(UnitType unit)
        {
            AttackStateOwner_.InitData initData = unit.AttackStateInitData;
            attackSeconds = initData.attackSeconds;
            Debug.Assert(attackSeconds > 0f, "attack seconds must be greater than 0!");

            attackDamage = initData.attackDamage;
            Debug.Assert(attackDamage > 0f, "attack damage must be greater than 0!");

            targetDamageTaker = initData.damageTaker;
            Debug.Assert(targetDamageTaker != null, "target DamageTaker was null");

            attackSecondsLeft = attackSeconds;
        }

        override public void OnStateUpdate(UnitType unit)
        {
            if (attackSecondsLeft <= 0f)
            {
                // TODO(Gerald, 2025 07 23): should be triggered by animation event                
                targetDamageTaker.TakeDamage(attackDamage);

                attackSecondsLeft = attackSeconds;
            }
            else
            {
                attackSecondsLeft -= Time.deltaTime;
            }
        }
        
        override public void OnStateExit(UnitType unit)
        {
            targetDamageTaker = null;
        }
    }
}