using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float angularSpeed = 720f;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private AttackController attackController;
    private Animator animator;

    private Vector2 moveInput;
    private Quaternion targetRotation;

    private int speedXHash;
    private int speedYHash;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        animator = GetComponent<Animator>();

        if (attackController == null)
        {
            attackController = GetComponent<AttackController>();
        }

        speedXHash = Animator.StringToHash("SpeedX");
        speedYHash = Animator.StringToHash("SpeedY");
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }
        else if (ctx.canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    void Update()
    {
        Animate();
        SolveRotation();
        ApplyRotation();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (IsMovementLocked())
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        Vector3 moveDirection = GetCameraRelativeMoveDirection();

        rb.linearVelocity = new Vector3(
            moveDirection.x * moveSpeed,
            rb.linearVelocity.y,
            moveDirection.z * moveSpeed
        );
    }

    private void SolveRotation()
    {
        if (IsMovementLocked()) return;
        if (moveInput.magnitude < 0.1f) return;

        Vector3 moveDirection = GetCameraRelativeMoveDirection();
        if (moveDirection.sqrMagnitude < 0.0001f) return;

        targetRotation = Quaternion.LookRotation(moveDirection);
    }

    private void ApplyRotation()
    {
        if (IsMovementLocked()) return;
        if (moveInput.magnitude < 0.1f) return;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            angularSpeed * Time.deltaTime
        );
    }

    private void Animate()
    {
        if (IsMovementLocked())
        {
            animator.SetFloat(speedXHash, 0f);
            animator.SetFloat(speedYHash, 0f);
            return;
        }

        animator.SetFloat(speedXHash, moveInput.x);
        animator.SetFloat(speedYHash, moveInput.y);
    }

    private bool IsMovementLocked()
    {
        return attackController != null && attackController.IsMovementLocked;
    }

    private Vector3 GetCameraRelativeMoveDirection()
    {
        if (cameraTransform == null)
        {
            return new Vector3(moveInput.x, 0f, moveInput.y);
        }

        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        if (cameraForward.sqrMagnitude < 0.0001f)
        {
            cameraForward = Vector3.forward;
        }

        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        if (cameraRight.sqrMagnitude < 0.0001f)
        {
            cameraRight = Vector3.right;
        }

        return (cameraRight * moveInput.x + cameraForward * moveInput.y).normalized;
    }
}