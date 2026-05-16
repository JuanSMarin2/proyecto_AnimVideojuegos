using UnityEngine;

public class ShieldDecorator : PowerUpDecorator
{
    private Renderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public override void Apply()
    {
        Debug.Log("ESCUDO ACTIVADO");

        foreach(Renderer rend in renderers)
        {
            rend.material.EnableKeyword("_EMISSION");
        }
    }

    public override void Remove()
    {
        Debug.Log("ESCUDO TERMINADO");

        foreach(Renderer rend in renderers)
        {
            rend.material.DisableKeyword("_EMISSION");
        }
    }
}