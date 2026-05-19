using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpsVisibility : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField]
    private PlayerMovement[] playerMovements;

    [Header("Estados")]
    public bool P1;
    public bool P2;
    public bool P3;

    [Header("Im�genes")]
    public Image imageP1;
    public Image imageP2;
    public Image imageP3;

    [Header("Timer")]
    public TMP_Text berserkText;
    public TMP_Text shieldText;
    public TMP_Text speedText;
    [SerializeField]
    private string timerFormat = "{0}/{1}";

    [Header("Configuraci�n")]
    protected float activeScaleMultiplier = 1.4f;

    [Range(0f, 1f)]
    protected float inactiveAlpha = 0.15f;

    [Range(0f, 1f)]
    protected float activeAlpha = 0.9f;

    [Header("Velocidad de interpolaci�n")]
    protected float smoothSpeed = 8f;

    private Vector3 p1OriginalScale;
    private Vector3 p2OriginalScale;
    private Vector3 p3OriginalScale;

    void Start()
    {
        p1OriginalScale = imageP1.rectTransform.localScale;
        p2OriginalScale = imageP2.rectTransform.localScale;
        p3OriginalScale = imageP3.rectTransform.localScale;

        if (playerMovements == null || playerMovements.Length == 0)
        {
            playerMovements = FindObjectsOfType<PlayerMovement>(true);
        }
    }

    void Update()
    {
        PlayerMovement activeMovement = GetActivePlayerMovement();

        BerserkDecorator berserk =
            activeMovement ? activeMovement.GetComponent<BerserkDecorator>() : null;
        ShieldDecorator shield =
            activeMovement ? activeMovement.GetComponent<ShieldDecorator>() : null;
        SpeedDecorator speed =
            activeMovement ? activeMovement.GetComponent<SpeedDecorator>() : null;

        P1 = berserk != null && berserk.IsActive;
        P2 = shield != null && shield.IsActive;
        P3 = speed != null && speed.IsActive;

        AnimateImage(imageP1, P1, p1OriginalScale);
        AnimateImage(imageP2, P2, p2OriginalScale);
        AnimateImage(imageP3, P3, p3OriginalScale);

        UpdateTimerText(berserkText, berserk);
        UpdateTimerText(shieldText, shield);
        UpdateTimerText(speedText, speed);
    }

    private PlayerMovement GetActivePlayerMovement()
    {
        if (playerMovements == null || playerMovements.Length == 0)
        {
            return null;
        }

        for (int i = 0; i < playerMovements.Length; i++)
        {
            PlayerMovement movement = playerMovements[i];
            if (movement && movement.gameObject.activeInHierarchy)
            {
                return movement;
            }
        }

        return null;
    }

    private void UpdateTimerText(
        TMP_Text targetText,
        PowerUpDecorator powerUp)
    {
        if (targetText == null)
        {
            return;
        }

        if (powerUp == null || !powerUp.IsActive)
        {
            targetText.text = string.Empty;
            return;
        }

        int remaining = Mathf.CeilToInt(powerUp.RemainingTime);
        int duration = Mathf.CeilToInt(powerUp.Duration);

        if (remaining <= 0)
        {
            targetText.text = string.Empty;
            return;
        }

        targetText.text = string.Format(timerFormat, remaining, duration);
    }

    void AnimateImage(Image img, bool state, Vector3 originalScale)
    {
        // Escala objetivo
        Vector3 targetScale = state
            ? originalScale * activeScaleMultiplier
            : originalScale;

        // Interpolaci�n de escala
        img.rectTransform.localScale = Vector3.Lerp(
            img.rectTransform.localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );

        // Alpha objetivo
        float targetAlpha = state
            ? activeAlpha
            : inactiveAlpha;

        // Interpolaci�n de alpha
        Color color = img.color;

        color.a = Mathf.Lerp(
            color.a,
            targetAlpha,
            Time.deltaTime * smoothSpeed
        );

        img.color = color;
    }
}
