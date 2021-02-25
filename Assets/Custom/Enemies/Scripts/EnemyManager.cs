using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyType[] enemyTypes; 
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float timeBetweenWaves = 5.0f;
    private int waveNum, spawnedEnemies, enemiesRemaining;
    private EnemySpawnPoint[] spawnPoints;

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

    public void EnemyDeathHandler(GameObject enemy) {
        if( --enemiesRemaining <= 0) {
            waveNum++;
            StartCoroutine( StartNextWave() );
        }
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
