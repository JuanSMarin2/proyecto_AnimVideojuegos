using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.scripts
{
    public class CharacterMovement : MonoBehaviour ,ICharacterComponent
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
            _speedXHash = Animator.StringToHash(name:"SpeedX");
            _speedYHash = Animator.StringToHash(name: "SpeedY");
        }

        private void SolveCharacterRotation()
        {
            Vector3 floorNormal = transform.up;
            Vector3 cameraRealForward = camera.transform.forward;
            float angleInterpolator = Mathf.Abs(f:Vector3.Dot(lhs: cameraRealForward, rhs: floorNormal));
            Vector3 cameraForward = Vector3.Lerp(cameraRealForward, camera.transform.up, angleInterpolator).normalized;
            Vector3 characterForward = Vector3.ProjectOnPlane(vector: cameraForward, floorNormal).normalized;
            Debug.DrawLine(transform.position, transform.position + characterForward*3, Color.green, duration:5);
            targetRotation = Quaternion.LookRotation(characterForward, upwards: floorNormal);
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            Vector2 InputValue = ctx.ReadValue<Vector2>();
            speedX.TargetValue = InputValue.x;
            speedY.TargetValue = InputValue.y;
        }

        private void Update()
        {
            speedX.Update();
            speedY.Update();
            _animator.SetFloat(_speedXHash, speedX.CurrentValue);
            _animator.SetFloat(_speedYHash, speedY.CurrentValue);
        }

        public Character ParentCharacter { get; set; }
    }
}
