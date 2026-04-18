using UnityEngine;
using UnityEngine.Animations;

namespace Clases.Clase_7.Scripts
{
    public class EnableRagdollOnExit : StateMachineBehaviour
    {
        [Range(0.8f, 1.0f)] public float normalizedTime = 0.98f;
        private bool fired;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            fired = false;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (fired) return;
            if (stateInfo.normalizedTime >= normalizedTime)
            {
                fired = true;
                var rc = animator.GetComponent<RagDollController>();
                if (rc) rc.EnableRagDoll();
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (fired) return;
            var rc = animator.GetComponent<RagDollController>();
            if (rc) rc.EnableRagDoll();
        }
    }
}