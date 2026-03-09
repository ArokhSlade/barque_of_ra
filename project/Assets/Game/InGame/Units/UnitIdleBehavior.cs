using UnityEngine;

public class UnitIdleBehavior : StateMachineBehaviour
{
    float timer;
    bool timerActive = true;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;

        animator.SetBool("isApproaching", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timerActive && timer > 2)
        {
            animator.SetBool("isApproaching", true);
            timerActive = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
