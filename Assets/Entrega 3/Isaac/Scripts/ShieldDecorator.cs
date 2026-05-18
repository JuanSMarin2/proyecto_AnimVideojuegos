using UnityEngine;
using System.Reflection;

public class ShieldDecorator : PowerUpDecorator
{
    private PlayerDamageReceiver damageReceiver;

    private FieldInfo invulnerableField;

    private Renderer[] renderers;

    private void Awake()
    {
        damageReceiver = GetComponent<PlayerDamageReceiver>();

        renderers = GetComponentsInChildren<Renderer>();

        invulnerableField = typeof(PlayerDamageReceiver)
            .GetField("invulnerable", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public override void Apply()
    {
        Debug.Log("ESCUDO ACTIVADO");

        if (invulnerableField != null && damageReceiver != null)
        {
            invulnerableField.SetValue(damageReceiver, true);
        }

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

        if (invulnerableField != null && damageReceiver != null)
        {
            invulnerableField.SetValue(damageReceiver, false);
        }

        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_EmissionColor"))
            {
                rend.material.DisableKeyword("_EMISSION");
            }
        }
    }
}