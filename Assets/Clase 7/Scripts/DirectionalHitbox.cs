using UnityEngine;

public class DirectionalHitbox : MonoBehaviour
{
    [SerializeField] private HitDirection direction;
    [SerializeField] private Collider hitboxCollider;
    [SerializeField] private PlayerDamageReceiver damageReceiver;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float damageMultiplier = 1f;

    private void Awake()
    {
        if (hitboxCollider && !hitboxCollider.isTrigger)
        {
            hitboxCollider.isTrigger = true;
        }
    }

    public void SetReceiver(PlayerDamageReceiver receiver)
    {
        damageReceiver = receiver;
    }

    public void ReceiveProjectile(float damage, GameObject source)
    {
        if (!damageReceiver || damageReceiver.IsInvulnerable)
        {
            return;
        }

        float finalDamage = Mathf.Max(0f, damage) * Mathf.Max(0f, damageMultiplier);
        DamageInfo info = new DamageInfo(finalDamage, direction, source);
        damageReceiver.ReceiveHit(info);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(enemyTag))
        {
            return;
        }

        if (!damageReceiver || damageReceiver.IsInvulnerable)
        {
            return;
        }

        EnemyAttack enemyAttack = other.GetComponent<EnemyAttack>();
        if (!enemyAttack)
        {
            enemyAttack = other.GetComponentInParent<EnemyAttack>();
        }
        if (!enemyAttack)
        {
            enemyAttack = other.transform.root.GetComponentInChildren<EnemyAttack>();
        }

        if (!enemyAttack)
        {
            return;
        }

        if (!enemyAttack.IsAttacking)
        {
            return;
        }

        float finalDamage = enemyAttack.Damage * Mathf.Max(0f, damageMultiplier);
        DamageInfo info = new DamageInfo(finalDamage, direction, other.gameObject);
        damageReceiver.ReceiveHit(info);
    }
}
