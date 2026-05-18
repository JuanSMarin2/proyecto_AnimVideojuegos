using UnityEngine;

public class EnemyDirectionalHitbox : MonoBehaviour
{
    [SerializeField] private HitDirection direction;
    [SerializeField] private Collider hitboxCollider;
    [SerializeField] private EnemyDamageReceiver damageReceiver;
    [SerializeField] private string playerAttackTag = "PlayerAttack";
    [SerializeField] private float damageMultiplier = 1f;

    private void Awake()
    {
        if (hitboxCollider && !hitboxCollider.isTrigger)
        {
            hitboxCollider.isTrigger = true;
        }
    }

    public void SetReceiver(EnemyDamageReceiver receiver)
    {
        damageReceiver = receiver;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerAttackTag))
        {
            return;
        }

        if (!damageReceiver)
        {
            return;
        }

        PlayerAttackSource attackSource = other.GetComponent<PlayerAttackSource>();
        if (!attackSource)
        {
            attackSource = other.GetComponentInParent<PlayerAttackSource>();
        }

        if (!attackSource)
        {
            return;
        }

        if (!attackSource.IsActive)
        {
            return;
        }

        float finalDamage = attackSource.GetDamage() * Mathf.Max(0f, damageMultiplier);
        DamageInfo info = new DamageInfo(finalDamage, direction, other.gameObject);
        damageReceiver.ReceiveHit(info);
    }
}
