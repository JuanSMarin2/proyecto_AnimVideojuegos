using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.scripts
{
    public class CharacterLook : MonoBehaviour,ICharacterComponent
    {
        [SerializeField] private Transform target;

        [SerializeField] private FloatDampener horizontalDampener;
        [SerializeField] private FloatDampener verticalDampener;

        [SerializeField] private float horizontalRotationSpeed;
        [SerializeField] private float verticalRotationSpeed;
        [SerializeField] private Vector2 verticalRotationLimits;

        private float verticalRotation;
        public void Onlock(InputAction.CallbackContext ctx)
        {
            Vector2 InputValue = ctx.ReadValue<Vector2>();
            InputValue = InputValue / new Vector2(Screen.width, Screen.height);
            horizontalDampener.TargetValue = InputValue.x;
            verticalDampener.TargetValue = InputValue.y;
        }

        private void ApplyLookRotation()
        {
            if (target == null)
            {
                throw new NullReferenceException("Look target is null");
            }

            target.RotateAround(point:target.position, axis:transform.up, angle:horizontalDampener.CurrentValue * horizontalRotationSpeed * 360 * Time.deltaTime);
            verticalRotation += verticalDampener.CurrentValue * verticalRotationSpeed * 360 * Time.deltaTime;
            verticalRotation = Mathf.Clamp(verticalRotation, min:verticalRotationLimits.x, max:verticalRotationLimits.y);

            Vector3 euler = target.localEulerAngles;
            euler.x = verticalRotation;
            target.localEulerAngles = euler;
        }

        private void Update()
        {
            horizontalDampener.Update();
            verticalDampener.Update();
            ApplyLookRotation();
        }

        [field:SerializeField] public Character ParentCharacter { get; set; }
    }
}
