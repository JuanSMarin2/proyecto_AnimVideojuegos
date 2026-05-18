using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class EnemyGroup
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnRate; // enemigos por segundo
}

[System.Serializable]
public class Wave
{
    public List<EnemyGroup> enemyGroups;
}

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves;
    public Transform[] spawnPoints;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;

    void OnEnable()
    {
        EnemyDeathHandler.OnEnemyDeath += OnEnemyKilled;
    }

    void OnDisable()
    {
        EnemyDeathHandler.OnEnemyDeath -= OnEnemyKilled;
    }

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (currentWaveIndex < waves.Count)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            // Esperar a que todos mueran
            yield return new WaitUntil(() => enemiesAlive <= 0);

            Debug.Log("Oleada " + currentWaveIndex + " completada");

            yield return new WaitForSeconds(2f);

            currentWaveIndex++;
        }

        // Cuando terminan todas las oleadas
        Debug.Log("Todas las oleadas completadas");

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator SpawnWave(Wave wave)
    {
        //  Contar todos los enemigos de TODOS los grupos
        enemiesAlive = 0;

        foreach (EnemyGroup group in wave.enemyGroups)
        {
            enemiesAlive += group.count;
        }

        //  Spawnear cada grupo
        foreach (EnemyGroup group in wave.enemyGroups)
        {
            for (int i = 0; i < group.count; i++)
            {
                SpawnEnemy(group.enemyPrefab);
                yield return new WaitForSeconds(1f / group.spawnRate);
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No hay puntos de spawn asignados");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    void OnEnemyKilled()
    {
        if (enemiesAlive <= 0)
            return;

        enemiesAlive--;

        Debug.Log("Enemigos restantes: " + enemiesAlive);
    }
}
