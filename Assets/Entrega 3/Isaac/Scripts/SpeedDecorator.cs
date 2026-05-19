using UnityEngine;
public class SpeedDecorator : PowerUpDecorator, IPlayerStatsDecorator
{
    [SerializeField] private float multiplier = 1.5f;

    private PlayerStatsContext statsContext;
    private IPlayerStats inner;

    public IPlayerStats Inner => inner;
    public float MoveSpeedMultiplier =>
        (inner != null ? inner.MoveSpeedMultiplier : 1f) * multiplier;
    public float DamageMultiplier => inner != null ? inner.DamageMultiplier : 1f;
    public bool IsInvulnerable => inner != null && inner.IsInvulnerable;

    private void Awake()
    {
        statsContext = GetComponentInParent<PlayerStatsContext>();
        if (statsContext == null)
        {
            statsContext = gameObject.AddComponent<PlayerStatsContext>();
        }
    }

    public void SetInner(IPlayerStats innerStats)
    {
        inner = innerStats;
    }

    public override void Apply()
    {
        statsContext.AddDecorator(this);

        Debug.Log("SPEED BOOST ACTIVADO");
    }

    public override void Remove()
    {
        statsContext.RemoveDecorator(this);

        Debug.Log("SPEED BOOST TERMINADO");
    }
}