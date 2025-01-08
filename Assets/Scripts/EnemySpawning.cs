using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    [SerializeField] private float initialSpawnRate = 1f;
    [SerializeField] private float spawnRateIncreaseInterval = 10f; 
    [SerializeField] private float spawnRateDecrease = 0.1f; 
    [SerializeField] private float minSpawnRate = 0.3f; 
    [SerializeField] private GameObject[] enemyPrefabs; 
    [SerializeField] private bool canSpawn = true;

    private float currentSpawnRate;
    private float elapsedTime = 0f; 

    
    [SerializeField] private float healthMultiplier = 1f;
    [SerializeField] private float speedMultiplier = 1f; 
    [SerializeField] private float scalingInterval = 20f; 
    private float scalingTimer = 0f; 

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        StartCoroutine(Spawner());
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        scalingTimer += Time.deltaTime;

        
        if (elapsedTime >= spawnRateIncreaseInterval)
        {
            elapsedTime = 0f; 

           
            if (currentSpawnRate > minSpawnRate)
            {
                currentSpawnRate -= spawnRateDecrease;
                if (currentSpawnRate < minSpawnRate) currentSpawnRate = minSpawnRate;
                Debug.Log("Spawn rate increased. New spawn interval: " + currentSpawnRate);
            }
        }

       
        if (scalingTimer >= scalingInterval)
        {
            scalingTimer = 0f; 
            healthMultiplier += 0.2f; 
            speedMultiplier += 0.1f; 
            Debug.Log($"Enemy stats scaled. Health multiplier: {healthMultiplier}, Speed multiplier: {speedMultiplier}");
        }
    }

    private IEnumerator Spawner()
    {
        while (canSpawn)
        {
            
            int rand = Random.Range(0, enemyPrefabs.Length);
            float randomY = transform.position.y; 
            float spawnX = Random.Range(-8, 8); 
            float spawnZ = transform.position.z;

            GameObject enemyToSpawn = enemyPrefabs[rand];

           
            Enemy enemyScript = enemyToSpawn.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.healthMultiplier = healthMultiplier;
                enemyScript.speedMultiplier = speedMultiplier;
            }

            Instantiate(enemyToSpawn, new Vector3(spawnX, randomY, spawnZ), Quaternion.identity);

            
            yield return new WaitForSeconds(currentSpawnRate);
        }
    }
}
