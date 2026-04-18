using UnityEngine;

public class LockRootMotionY : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        Vector3 delta = animator.deltaPosition;

   
        delta.y = 0f;

   
        transform.position += delta;

  
        transform.rotation *= animator.deltaRotation;
    }
}