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
        if(enemy.IsMage && enemy.PlayerInShootRange())
        {
            enemy.ChangeState(new Clases.Clase_8.Scripts.States.ShootState(enemy));
            return;
        }

        if(enemy.PlayerInRange(5f))
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.2f)
        {
            enemy.NextWayPoint();
        }
    }

	public override void Exit()
	{
	}
}
