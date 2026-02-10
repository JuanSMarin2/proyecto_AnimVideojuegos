using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.scripts2
{   

    public class CharacterLook: MonoBehaviour, ICharacterComponent
    {
        [SerializeField] private Transform target;
        [SerializeField] private FloatDampener horizontalDampener;
        [SerializeField] private FloatDampener verticalDampener;
        [SerializeField] private float horizontalrotationspeed;
        [SerializeField] private float verticalrotationspeed;
        [SerializeField] private Vector2 verticalrotationlimits;
        private float verticalrotation;
        public void OnLook(InputAction.CallbackContext ctx)
        {
            Vector2 inputvalue = ctx.ReadValue<Vector2>();
            inputvalue = inputvalue / new Vector2(Screen.width, Screen.height);

            horizontalDampener.targetValue += inputvalue.x;
            verticalDampener.targetValue += inputvalue.y;
        }

        private void ApplyLookRotation()
        {
            if(target == null)
            {
                throw new NullReferenceException("Target is null");
            }

            target.RotateAround(point: target.position, axis: transform.up, horizontalDampener.currentValue * horizontalrotationspeed * 360 * Time.deltaTime);
            verticalrotation += verticalDampener.currentValue * verticalrotationspeed * 360 * Time.deltaTime;
            verticalrotationspeed = Mathf.Clamp(verticalrotation, min:verticalrotationlimits.x, max:verticalrotationlimits.y);

            Vector3 euler = target.localEulerAngles;
            euler.x = verticalrotation;
            target.localEulerAngles = euler;
        }

        private void Update()
        {
           horizontalDampener.Update();
            verticalDampener.Update();
            ApplyLookRotation();
        }

        public Character ParentCharacter { get; set; }
    }
}
