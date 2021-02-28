using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyType[] enemyTypes; 
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float timeBetweenWaves = 5.0f;
    [SerializeField] GameObject lootPrefab;

    private int waveNum, spawnedEnemies, enemiesRemaining;
    private EnemySpawnPoint[] spawnPoints;    
    private Queue<PizzaToken> lootPool = new Queue<PizzaToken>();

    [System.Serializable] class EnemyType {
        public GameObject enemyPrefab;
        public int earliestWave;        
    }

    void Start()
    {
        spawnPoints = GetComponentsInChildren<EnemySpawnPoint>();
        waveNum = 1;
        StartCoroutine( StartNextWave() );
    }

    public void EnemyDeathHandler(Enemy enemy) {
        SpawnLoot(enemy);

        if( --enemiesRemaining <= 0) {
            waveNum++;
            StartCoroutine( StartNextWave() );
        }
    }

    public void AddToLootPool(PizzaToken loot) {
        lootPool.Enqueue(loot);
        Vector3 hiddenLootPosition = new Vector3(
            lootPool.Count * 10.0f,
            -99.0f,
            0.0f
        );
        loot.transform.position = hiddenLootPosition;
    }

    private IEnumerator StartNextWave() {      
        spawnedEnemies = waveNum;
        enemiesRemaining = spawnedEnemies;   
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine( SpawnEnemies() ); 
    }

    private IEnumerator SpawnEnemies() {
        for (int i=0; i < spawnedEnemies; i++) {
            Vector3 spawnPosition = ChooseRandomSpawnPoint().position;       
            EnemyType enemyType = ChooseRandomEnemyType();
            Instantiate(enemyType.enemyPrefab,spawnPosition,Quaternion.identity,transform);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void SpawnLoot(Enemy enemy) {

        int numLoot = LootRoll(enemy);

        for (int i=0;i<numLoot;i++) {
            Vector3 lootSpawnPosition = new Vector3(
                enemy.transform.position.x + Random.Range(0,enemy.lootRange),
                0,
                enemy.transform.position.z + Random.Range(0,enemy.lootRange)
            );

            if (lootPool.Count > 0) {
                PizzaToken loot = lootPool.Dequeue();
                loot.Reanimate(lootSpawnPosition);
            } else {            
                Instantiate(lootPrefab,lootSpawnPosition,Quaternion.identity);
            }
        }
    }

    private int LootRoll(Enemy enemy) {
        return( Random.Range(enemy.minLoot, enemy.maxLoot+1) );
    }

    private EnemyType ChooseRandomEnemyType() {
        int rngEnemyType = -1;
        do { rngEnemyType = Random.Range(0,enemyTypes.Length); }
        while ( enemyTypes[rngEnemyType].earliestWave > waveNum);        
        return( enemyTypes[rngEnemyType] );
    }

    private Transform ChooseRandomSpawnPoint() {
        int rngSpawn = Random.Range(0,spawnPoints.Length);
        return spawnPoints[rngSpawn].transform;
    }
}
