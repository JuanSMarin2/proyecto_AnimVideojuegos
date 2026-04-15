using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Entrega_3.Esteban.Scripts
{
    internal class CleanupHitbox : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.SendMessage("CleanUpAttackHitBox");
            base.OnStateExit(animator, stateInfo, layerIndex);
            AttackHitBoxController hitBoxController = animator.GetComponent<AttackHitBoxController>();
            hitBoxController.CleanupHitBoxes();
        }
    }
}
