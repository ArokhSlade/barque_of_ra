using UnityEngine;

namespace BarqueOfRa.Game.UI.States.Combat
{   
    public abstract class CombatUIState : MonoBehaviour
    {
        protected PlayerCombatInput owner;
        protected Ray pointerRay;
        
        protected RaycastHit soulHit, slotHit, guardianHit, enemyHit, highlightableHit;
        protected bool wasSoulHit, wasSlotHit, wasGuardianHit, wasEnemyHit, wasHighlightableHit;

        protected Camera mainCamera;

        // TODO(Gerald 2025 08 03): no need to have local copies of owners properties. 
        // just a source of bugs.
        public virtual void Initialize(PlayerCombatInput owner)
        {
            this.owner = owner;
            mainCamera = Camera.main;
        }
        
        public virtual void CleanUp()
        {
        }
        
        public virtual CombatUIState StateUpdate()
        {
            
            pointerRay = mainCamera.ScreenPointToRay(owner.PointerPosition);
            
            wasSoulHit = Physics.Raycast(pointerRay, out soulHit, Mathf.Infinity, owner.SoulsLayer, QueryTriggerInteraction.Collide);
            wasSlotHit = Physics.Raycast(pointerRay, out slotHit, Mathf.Infinity, owner.SlotsLayer, QueryTriggerInteraction.Collide);
            wasGuardianHit = Physics.Raycast(pointerRay, out guardianHit, Mathf.Infinity, owner.GuardiansLayer, QueryTriggerInteraction.Collide);
            wasEnemyHit = Physics.Raycast(pointerRay, out enemyHit, Mathf.Infinity, owner.EnemiesLayer, QueryTriggerInteraction.Collide);
            wasHighlightableHit = Physics.Raycast(pointerRay, out highlightableHit, Mathf.Infinity, owner.HighlightablesLayer, QueryTriggerInteraction.Collide);

            return this;
        }
        
        public virtual void Enter()
        {
        }
        
        public virtual void Exit()
        {
        }
    }
    
}