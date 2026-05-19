using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool isAttacking;
    private EnemyAI enemyAI;

    public float Damage => damage;
    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        if (!enemyAI)
        {
            enemyAI = GetComponentInParent<EnemyAI>();
            if (!enemyAI)
            {
                enemyAI = GetComponentInChildren<EnemyAI>();
            }
        }
    }

    public void BeginAttack()
    {
        isAttacking = true;
        if (enemyAI)
        {
            enemyAI.NotifyAttack();
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
