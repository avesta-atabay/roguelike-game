using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        //the interval at which to spawn enemies
        public float spawnInteval;
        //the number of enemys alredy spawned in wave
        public int spawnCount;
    }
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        //the number of enemy to pawn in this wave
        public int enemyCount;
        //the number of enemys of this type already spawned in this wave
        public int spawnCount;
        public GameObject enemyPrefab;
    }
    //a list of all the weavs in the game
    public List<Wave> waves;
    //the index of the currnet wave
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    float spawnTimer;//timer use to determine when to spawn the next enemy
    public int enemiesAlive;
    public int maxEnimesAllowed;//the max number of enemies allowed on the map at once
    public bool maxEnimesReaches;
    public float waveInterval;//the interval between each wave

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPositions;

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    void Update()
    {
        //check if the wave has ended and the next wave should start
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= waves[currentWaveCount].spawnInteval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval);
        if(currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    //this method will stop spawning enemies ıf the amount of enemies on the map is max
    void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota &&!maxEnimesReaches)
        {
            foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    //limit the number of enemies that can be spawned at once
                    if(enemiesAlive >= maxEnimesAllowed)
                    {
                        maxEnimesReaches = true;
                        return;
                    }

                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPositions[Random.Range(0, relativeSpawnPositions.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }
        //reset the maxenimiesreached flag if the number of ememies alive has dropped bvelow the max amount
        if(enemiesAlive < maxEnimesAllowed){
            maxEnimesReaches =false;
        }
    }
    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}
