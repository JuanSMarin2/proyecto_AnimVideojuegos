using System.Collections;
using UnityEngine;

public enum HitDirection
{
    Front,
    Back,
    Left,
    Right
}

public struct DamageInfo
{
    public float Amount;
    public HitDirection Direction;
    public GameObject Source;

    public DamageInfo(float amount, HitDirection direction, GameObject source)
    {
        Amount = amount;
        Direction = direction;
        Source = source;
    }
}

public class PlayerDamageReceiver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private HealthController healthController;

    [Header("Hitboxes")]
    [SerializeField] private DirectionalHitbox[] hitboxes;

    [Header("Damage Settings")]
    [SerializeField] private float invulnerabilityDuration = 1f;

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

        damageFrontHash = Animator.StringToHash("DamageFront");
        damageBackHash = Animator.StringToHash("DamageBack");
        damageLeftHash = Animator.StringToHash("DamageLeft");
        damageRightHash = Animator.StringToHash("DamageRight");

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
            DirectionalHitbox hitbox = hitboxes[i];
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
