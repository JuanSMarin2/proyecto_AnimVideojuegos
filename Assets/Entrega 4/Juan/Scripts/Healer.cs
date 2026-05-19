using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField, Range(0f, 1f)] private float healPercent = 0.5f;
    [SerializeField] private bool destroyOnUse = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        HealthController health = other.GetComponent<HealthController>();
        if (!health)
        {
            health = other.GetComponentInParent<HealthController>();
        }

        if (!health)
        {
            return;
        }

        health.HealPercent(healPercent);

        if (destroyOnUse)
        {
            Destroy(gameObject);
        }
    }
}
