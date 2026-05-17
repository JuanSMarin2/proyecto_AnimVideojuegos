using UnityEngine;
using UnityEngine.UI;

public class VidaController : MonoBehaviour
{
    [Header("Referencias")]
    protected HealthController playerHealth; //Necesista HealthController en la otra rama

    [Header("UI")]
    public Image healthBar;

    [Header("Suavizado")]
    protected float smoothSpeed = 5f;

    private float targetFill;

    void Update()
    {
        if (playerHealth == null)
            return;

        // Obtener porcentaje de vida REAL
        targetFill =
            playerHealth.CurrentHealth /
            playerHealth.MaxHealth;

        // Interpolación suave
        healthBar.fillAmount = Mathf.Lerp(
            healthBar.fillAmount,
            targetFill,
            Time.deltaTime * smoothSpeed
        );
    }
}