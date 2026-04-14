using UnityEngine;

public class PatrolState : State
{
	public PatrolState(EnemyAI enemy) : base(enemy)
	{
	}

	public override void Enter()
    {
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.walkSpeed;
        enemy.NextWayPoint();

    }

	public override void Update()
    {
        if(enemy.PlayerInRange(5f))
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.05f)
        {
            enemy.NextWayPoint();
        }
    }

	public override void Exit()
	{
	}
}
