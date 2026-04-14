using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    private Animator animator;
    private AttackHitBoxController hitBoxController;

    [SerializeField] private float ligthCost = 15f;
    [SerializeField] private float heavyCost = 35f;

    [SerializeField] private bool isLight;

    [SerializeField] private CameraShakeController cameraShake;

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

                OnLightAttackHit(); //Esto para prepararlo para que sea solo en el impacto
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

                OnHeavyAttackHit(); //Esto para prepararlo para que sea solo en el impacto
            }
        }
    }

    public void OnLightAttackHit()
    {
        cameraShake.Shake(1f, isLight = true);
    }

    public void OnHeavyAttackHit()
    {
        cameraShake.Shake(1.3f, isLight = false);
    }
}