using UnityEngine;

public class BerserkDecorator : PowerUpDecorator
{
    [SerializeField] private float damageMultiplier = 1.5f;

    private PlayerAttackSource[] attackSources;

    private float[] originalDamages;

    private Renderer[] renderers;

    private void Awake()
    {
        attackSources = GetComponentsInChildren<PlayerAttackSource>();

        renderers = GetComponentsInChildren<Renderer>();

        originalDamages = new float[attackSources.Length];
    }

    public override void Apply()
    {
        Debug.Log("BERSERK ACTIVADO");

        for (int i = 0; i < attackSources.Length; i++)
        {
            PlayerAttackSource source = attackSources[i];

            float currentDamage = source.GetDamage();

            originalDamages[i] = currentDamage;

            source.SetBaseDamage(currentDamage * damageMultiplier);
        }

        foreach(Renderer rend in renderers)
        {
            if(rend.material.HasProperty("_EmissionColor"))
            {
                rend.material.EnableKeyword("_EMISSION");
            }
        }
    }

    public override void Remove()
    {
        Debug.Log("BERSERK TERMINADO");

        for (int i = 0; i < attackSources.Length; i++)
        {
            attackSources[i].SetBaseDamage(originalDamages[i]);
        }

        foreach(Renderer rend in renderers)
        {
            if(rend.material.HasProperty("_EmissionColor"))
            {
                rend.material.DisableKeyword("_EMISSION");
            }
        }
    }
}