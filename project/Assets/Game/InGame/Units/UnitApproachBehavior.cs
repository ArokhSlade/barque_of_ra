using UnityEngine;

public class UnitApproachBehavior : StateMachineBehaviour
{
    IUnitBrain brain;
    IApproachProvider approachProvider;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.TryGetComponent(out approachProvider))
        {
            Debug.LogError($"{animator.gameObject} does not provide a component that implements IApproachProvider!");
        }

        if (!animator.TryGetComponent(out brain))
        {
            Debug.LogError($"{animator.gameObject} does not provide a component that implements IUnitBrain!");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        approachProvider.Approach();
        brain.Think();
    }
}
