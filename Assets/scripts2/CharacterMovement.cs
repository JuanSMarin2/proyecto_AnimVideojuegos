using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.scripts2
{
    public class CharacterMovement: MonoBehaviour, ICharacterComponent
    {
        [SerializeField] private FloatDampener speedX;
        [SerializeField] private FloatDampener speedY;
        [SerializeField] private Camera camera;

        private Quaternion targetRotation;

        private int _speedXHash;
        private int _speedYHash;
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _speedXHash = Animator.StringToHash("SpeedX");
            _speedYHash = Animator.StringToHash("SpeedY");
        }

        private void SolveCharacterRotation()
        {
            Vector3 floornormal = transform.up;
            Vector3 cameraRealForward = camera.transform.forward;
            float angleInterpolator = Mathf.Abs(Vector3.Dot(lhs: cameraRealForward, rhs: floornormal));
            Vector3 cameraForeward = Vector3.Lerp(cameraRealForward, camera.transform.up, angleInterpolator).normalized;
            Vector3 characterForeward = Vector3.ProjectOnPlane(vector: cameraForeward, floornormal).normalized;
            Debug.DrawLine(transform.position, transform.position + characterForeward * 5, Color.red);
            targetRotation = Quaternion.LookRotation(characterForeward, floornormal);
        }

        private void Update()
        {
            speedX.Update();
            speedY.Update();
            _animator.SetFloat(_speedXHash, speedX.currentValue);
            _animator.SetFloat(_speedYHash, speedY.currentValue);
            SolveCharacterRotation();
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
           Vector2 inputvalue = ctx.ReadValue<Vector2>();
           speedX.targetValue = inputvalue.x;
           speedY.targetValue = inputvalue.y;
        }

        public Character ParentCharacter { get; set; }
    }
}
