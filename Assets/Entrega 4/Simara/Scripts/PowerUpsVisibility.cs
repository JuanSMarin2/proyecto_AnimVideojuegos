using UnityEngine;
using UnityEngine.UI;

public class PowerUpsVisibility : MonoBehaviour
{
    [Header("Estados")]
    public bool P1;
    public bool P2;
    public bool P3;

    [Header("Imágenes")]
    public Image imageP1;
    public Image imageP2;
    public Image imageP3;

    [Header("Configuración")]
    protected float activeScaleMultiplier = 1.4f;

    [Range(0f, 1f)]
    protected float inactiveAlpha = 0.15f;

    [Range(0f, 1f)]
    protected float activeAlpha = 0.9f;

    [Header("Velocidad de interpolación")]
    protected float smoothSpeed = 8f;

    private Vector3 p1OriginalScale;
    private Vector3 p2OriginalScale;
    private Vector3 p3OriginalScale;

    void Start()
    {
        p1OriginalScale = imageP1.rectTransform.localScale;
        p2OriginalScale = imageP2.rectTransform.localScale;
        p3OriginalScale = imageP3.rectTransform.localScale;
    }

    void Update()
    {
        AnimateImage(imageP1, P1, p1OriginalScale);
        AnimateImage(imageP2, P2, p2OriginalScale);
        AnimateImage(imageP3, P3, p3OriginalScale);

        // TEST
        if (Input.GetKeyDown(KeyCode.Alpha1))
            P1 = !P1;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            P2 = !P2;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            P3 = !P3;
    }

    void AnimateImage(Image img, bool state, Vector3 originalScale)
    {
        // Escala objetivo
        Vector3 targetScale = state
            ? originalScale * activeScaleMultiplier
            : originalScale;

        // Interpolación de escala
        img.rectTransform.localScale = Vector3.Lerp(
            img.rectTransform.localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );

        // Alpha objetivo
        float targetAlpha = state
            ? activeAlpha
            : inactiveAlpha;

        // Interpolación de alpha
        Color color = img.color;

        color.a = Mathf.Lerp(
            color.a,
            targetAlpha,
            Time.deltaTime * smoothSpeed
        );

        img.color = color;
    }
}
