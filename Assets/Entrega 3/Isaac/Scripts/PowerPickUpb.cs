using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private PowerUpDecorator powerUpPrefab;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo entró al trigger: " + other.name);

        if(other.CompareTag("Player"))
        {
            Debug.Log("El jugador tocó el power up");

            PlayerMovement targetMovement = FindActivePlayerMovement(other);

            if (targetMovement == null)
            {
                Debug.LogError("No se encontró un PlayerMovement activo en el jugador");
                return;
            }

            PowerUpDecorator powerUp =
                targetMovement.gameObject.AddComponent(
                    powerUpPrefab.GetType()
                ) as PowerUpDecorator;

            Debug.Log("Power up agregado: " + powerUp.GetType().Name);

            StartCoroutine(powerUp.PowerUpRoutine());

            Destroy(gameObject);
        }
    }

    private PlayerMovement FindActivePlayerMovement(Collider other)
    {
        PlayerMovement[] movements =
            other.GetComponentsInChildren<PlayerMovement>(true);

        foreach (PlayerMovement movement in movements)
        {
            if (movement.gameObject.activeInHierarchy)
            {
                return movement;
            }
        }

        return null;
    }
}