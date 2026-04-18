using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Clases.Clase_2.Scripts
{
    public class AttackController : MonoBehaviour
    {
        private Animator animator;
        private AttackHitBoxController hitBoxController;

        [SerializeField] private float ligthCost = 15f;
        [SerializeField] private float heavyCost = 35f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            hitBoxController = GetComponent<AttackHitBoxController>();
        }

        public void OnLightAttack(InputAction.CallbackContext context)
        {

            if (context.performed)
            {
                if (Game.Instance.PlayerOne.CurrentStamina > 0)
                {
                    Game.Instance.PlayerOne.DepleteStamina(ligthCost);
                    animator.SetTrigger("Attack");
                }

            }

        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (Game.Instance.PlayerOne.CurrentStamina > 0)
                {
                    Game.Instance.PlayerOne.DepleteStamina(heavyCost);
                    animator.SetTrigger("HeavyAttack");
                }

            }
        }

        public void DepleteStamina(float amount)
        {
            Game.Instance.PlayerOne.DepleteStamina(amount);
        }

        public void DepleteStaminaWithParameter(string parameter)
        {
            float motionValue = GetComponent<Animator>().GetFloat(parameter);
            DepleteStamina(motionValue);
        }

        public void ToggleAttackHitBox(int hitboxId)
        {
            hitBoxController.ToggleHitboxes(hitboxId);
        }

        public void CleanUpAttackHitBox()
        {
            hitBoxController.CleanupHitBoxes();
        }
    }
}
