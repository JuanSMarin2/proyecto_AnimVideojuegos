using UnityEngine;
using UnityEngine.AI;

namespace Clases.Clase_8.Scripts.States
{
    public class AttackState : State
    {
        private readonly ComboSequence _combo;
        private NavMeshAgent _agent;
        private ComboExecutor _executor;
        private float _cooldownTimer;

        public AttackState(EnemyAI enemy, ComboSequence combo) : base(enemy)
        {
            _combo = combo;
        }

        public override void Enter()
        {
            _agent = enemy.agent;
            _agent.isStopped = true;
            _agent.ResetPath();

            _executor = enemy.GetComponentInChildren<ComboExecutor>(true);
            if (_executor == null)
            {
                var anim = enemy.GetComponentInChildren<Animator>(true);
                if (anim == null)
                {
                    enemy.ChangeState(new IdleState(enemy));
                    return;
                }
                _executor = anim.gameObject.AddComponent<ComboExecutor>();
                _executor.animator = anim;
            }

            _cooldownTimer = 0f;
            _executor.PlayCombo(_combo);
        }

        public override void Update()
        {
            if (enemy.player == null)
            {
                enemy.ChangeState(new IdleState(enemy));
                return;
            }

            if (!_executor.IsBusy())
            {
                enemy.ChangeState(new ChaseState(enemy));
            }
        }

        public override void Exit()
        {
            if (_executor != null) _executor.Cancel();
        }
    }
}