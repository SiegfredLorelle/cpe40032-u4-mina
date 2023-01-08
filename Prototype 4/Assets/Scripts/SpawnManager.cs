using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //public GameObject enemyPrefab;
    public GameObject[] enemiesPrefab;
    public GameObject[] powerupsPrefab;
    float spawnRange = 9.0f;
    public int enemyCount;
    public int waveNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEmemyWave(waveNumber);
        SpawnPowerUp();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEmemyWave(waveNumber);
            SpawnPowerUp();

        }
    }

    // Spawn a wave of enemies
    private void SpawnEmemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemiesPrefab.Length);
            Instantiate(enemiesPrefab[randomEnemyIndex], GenerateSpawnPosition(), enemiesPrefab[randomEnemyIndex].transform.rotation);
        }
    }

    // Generates a random spawn position
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPostZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPostZ);
        return randomPos;
    }

    private void SpawnPowerUp()
    {
        int randomPowerupIndex = Random.Range(0, powerupsPrefab.Length);
        Instantiate(powerupsPrefab[randomPowerupIndex], GenerateSpawnPosition(), powerupsPrefab[randomPowerupIndex].transform.rotation);
    }
}

