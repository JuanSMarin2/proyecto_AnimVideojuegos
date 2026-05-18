using UnityEngine;

public class PreventPlayerFall : MonoBehaviour
{
    [SerializeField] private Transform feetTarget;
    private const float RAYCAST_DISTANCE = 100f;

    private void Update()
    {
        if (feetTarget == null) return;

        // Raycast hacia abajo desde feetTarget
        if (Physics.Raycast(feetTarget.position, Vector3.down, out RaycastHit hitDown, RAYCAST_DISTANCE))
        {
            // Verificar si la superficie tiene tag "Floor"
            if (hitDown.collider.CompareTag("Floor"))
            {
                // Ya hay una superficie debajo, no hacer nada
                return;
            }
        }

        // Si no hay superficie debajo, hacer raycast hacia arriba
        if (Physics.Raycast(feetTarget.position, Vector3.up, out RaycastHit hitUp, RAYCAST_DISTANCE))
        {
            // Verificar si la superficie tiene tag "Floor"
            if (hitUp.collider.CompareTag("Floor"))
            {
                // Ajustar la posición del personaje para que feetTarget esté en contacto con la superficie
                float distanceToSurface = hitUp.distance;
                Vector3 adjustment = new Vector3(0, distanceToSurface, 0);
                transform.position += adjustment;
            }
        }
    }
}
