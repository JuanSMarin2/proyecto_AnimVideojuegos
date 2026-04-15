using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemPlayerInput = UnityEngine.InputSystem.PlayerInput;

[RequireComponent(typeof(InputSystemPlayerInput))]
public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 followOffset = new Vector3(0f, 1.6f, -4f);
    [SerializeField] private Vector3 lookAtOffset = new Vector3(0f, 1.2f, 0f);

    [Header("Input")]
    [Tooltip("Input action name that provides a Vector2 look delta, for example Look or CameraMove.")]
    [SerializeField] private string cameraMoveActionName = "Look";
    [SerializeField] private bool lockAndHideCursor = true;

    [Header("Sensitivity")]
    [Min(0f)] [SerializeField] private float mouseSensitivity = 0.08f;
    [Min(0f)] [SerializeField] private float controllerSensitivity = 180f;
    [SerializeField] private bool invertY = false;

    [Header("Pitch Clamp")]
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    [Header("Smoothing")]
    [SerializeField] private bool smoothRotation = true;
    [Min(0f)] [SerializeField] private float rotationSmoothTime = 0.05f;
    [Min(0f)] [SerializeField] private float followSmoothTime = 0.08f;

    private InputSystemPlayerInput playerInput;
    private InputAction cameraMoveAction;

    private Vector2 lookInput;
    private bool lastInputWasMouse = true;

    private float targetYaw;
    private float targetPitch;
    private float currentYaw;
    private float currentPitch;

    private float yawVelocity;
    private float pitchVelocity;
    private Vector3 followVelocity;

    private void Awake()
    {
        playerInput = GetComponent<InputSystemPlayerInput>();
        ResolveCameraMoveAction();
        InitializeAnglesFromTransform();
    }

    private void OnEnable()
    {
        SubscribeInput();
        ApplyCursorState();
    }

    private void OnDisable()
    {
        UnsubscribeInput();
        SetCursorLocked(false);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!lockAndHideCursor)
        {
            return;
        }

        SetCursorLocked(hasFocus);
    }

    private void LateUpdate()
    {
        if (target == null || cameraMoveAction == null)
        {
            return;
        }

        UpdateRotationTargets();
        UpdateCurrentRotation();
        UpdateCameraTransform();
    }

    private void ResolveCameraMoveAction()
    {
        if (playerInput == null || playerInput.actions == null)
        {
            Debug.LogError("ThirdPersonCameraController requires a PlayerInput with an assigned Input Actions asset.", this);
            return;
        }

        cameraMoveAction = playerInput.actions.FindAction(cameraMoveActionName, false);
        if (cameraMoveAction == null)
        {
            Debug.LogError($"Action '{cameraMoveActionName}' was not found in PlayerInput actions.", this);
            return;
        }

        if (!cameraMoveAction.enabled)
        {
            cameraMoveAction.Enable();
        }
    }

    private void SubscribeInput()
    {
        if (cameraMoveAction == null)
        {
            return;
        }

        cameraMoveAction.performed += OnCameraMovePerformed;
        cameraMoveAction.canceled += OnCameraMoveCanceled;
    }

    private void UnsubscribeInput()
    {
        if (cameraMoveAction == null)
        {
            return;
        }

        cameraMoveAction.performed -= OnCameraMovePerformed;
        cameraMoveAction.canceled -= OnCameraMoveCanceled;
    }

    private void OnCameraMovePerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        lastInputWasMouse = context.control != null && context.control.device is Mouse;
    }

    private void OnCameraMoveCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    private void UpdateRotationTargets()
    {
        Vector2 lookDelta = GetScaledLookDelta(lookInput);

        targetYaw += lookDelta.x;

        float verticalDelta = invertY ? lookDelta.y : -lookDelta.y;
        targetPitch = Mathf.Clamp(targetPitch + verticalDelta, minPitch, maxPitch);
    }

    private Vector2 GetScaledLookDelta(Vector2 rawInput)
    {
        if (lastInputWasMouse)
        {
            return rawInput * mouseSensitivity;
        }

        return rawInput * controllerSensitivity * Time.unscaledDeltaTime;
    }

    private void UpdateCurrentRotation()
    {
        if (smoothRotation)
        {
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, rotationSmoothTime);
            currentPitch = Mathf.SmoothDampAngle(currentPitch, targetPitch, ref pitchVelocity, rotationSmoothTime);
            return;
        }

        currentYaw = targetYaw;
        currentPitch = targetPitch;
    }

    private void UpdateCameraTransform()
    {
        Quaternion orbitRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 desiredPosition = target.position + orbitRotation * followOffset;

        if (followSmoothTime > 0f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref followVelocity, followSmoothTime);
        }
        else
        {
            transform.position = desiredPosition;
        }

        Vector3 lookPoint = target.position + lookAtOffset;
        transform.rotation = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
    }

    private void InitializeAnglesFromTransform()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        currentYaw = targetYaw = euler.y;
        currentPitch = targetPitch = NormalizePitchAngle(euler.x);
    }

    private static float NormalizePitchAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }

    private void ApplyCursorState()
    {
        if (!lockAndHideCursor)
        {
            return;
        }

        SetCursorLocked(true);
    }

    private static void SetCursorLocked(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }
}
