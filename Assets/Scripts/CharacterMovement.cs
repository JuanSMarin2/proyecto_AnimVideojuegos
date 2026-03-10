using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterMovement : MonoBehaviour, ICharacterComponent
{

    private int _speedXHash;
    private int _speedYHash;

    [SerializeField] private FloatDampener speedX;
    [SerializeField] private FloatDampener speedY;

    [SerializeField ]private float angularSpeed;   

    [SerializeReference] private Camera camera;

    private Quaternion targetRotation;

    private Animator _animator;

    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _speedXHash = Animator.StringToHash("SpeedX");
        _speedYHash = Animator.StringToHash("SpeedY");
    }
    private void MoveCharacter()
    {
        Vector3 moveDirection = new Vector3(speedX.CurrentValue, 0, speedY.CurrentValue);
        moveDirection = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * moveDirection;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void SolveCharacterRotation()
    {
        Vector3 floorNormal = transform.up;
        Vector3 cameraRealFoward = camera.transform.forward;

        float angleInterpolator = Mathf.Abs(Vector3.Dot(cameraRealFoward, floorNormal));
        Vector3 cameraFoward = Vector3.Lerp(cameraRealFoward, camera.transform.up, angleInterpolator).normalized;

        Vector3 characterForward = Vector3.ProjectOnPlane(cameraFoward, floorNormal).normalized;

        Debug.DrawLine(transform.position, transform.position + characterForward * 3, Color.green, 5);
        targetRotation = Quaternion.LookRotation(characterForward, floorNormal);


    }
    


    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        speedX.TargetValue = inputValue.x;
        speedY.TargetValue = inputValue.y;

   
    }

    private void Update()
    {
        speedX.Update();
        speedY.Update();


        moveSpeed = ParentCharacter.IsCrouching ? 2.5f : 5f;
        float animMultiplier = ParentCharacter.IsCrouching ? .5f : 1f;

        MoveCharacter();

        _animator.SetFloat(_speedXHash, speedX.CurrentValue * animMultiplier);
        _animator.SetFloat(_speedYHash, speedY.CurrentValue * animMultiplier);



        SolveCharacterRotation();

        if(!ParentCharacter.IsAiming)
        {
           ApplyCharacterRotation();
        }

    }

    private void ApplyCharacterRotation()
    {
        float motionMagnitud = Mathf.Sqrt(speedX.TargetValue * speedX.TargetValue + speedY.TargetValue * speedY.TargetValue);
        float rotationSpeed = Mathf.SmoothStep(0, .01f, motionMagnitud);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed  * rotationSpeed);
    }

    public Character ParentCharacter { get; set; }
}
