using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float angularSpeed = 720f;
    [SerializeField] private Camera playerCamera;

    private Rigidbody rb;
    private Animator animator;

    private Vector2 moveInput;
    private Quaternion targetRotation;

    private int speedXHash;
    private int speedYHash;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

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
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // relativo a cámara
        moveDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * moveDirection;

        rb.linearVelocity = new Vector3(
            moveDirection.x * moveSpeed,
            rb.linearVelocity.y,
            moveDirection.z * moveSpeed
        );
    }

    private void SolveRotation()
    {
        if (moveInput.magnitude < 0.1f) return; // 🔥 evita giro loco

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * moveDirection;

        targetRotation = Quaternion.LookRotation(moveDirection);
    }

    private void ApplyRotation()
    {
        if (moveInput.magnitude < 0.1f) return;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            angularSpeed * Time.deltaTime
        );
    }

    private void Animate()
    {
        animator.SetFloat(speedXHash, moveInput.x);
        animator.SetFloat(speedYHash, moveInput.y);
    }
}