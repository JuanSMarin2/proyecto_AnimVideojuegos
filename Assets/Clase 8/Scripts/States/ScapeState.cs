using UnityEngine;
using UnityEngine.AI;

namespace Clases.Clase_8.Scripts.States
{
    public class ScapeState : State
    {
        private readonly float duration;
        private readonly float distance;
        private float timer;
        private NavMeshAgent agent;

        public ScapeState(EnemyAI enemy, float duration, float distance) : base(enemy)
        {
            this.duration = Mathf.Max(0.1f, duration);
            this.distance = Mathf.Max(0.1f, distance);
        }

        public override void Enter()
        {
            agent = enemy.agent;
            timer = 0f;

            if (agent)
            {
                agent.isStopped = false;
                agent.speed = enemy.runSpeed;
                agent.ResetPath();
            }

            Vector3 away = GetAwayDirection();
            Vector3 target = enemy.transform.position + away * distance;

            if (agent && agent.isOnNavMesh)
            {
                if (NavMesh.SamplePosition(target, out var hit, distance, NavMesh.AllAreas))
                {
                    target = hit.position;
                }
                agent.SetDestination(target);
            }
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                enemy.ChangeState(new ChaseState(enemy));
            }
        }

        public override void Exit()
        {
        }

        private Vector3 GetAwayDirection()
        {
            Transform target = enemy.playerTarget ? enemy.playerTarget : enemy.player;
            if (target)
            {
                Vector3 away = enemy.transform.position - target.position;
                away.y = 0f;
                if (away.sqrMagnitude > 0.001f)
                {
                    return away.normalized;
                }
            }

            return -enemy.transform.forward;
        }
    }
}
