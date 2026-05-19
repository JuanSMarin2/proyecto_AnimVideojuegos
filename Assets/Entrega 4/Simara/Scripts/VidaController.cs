using UnityEngine;
using UnityEngine.UI;

public class VidaController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField]
    private HealthController playerHealth; 
    [SerializeField]
    private HealthController[] playerHealths;
    [SerializeField]
    private string playerTag = "Player";

    [Header("UI")]
    public Image healthBar;

    [Header("Suavizado")]
    protected float smoothSpeed = 5f;

    private float targetFill;

    void Update()
    {
        HealthController activeHealth = ResolveActivePlayerHealth();

        if (activeHealth == null)
            return;

      
        targetFill =
            activeHealth.CurrentHealth /
            activeHealth.MaxHealth;

        // Interpolaci�n suave
        healthBar.fillAmount = Mathf.Lerp(
            healthBar.fillAmount,
            targetFill,
            Time.deltaTime * smoothSpeed
        );
    }

    private HealthController ResolveActivePlayerHealth()
    {
        if (playerHealth != null && playerHealth.gameObject.activeInHierarchy)
        {
            return playerHealth;
        }

        if (playerHealths != null && playerHealths.Length > 0)
        {
            for (int i = 0; i < playerHealths.Length; i++)
            {
                HealthController candidate = playerHealths[i];
                if (IsValidPlayer(candidate))
                {
                    playerHealth = candidate;
                    return candidate;
                }
            }
        }

        HealthController[] allHealths = FindObjectsOfType<HealthController>(true);
        for (int i = 0; i < allHealths.Length; i++)
        {
            HealthController candidate = allHealths[i];
            if (IsValidPlayer(candidate))
            {
                playerHealth = candidate;
                return candidate;
            }
        }

        return playerHealth;
    }

    private bool IsValidPlayer(HealthController candidate)
    {
        if (candidate == null || !candidate.gameObject.activeInHierarchy)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(playerTag) &&
            !candidate.CompareTag(playerTag))
        {
            return false;
        }

        return true;
    }
}