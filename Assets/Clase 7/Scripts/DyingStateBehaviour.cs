using UnityEngine;

public class DyingStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private string dyingParam = "isDying";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator)
        {
            return;
        }

        animator.SetBool(dyingParam, true);
    }
}
