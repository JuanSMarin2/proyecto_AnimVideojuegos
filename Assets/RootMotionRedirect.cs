using UnityEngine;

public class RootMotionRedirect : MonoBehaviour
{
 [SerializeField] private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        if (rb == null) return;

        Vector3 delta = animator.deltaPosition;

        // Convertir a velocidad
        Vector3 velocity = delta / Time.deltaTime;

        // Aplicar solo en XZ
        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y, // mantener gravedad
            velocity.z
        );
    }
}