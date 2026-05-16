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

            PowerUpDecorator powerUp =
                other.gameObject.AddComponent(
                    powerUpPrefab.GetType()
                ) as PowerUpDecorator;

            Debug.Log("Power up agregado: " + powerUp.GetType().Name);

            StartCoroutine(powerUp.PowerUpRoutine());

            Destroy(gameObject);
        }
    }
}