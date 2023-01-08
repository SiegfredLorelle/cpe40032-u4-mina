using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public PlayerController playerControllerScript;

    public GameObject[] enemiesPrefab;
    public GameObject[] powerupsPrefab;
    //public GameObject rocket;
    private float spawnRange = 9.0f;
    public Enemy[] enemiesAlive;
    public int enemyCount;
    public int waveNumber = 1;


    // Start is called before the first frame update
    void Start()
    {
        SpawnEmemyWave(waveNumber);
        SpawnPowerUp();

        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        enemiesAlive = FindObjectsOfType<Enemy>();
        enemyCount = enemiesAlive.Length;

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

