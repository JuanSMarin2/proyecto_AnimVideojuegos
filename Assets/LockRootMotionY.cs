using UnityEngine;

public class LockRootMotionY : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool applyHorizontalRootMotion = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        if (animator == null) return;

        Vector3 delta = animator.deltaPosition;

        delta.y = 0f;

        if (applyHorizontalRootMotion)
        {
            transform.position += delta;
        }

        transform.rotation *= animator.deltaRotation;
    }
}