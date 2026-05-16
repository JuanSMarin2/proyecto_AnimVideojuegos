using UnityEngine;
using UnityEngine.UI;

public class VidaController : MonoBehaviour
{
    public Image healthBar;

    public float maxHealth = 100f;
    public float currentHealth;

    private float targetFill;

    public float smoothSpeed = 5f;

    void Start()
    {
        currentHealth = maxHealth;
        targetFill = 1f;
    }

    void Update()
    {
        // Interpolación suave
        healthBar.fillAmount = Mathf.Lerp(
            healthBar.fillAmount,
            targetFill,
            Time.deltaTime * smoothSpeed
        );

        // TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        targetFill = currentHealth / maxHealth;
    }
}
