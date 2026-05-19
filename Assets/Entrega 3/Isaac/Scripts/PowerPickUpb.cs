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

            PowerUpDecorator existing =
                targetMovement.GetComponent(powerUpPrefab.GetType()) as PowerUpDecorator;

            if (existing != null)
            {
                existing.Refresh();
                Destroy(gameObject);
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
        Transform searchRoot = FindPlayerRoot(other.transform);

        if (searchRoot == null)
        {
            searchRoot = other.transform.root;
        }

        PlayerMovement[] movements =
            searchRoot.GetComponentsInChildren<PlayerMovement>(true);

        foreach (PlayerMovement movement in movements)
        {
            if (movement.gameObject.activeInHierarchy)
            {
                return movement;
            }
        }

        PlayerMovement[] allMovements =
            FindObjectsOfType<PlayerMovement>(true);

        PlayerMovement closest = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < allMovements.Length; i++)
        {
            PlayerMovement movement = allMovements[i];
            if (movement == null || !movement.gameObject.activeInHierarchy)
            {
                continue;
            }

            float distance = Vector3.Distance(
                other.transform.position,
                movement.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = movement;
            }
        }

        if (closest != null)
        {
            return closest;
        }

        return null;
    }

    private Transform FindPlayerRoot(Transform source)
    {
        Transform current = source;
        while (current != null)
        {
            if (current.CompareTag("Player"))
            {
                return current;
            }

            current = current.parent;
        }

        return null;
    }
}