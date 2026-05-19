using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

[System.Serializable]
public class EndlessEnemyGroup
{
    public GameObject enemyPrefab;
    [Range(0f, 1f)]
    public float spawnProb = 0.5f;
}

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves;
    public List<EndlessEnemyGroup> enemyGroupsEndless;
    public Transform[] spawnPoints;
    public TMP_Text waveTMP;

    [Header("Endless")]
    [SerializeField]
    private int endlessMinEnemies = 5;
    [SerializeField]
    private int endlessMaxEnemies = 10;
    [SerializeField]
    private float endlessSpawnRate = 2f;
    [SerializeField]
    private float endlessHealthBonusPerWave = 10f;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;
    private int endlessWaveIndex = 0;

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
            UpdateWaveText(currentWaveIndex + 1);
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            // Esperar a que todos mueran
            yield return new WaitUntil(() => enemiesAlive <= 0);

            Debug.Log("Oleada " + currentWaveIndex + " completada");

            yield return new WaitForSeconds(2f);

            currentWaveIndex++;
        }

        if (enemyGroupsEndless == null || enemyGroupsEndless.Count == 0)
        {
            Debug.Log("Todas las oleadas completadas");

            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            yield break;
        }

        while (true)
        {
            endlessWaveIndex++;
            UpdateWaveText(waves.Count + endlessWaveIndex);
            yield return StartCoroutine(SpawnEndlessWave());

            yield return new WaitUntil(() => enemiesAlive <= 0);

            Debug.Log("Oleada endless " + endlessWaveIndex + " completada");

            yield return new WaitForSeconds(2f);
        }
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

    IEnumerator SpawnEndlessWave()
    {
        enemiesAlive = 0;

        int totalToSpawn = Random.Range(
            endlessMinEnemies,
            endlessMaxEnemies + 1
        );

        for (int i = 0; i < totalToSpawn; i++)
        {
            GameObject prefab = GetRandomEndlessPrefab();
            if (prefab != null)
            {
                SpawnEnemy(prefab, true, endlessHealthBonusPerWave * endlessWaveIndex);
                enemiesAlive++;
            }

            if (endlessSpawnRate > 0f)
            {
                yield return new WaitForSeconds(1f / endlessSpawnRate);
            }
            else
            {
                yield return null;
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        SpawnEnemy(enemyPrefab, false, 0f);
    }

    void SpawnEnemy(
        GameObject enemyPrefab,
        bool applyHealthBonus,
        float healthBonus)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No hay puntos de spawn asignados");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        if (applyHealthBonus)
        {
            HealthController health = enemy.GetComponentInChildren<HealthController>();
            if (health != null)
            {
                health.AddMaxHealth(healthBonus, true);
            }
        }
    }

    GameObject GetRandomEndlessPrefab()
    {
        if (enemyGroupsEndless == null || enemyGroupsEndless.Count == 0)
        {
            return null;
        }

        float totalWeight = 0f;
        for (int i = 0; i < enemyGroupsEndless.Count; i++)
        {
            EndlessEnemyGroup group = enemyGroupsEndless[i];
            if (group != null && group.enemyPrefab != null && group.spawnProb > 0f)
            {
                totalWeight += group.spawnProb;
            }
        }

        if (totalWeight <= 0f)
        {
            return null;
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < enemyGroupsEndless.Count; i++)
        {
            EndlessEnemyGroup group = enemyGroupsEndless[i];
            if (group == null || group.enemyPrefab == null || group.spawnProb <= 0f)
            {
                continue;
            }

            cumulative += group.spawnProb;
            if (roll <= cumulative)
            {
                return group.enemyPrefab;
            }
        }

        return enemyGroupsEndless[enemyGroupsEndless.Count - 1].enemyPrefab;
    }

    void UpdateWaveText(int waveNumber)
    {
        if (waveTMP == null)
        {
            return;
        }

        waveTMP.text = waveNumber.ToString();
    }

    void OnEnemyKilled()
    {
        if (enemiesAlive <= 0)
            return;

        enemiesAlive--;

        Debug.Log("Enemigos restantes: " + enemiesAlive);
    }
}
