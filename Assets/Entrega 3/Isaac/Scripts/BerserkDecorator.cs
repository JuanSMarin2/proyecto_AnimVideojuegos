using UnityEngine;

public class BerserkDecorator : PowerUpDecorator
{
    [SerializeField] private float animatorMultiplier = 1.5f;

    private Animator animator;

    private float originalSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Apply()
    {
        Debug.Log("BERSERK ACTIVADO");

        if (animator != null)
        {
            originalSpeed = animator.speed;

            animator.speed *= animatorMultiplier;
        }
    }

    public override void Remove()
    {
        Debug.Log("BERSERK TERMINADO");

        if (animator != null)
        {
            animator.speed = originalSpeed;
        }
    }
}