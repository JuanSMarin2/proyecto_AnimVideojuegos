using UnityEngine;

public class AttackStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private bool affectPlayerAttack = true;
    [SerializeField] private bool affectEnemyAttack = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (affectPlayerAttack)
        {
            AttackController controller = animator.GetComponent<AttackController>();
            if (!controller)
            {
                controller = animator.GetComponentInParent<AttackController>();
            }
            if (controller)
            {
                controller.SetAttackWindowActive(true);
            }
        }

        if (affectEnemyAttack)
        {
            EnemyAttack enemyAttack = animator.GetComponent<EnemyAttack>();
            if (!enemyAttack)
            {
                enemyAttack = animator.GetComponentInParent<EnemyAttack>();
            }
            if (!enemyAttack)
            {
                enemyAttack = animator.GetComponentInChildren<EnemyAttack>();
            }
            if (enemyAttack)
            {
                enemyAttack.BeginAttack();
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (affectPlayerAttack)
        {
            AttackController controller = animator.GetComponent<AttackController>();
            if (!controller)
            {
                controller = animator.GetComponentInParent<AttackController>();
            }
            if (controller)
            {
                controller.SetAttackWindowActive(false);
            }
        }

        if (affectEnemyAttack)
        {
            EnemyAttack enemyAttack = animator.GetComponent<EnemyAttack>();
            if (!enemyAttack)
            {
                enemyAttack = animator.GetComponentInParent<EnemyAttack>();
            }
            if (!enemyAttack)
            {
                enemyAttack = animator.GetComponentInChildren<EnemyAttack>();
            }
            if (enemyAttack)
            {
                enemyAttack.EndAttack();
            }
        }
    }
}
