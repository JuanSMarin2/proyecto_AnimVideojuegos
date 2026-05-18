using System.Collections;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    [Header("Disable Root")]
    [SerializeField] private GameObject rootToDisable;
    [SerializeField] private float disableDelay = 5f;

    [Header("Drops")]
    [SerializeField] private Transform dropSpawnPoint;
    [SerializeField] private GameObject[] dropPrefabs;
    [SerializeField] private GameObject healerPrefab;
    [SerializeField, Range(0f, 1f)] private float dropChance = 0.5f;
    [SerializeField, Range(0f, 1f)] private float healerChance = 0.7f;

    private bool defeated;


    public static System.Action OnEnemyDeath; // esteban


    private void Awake()
    {
        if (!rootToDisable)
        {
            rootToDisable = transform.root.gameObject;
        }

        if (!dropSpawnPoint)
        {
            dropSpawnPoint = transform;
        }
    }

    public void EnemyDefeated()
    {
        if (defeated)
        {
            return;
        }

        defeated = true;

        OnEnemyDeath?.Invoke(); // esteban

        TrySpawnDrop();
        StartCoroutine(DisableRootAfterDelay());
    }

    private void TrySpawnDrop()
    {
        if (dropPrefabs == null)
        {
            return;
        }

        if (Random.value > dropChance)
        {
            return;
        }

        GameObject prefabToSpawn = null;
        if (healerPrefab && Random.value <= healerChance)
        {
            prefabToSpawn = healerPrefab;
        }
        else if (dropPrefabs.Length > 0)
        {
            prefabToSpawn = dropPrefabs[Random.Range(0, dropPrefabs.Length)];
        }

        if (!prefabToSpawn)
        {
            return;
        }

        Instantiate(prefabToSpawn, dropSpawnPoint.position, dropSpawnPoint.rotation);
    }

    private IEnumerator DisableRootAfterDelay()
    {
        if (disableDelay > 0f)
        {
            yield return new WaitForSeconds(disableDelay);
        }

        if (rootToDisable)
        {
            rootToDisable.SetActive(false);
        }
    }
}
