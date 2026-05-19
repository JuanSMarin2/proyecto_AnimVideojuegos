using UnityEngine;

public class BasePlayerStats : MonoBehaviour, IPlayerStats
{
    [SerializeField] private float moveSpeedMultiplier = 1f;
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private bool invulnerable = false;

    public float MoveSpeedMultiplier => Mathf.Max(0f, moveSpeedMultiplier);
    public float DamageMultiplier => Mathf.Max(0f, damageMultiplier);
    public bool IsInvulnerable => invulnerable;
}
