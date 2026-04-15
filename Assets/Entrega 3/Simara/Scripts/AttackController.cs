using System;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemPlayerInput = UnityEngine.InputSystem.PlayerInput;

public class AttackController : MonoBehaviour
{
    public enum AttackDirection
    {
        Neutral = 0,
        Forward = 1,
        Backward = 2,
        Left = 3,
        Right = 4
    }

    private Animator animator;
    private AttackHitBoxController hitBoxController;
    private InputSystemPlayerInput inputSystemPlayerInput;
    private InputAction moveAction;

    [SerializeField] private float ligthCost = 15f;
    [SerializeField] private float heavyCost = 35f;

    [SerializeField] private bool isLight;

    [SerializeField] private CameraShakeController cameraShake;

    [Header("Attack Movement Lock")]
    [SerializeField] private bool lockMovementDuringAttack = true;
    [SerializeField] private int locomotionLayerIndex = 0;
    [SerializeField] private string locomotionStateName = "Locomotion";

    [Header("Directional Buffer")]
    [SerializeField] private string moveActionName = "Move";
    [SerializeField] private float directionDeadzone = 0.25f;
    [SerializeField] private float directionBufferWindow = 0.25f;
    [SerializeField] private Transform cameraTransform;

    private bool isInAttackMode;
    private bool hasExitedLocomotionSinceAttack;
    private Vector2 currentMoveInput;

    private AttackDirection bufferedDirection = AttackDirection.Neutral;
    private float bufferedDirectionTime = float.NegativeInfinity;
    private bool hasBufferedDirection;

    private AttackDirection comboWindowDirection = AttackDirection.Neutral;
    private bool hasComboWindowDirection;
    private int locomotionStateHash;

    public bool IsMovementLocked => lockMovementDuringAttack && isInAttackMode;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitBoxController = GetComponent<AttackHitBoxController>();
        inputSystemPlayerInput = GetComponent<InputSystemPlayerInput>();
        locomotionStateHash = Animator.StringToHash(locomotionStateName);

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        ResolveMoveAction();
    }

    private void OnEnable()
    {
        SubscribeMoveInput();
    }

    private void OnDisable()
    {
        UnsubscribeMoveInput();
    }

    private void Update()
    {
        if (!isInAttackMode)
        {
            return;
        }

        if (!hasExitedLocomotionSinceAttack)
        {
            if (!IsInLocomotionState())
            {
                hasExitedLocomotionSinceAttack = true;
            }

            return;
        }

        if (IsInLocomotionState())
        {
            EndAttackMode();
        }
    }
    
    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Game.Instance.PlayerOne.CurrentStamina > 0)
            {
                Game.Instance.PlayerOne.DepleteStamina(ligthCost);
                BeginAttackMode();
                RotateCharacterToBufferedDirection();
                animator.SetTrigger("Attack");

                OnLightAttackHit(); //Esto para prepararlo para que sea solo en el impacto
            }
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Game.Instance.PlayerOne.CurrentStamina > 0)
            {
                Game.Instance.PlayerOne.DepleteStamina(heavyCost);
                BeginAttackMode();
                RotateCharacterToBufferedDirection();
                animator.SetTrigger("HeavyAttack");

                OnHeavyAttackHit(); //Esto para prepararlo para que sea solo en el impacto
            }
        }
    }

    public void OnLightAttackHit()
    {
        cameraShake.Shake(1f, isLight = true);
    }

    public void OnHeavyAttackHit()
    {
        cameraShake.Shake(1.3f, isLight = false);
    }

    public void BeginAttackMode()
    {
        isInAttackMode = true;
        hasExitedLocomotionSinceAttack = false;
    }

    public void EndAttackMode()
    {
        isInAttackMode = false;
        hasExitedLocomotionSinceAttack = false;
    }

    private bool IsInLocomotionState()
    {
        if (animator == null)
        {
            return false;
        }

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(locomotionLayerIndex);
        return currentState.shortNameHash == locomotionStateHash;
    }

    public void OnComboWindowTriggered()
    {
        comboWindowDirection = SelectDirectionForNextStep();
        hasComboWindowDirection = true;
    }

    public AttackDirection ConsumeNextAttackDirection()
    {
        if (hasComboWindowDirection)
        {
            hasComboWindowDirection = false;
            return comboWindowDirection;
        }

        return SelectDirectionForNextStep();
    }

    public AttackDirection GetDirectionalAttack(Vector2 inputDirection)
    {
        if (inputDirection.magnitude <= Mathf.Max(0f, directionDeadzone))
        {
            return AttackDirection.Neutral;
        }

        Vector2 normalized = inputDirection.normalized;
        if (Mathf.Abs(normalized.y) >= Mathf.Abs(normalized.x))
        {
            return normalized.y >= 0f ? AttackDirection.Forward : AttackDirection.Backward;
        }

        return normalized.x >= 0f ? AttackDirection.Right : AttackDirection.Left;
    }

    private AttackDirection SelectDirectionForNextStep()
    {
        if (hasBufferedDirection)
        {
            return bufferedDirection;
        }

        bool isWithinBufferWindow = Time.time - bufferedDirectionTime <= Mathf.Max(0f, directionBufferWindow);
        if (isWithinBufferWindow)
        {
            return bufferedDirection;
        }

        return GetDirectionalAttack(currentMoveInput);
    }

    private void ResolveMoveAction()
    {
        if (inputSystemPlayerInput == null || inputSystemPlayerInput.actions == null)
        {
            return;
        }

        moveAction = inputSystemPlayerInput.actions.FindAction(moveActionName, false);
        if (moveAction == null)
        {
            Debug.LogWarning($"Move action '{moveActionName}' was not found. Directional input buffer will stay neutral.", this);
            return;
        }

        if (!moveAction.enabled)
        {
            moveAction.Enable();
        }
    }

    private void SubscribeMoveInput()
    {
        if (moveAction == null)
        {
            return;
        }

        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
    }

    private void UnsubscribeMoveInput()
    {
        if (moveAction == null)
        {
            return;
        }

        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        currentMoveInput = context.ReadValue<Vector2>();
        BufferDirection(currentMoveInput);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        currentMoveInput = Vector2.zero;
        BufferDirection(currentMoveInput);
    }

    private void BufferDirection(Vector2 inputDirection)
    {
        AttackDirection direction = GetDirectionalAttack(inputDirection);
        if (direction == AttackDirection.Neutral)
        {
            return;
        }

        bufferedDirection = direction;
        bufferedDirectionTime = Time.time;
        hasBufferedDirection = true;
    }

    private void RotateCharacterToBufferedDirection()
    {
        AttackDirection nextDirection = ConsumeNextAttackDirection();
        Vector3 worldDirection = DirectionToWorld(nextDirection);
        if (worldDirection.sqrMagnitude < 0.0001f)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(worldDirection, Vector3.up);
    }

    private Vector3 DirectionToWorld(AttackDirection direction)
    {
        if (direction == AttackDirection.Neutral)
        {
            return Vector3.zero;
        }

        Vector3 forward;
        Vector3 right;

        if (cameraTransform != null)
        {
            forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        }
        else
        {
            forward = Vector3.forward;
            right = Vector3.right;
        }

        switch (direction)
        {
            case AttackDirection.Forward: return forward;
            case AttackDirection.Backward: return -forward;
            case AttackDirection.Left: return -right;
            case AttackDirection.Right: return right;
            default: return Vector3.zero;
        }
    }
}