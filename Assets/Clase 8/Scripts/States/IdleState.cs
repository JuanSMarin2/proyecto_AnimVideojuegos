
    using UnityEngine;
    public class IdleState : State
    {
        private float idleTime = 2f;
        private float timer;

        public IdleState(EnemyAI enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            timer = 0f;
            enemy.agent.isStopped = true;
        }

public override void Update()
{
    timer += Time.deltaTime;
    if (enemy.IsMage && enemy.PlayerInShootRange())
    {
        enemy.ChangeState(new Clases.Clase_8.Scripts.States.ShootState(enemy));
        return;
    }

    if (enemy.PlayerInRange(5f))
    {
        enemy.ChangeState(new ChaseState(enemy));
        return;
    }

    if (timer >= idleTime)
    {
        enemy.ChangeState(new PatrolState(enemy));
        return;
    }
}

        public override void Exit()
        {
            
        }
    }
