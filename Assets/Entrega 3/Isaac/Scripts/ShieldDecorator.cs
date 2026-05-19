using UnityEngine;
public class ShieldDecorator : PowerUpDecorator, IPlayerStatsDecorator
{
    private PlayerStatsContext statsContext;
    private IPlayerStats inner;
    private Renderer[] renderers;

    public IPlayerStats Inner => inner;
    public float MoveSpeedMultiplier => inner != null ? inner.MoveSpeedMultiplier : 1f;
    public float DamageMultiplier => inner != null ? inner.DamageMultiplier : 1f;
    public bool IsInvulnerable => true;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
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
        Debug.Log("ESCUDO ACTIVADO");

        statsContext.AddDecorator(this);

        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_EmissionColor"))
            {
                rend.material.EnableKeyword("_EMISSION");
            }
        }
    }

    public override void Remove()
    {
        Debug.Log("ESCUDO TERMINADO");

        statsContext.RemoveDecorator(this);

        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_EmissionColor"))
            {
                rend.material.DisableKeyword("_EMISSION");
            }
        }
    }
}