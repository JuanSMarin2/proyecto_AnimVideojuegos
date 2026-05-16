using UnityEngine;
using System.Reflection;

public class SpeedDecorator : PowerUpDecorator
{
    [SerializeField] private float multiplier = 1.5f;

    private PlayerMovement playerMovement;

    private float originalSpeed;

    private FieldInfo moveSpeedField;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        moveSpeedField = typeof(PlayerMovement)
            .GetField("moveSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public override void Apply()
    {
        if (moveSpeedField == null) return;

        originalSpeed = (float)moveSpeedField.GetValue(playerMovement);

        moveSpeedField.SetValue(
            playerMovement,
            originalSpeed * multiplier
        );
    }

    public override void Remove()
    {
        if (moveSpeedField == null) return;

        moveSpeedField.SetValue(
            playerMovement,
            originalSpeed
        );
    }
}