using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemiesPrefab;
    public GameObject[] powerupsPrefab;
    private float spawnRange = 9.0f;
    public Enemy[] enemiesAlive;
    public int enemyCount;
    private int waveNumber = 1;


    // Start is called before the first frame update
    void Start()
    {
        // Spawn 1 enemy and 1 powerup in the first wave
        SpawnEmemyWave(waveNumber);
        SpawnPowerUp();
    }

    // Update is called once per frame
    void Update()
    {
        // Keep tabs on the enemies alive
        enemiesAlive = FindObjectsOfType<Enemy>();
        enemyCount = enemiesAlive.Length;

        // If no enemies are alive
        if (enemyCount == 0)
        {
            // Spawn the next wave, with + 1 enemies, and 1 powerup
            waveNumber++;
            SpawnEmemyWave(waveNumber);
            SpawnPowerUp();
        }

    }

    // Spawn a wave of enemies
    private void SpawnEmemyWave(int enemiesToSpawn)
    {
        // Spawn enemies based on the passed arguement
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Randomize the place in which the enemy will spawn, and which type of enemy to spawn
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

    // Spawn a powerup
    private void SpawnPowerUp()
    {
        // Randomize the place in which the powerup will spawn, and which type of powerup to spawn
        int randomPowerupIndex = Random.Range(0, powerupsPrefab.Length);
        Instantiate(powerupsPrefab[randomPowerupIndex], GenerateSpawnPosition(), powerupsPrefab[randomPowerupIndex].transform.rotation);
    }
}

