using UnityEngine;

public class MageShootStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private float shootDelay = 0.5f;
    [SerializeField] private bool cancelOnExit = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyAI enemy = animator.GetComponent<EnemyAI>();
        if (!enemy)
        {
            enemy = animator.GetComponentInParent<EnemyAI>();
        }

        if (!enemy)
        {
            return;
        }

        float delay = shootDelay;
        if (delay < 0f)
        {
            delay = 0f;
        }

        enemy.ScheduleFireball(delay);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!cancelOnExit)
        {
            return;
        }

        EnemyAI enemy = animator.GetComponent<EnemyAI>();
        if (!enemy)
        {
            enemy = animator.GetComponentInParent<EnemyAI>();
        }

        if (!enemy)
        {
            return;
        }

        enemy.CancelScheduledFireball();
    }
}
