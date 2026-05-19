using UnityEngine;
using UnityEngine.UI;

public class VidaController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField]
    private HealthController playerHealth; 

    [Header("UI")]
    public Image healthBar;

    [Header("Suavizado")]
    protected float smoothSpeed = 5f;

    private float targetFill;

    void Update()
    {
        if (playerHealth == null)
            return;

      
        targetFill =
            playerHealth.CurrentHealth /
            playerHealth.MaxHealth;

        // Interpolaci�n suave
        healthBar.fillAmount = Mathf.Lerp(
            healthBar.fillAmount,
            targetFill,
            Time.deltaTime * smoothSpeed
        );
    }
}