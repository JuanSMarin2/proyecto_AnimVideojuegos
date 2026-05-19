using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ParticleSystem efectoExplosion;

    [Header("Configuración")]
    [SerializeField] private string layerObjetivo = "Player";

    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        // Esto saldrá en consola CUALQUIER cosa que toque el Power Up
        Debug.Log("Algo tocó el trigger: " + other.gameObject.name + " en la capa: " + LayerMask.LayerToName(other.gameObject.layer));

        if (yaSeActivo) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(layerObjetivo))
        {
            Debug.Log("¡El jugador tocó el Power Up! Activando partículas...");
            yaSeActivo = true;
            ActivarPowerUp();
        }
    }

    private void ActivarPowerUp()
{
    // Si olvidaste arrastrarlo, el script lo busca solo en sus objetos hijos
    if (efectoExplosion == null)
    {
        efectoExplosion = GetComponentInChildren<ParticleSystem>();
    }

    if (efectoExplosion != null)
    {
        efectoExplosion.Play();
        Debug.Log("¡Partículas encontradas automáticamente y activadas!");
    }
    else
    {
        Debug.LogError("ERROR: No se encontró ningún Particle System ni en el Inspector ni en los objetos hijos de: " + gameObject.name);
    }

    if (TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
    {
        mesh.enabled = false;
    }
}
}

