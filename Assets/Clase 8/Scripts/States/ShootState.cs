using UnityEngine;
using UnityEngine.AI;

namespace Clases.Clase_8.Scripts.States
{
    public class ShootState : State
    {
        private NavMeshAgent _agent;
        private float _cooldownTimer;
        private Animator _animator;
        private int _attackTriggerHash;

        private const string ATTACK_TRIGGER = "Attack";

        public ShootState(EnemyAI enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            _agent = enemy.agent;
            if (_agent)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }

            _animator = enemy.animator ? enemy.animator : enemy.GetComponentInChildren<Animator>(true);
            _attackTriggerHash = Animator.StringToHash(ATTACK_TRIGGER);
            _cooldownTimer = enemy.ShootCooldown;
        }

        public override void Update()
        {
            if (enemy.player == null)
            {
                enemy.ChangeState(new IdleState(enemy));
                return;
            }

            if (!enemy.PlayerInShootRange())
            {
                enemy.ChangeState(new ChaseState(enemy));
                return;
            }

            Vector3 toPlayer = enemy.player.position - enemy.transform.position;
            toPlayer.y = 0f;
            if (toPlayer.sqrMagnitude > 0.0001f)
            {
                Quaternion look = Quaternion.LookRotation(toPlayer);
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, look, enemy.rotationSmooth * Time.deltaTime);
            }

            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer >= enemy.ShootCooldown)
            {
                _cooldownTimer = 0f;
                PlayAttackAnimation();
            }
        }

        public override void Exit()
        {
            if (_agent)
            {
                _agent.isStopped = false;
            }
        }

        private void PlayAttackAnimation()
        {
            if (_animator)
            {
                _animator.ResetTrigger(_attackTriggerHash);
                _animator.SetTrigger(_attackTriggerHash);
            }
        }

    }
}
