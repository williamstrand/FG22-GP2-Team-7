using UnityEngine;

public class CatapultLoadStateMachineBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<Catapult>().LoadComplete();
    }
}
