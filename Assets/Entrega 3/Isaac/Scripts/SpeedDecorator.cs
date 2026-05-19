using UnityEngine;
using System.Reflection;

public class SpeedDecorator : PowerUpDecorator
{
    [SerializeField] private float multiplier = 1.5f;

    private PlayerMovement playerMovement;

    private float originalSpeed;

    private FieldInfo moveSpeedField;

    public override void Apply()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();

        if(playerMovement == null)
        {
            Debug.LogError("No se encontró PlayerMovement");
            return;
        }

        moveSpeedField = typeof(PlayerMovement)
            .GetField("moveSpeed", BindingFlags.NonPublic | BindingFlags.Instance);

        if(moveSpeedField == null)
        {
            Debug.LogError("No se encontró moveSpeed");
            return;
        }

        originalSpeed = (float)moveSpeedField.GetValue(playerMovement);

        moveSpeedField.SetValue(
            playerMovement,
            originalSpeed * multiplier
        );

        Debug.Log("SPEED BOOST ACTIVADO");
    }

    public override void Remove()
    {
        if(playerMovement == null || moveSpeedField == null)
        {
            return;
        }

        moveSpeedField.SetValue(playerMovement, originalSpeed);

        Debug.Log("SPEED BOOST TERMINADO");
    }
}