using System.Collections;
using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private HealthController healthController;

    [Header("Damage Triggers")]
    [SerializeField] private string damageFrontTrigger = "Front";
    [SerializeField] private string damageBackTrigger = "Back";
    [SerializeField] private string damageLeftTrigger = "Left";
    [SerializeField] private string damageRightTrigger = "Right";

    [Header("Hitboxes")]
    [SerializeField] private EnemyDirectionalHitbox[] hitboxes;

    [Header("Damage Settings")]
    [SerializeField] private float invulnerabilityDuration = 0.5f;

    private bool invulnerable;
    private Coroutine invulnerabilityRoutine;

    private int damageFrontHash;
    private int damageBackHash;
    private int damageLeftHash;
    private int damageRightHash;

    public bool IsInvulnerable => invulnerable;

    private void Awake()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }

        if (!healthController)
        {
            healthController = GetComponent<HealthController>();
        }

        damageFrontHash = Animator.StringToHash(damageFrontTrigger);
        damageBackHash = Animator.StringToHash(damageBackTrigger);
        damageLeftHash = Animator.StringToHash(damageLeftTrigger);
        damageRightHash = Animator.StringToHash(damageRightTrigger);

        RegisterHitboxes();
    }

    private void RegisterHitboxes()
    {
        if (hitboxes == null)
        {
            return;
        }

        for (int i = 0; i < hitboxes.Length; i++)
        {
            EnemyDirectionalHitbox hitbox = hitboxes[i];
            if (hitbox)
            {
                hitbox.SetReceiver(this);
            }
        }
    }

    public void ReceiveHit(DamageInfo info)
    {
        if (!healthController || healthController.IsDead)
        {
            return;
        }

        if (invulnerable)
        {
            return;
        }

        invulnerable = true;
        TriggerDamageAnimation(info.Direction);
        healthController.ApplyDamage(info);

        if (invulnerabilityRoutine != null)
        {
            StopCoroutine(invulnerabilityRoutine);
        }

        invulnerabilityRoutine = StartCoroutine(InvulnerabilityTimer());
    }

    private void TriggerDamageAnimation(HitDirection direction)
    {
        if (!animator)
        {
            return;
        }

        switch (direction)
        {
            case HitDirection.Front:
                animator.SetTrigger(damageFrontHash);
                break;
            case HitDirection.Back:
                animator.SetTrigger(damageBackHash);
                break;
            case HitDirection.Left:
                animator.SetTrigger(damageLeftHash);
                break;
            case HitDirection.Right:
                animator.SetTrigger(damageRightHash);
                break;
        }
    }

    private IEnumerator InvulnerabilityTimer()
    {
        yield return new WaitForSeconds(invulnerabilityDuration);
        invulnerable = false;
    }
}
