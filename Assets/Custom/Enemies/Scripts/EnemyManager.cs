using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyType[] enemyTypes; 
    [SerializeField] float timeBetweenSpawns = 1.0f;
    [SerializeField] float timeBetweenWaves = 5.0f;
    [SerializeField] int minLoot = 1;
    [SerializeField] int maxLoot = 3;
    [SerializeField] float lootRange = 5.5f;
    [SerializeField] GameObject lootPrefab;
    [SerializeField] GameObject winObjectPrefab;
    [SerializeField] GameObject tokenPoolTransform;
    [SerializeField] Text waveText;
    [SerializeField] int maxWaves = 28;
    [SerializeField] int maxEnemies = 250;

    private int waveNum, spawnedEnemies, enemiesRemaining;
    private EnemySpawnPoint[] spawnPoints;    
    private Stack<PizzaToken> lootPool = new Stack<PizzaToken>();
    private Stack<GameObject> enemyPool = new Stack<GameObject>();
    private int hiddenGridCounter = 0;
    private float hiddenGridOffset = 10.0f;
    private float hiddenGridYOffset = -50.0f;
    private float enemySpeed = 8.0f;
    private float enemyAttackSpeed = 1.0f;
    private PizzaToss player;

    [System.Serializable] class EnemyType {
        public GameObject enemyPrefab;
        public int earliestWave;        
    }

    void Start()
    {
        player = FindObjectOfType<PizzaToss>();
        spawnPoints = GetComponentsInChildren<EnemySpawnPoint>();
        winObjectPrefab.SetActive(false);
        waveNum = 1;
        UpdateWaveTextGUI();
        InitializeEnemyPool();
        StartCoroutine( StartNextWave() );        
    }

    public void EnemyDeathHandler(Enemy enemy) {
        SpawnLoot(enemy);

        if( --enemiesRemaining <= 0) {
            waveNum++;
            if (waveNum > maxWaves) {
                ProcessWavesComplete();
            } else {
                UpdateWaveTextGUI();
                player.PlayNewWaveAudio();
                StartCoroutine( StartNextWave() );
            }
        }
    }

    public void AddToLootPool(PizzaToken loot) {
     
        lootPool.Push(loot);
        Vector3 hiddenLootPosition = GetNextHiddenGridPosition();        
        loot.transform.position = hiddenLootPosition;
        hiddenGridCounter++;
    }

    private void InitializeEnemyPool() {   
        Vector3 spawnPosition = Vector3.zero;
        EnemyType enemyType = null;

        for (int i=0; i < maxEnemies; i++) {
            spawnPosition = ChooseRandomSpawnPoint().position;
            enemyType = ChooseRandomEnemyType();
            GameObject enemy = Instantiate(enemyType.enemyPrefab,spawnPosition,Quaternion.identity,transform);
            enemy.SetActive(false);
            enemyPool.Push(enemy);
        }
    }

    private void ProcessWavesComplete() {
        winObjectPrefab.SetActive(true);
    }

    private IEnumerator StartNextWave() {   
        
        if (waveNum < 6) { spawnedEnemies = waveNum; }
        else if (waveNum < 10) { spawnedEnemies = 6; }
        else if (waveNum < 15) { spawnedEnemies = 8; }
        else if (waveNum < 20) { spawnedEnemies = 10; }
        else if (waveNum < 25) { spawnedEnemies = 12; }
        else { spawnedEnemies = 13; }
        enemySpeed += 0.4f;
        enemyAttackSpeed += 0.05f;

        enemiesRemaining = spawnedEnemies;   
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine( SpawnEnemies() ); 
    }

    private IEnumerator SpawnEnemies() {
        for (int i=0; i < spawnedEnemies; i++) {
            if (enemyPool.Count > 0) { 
                GameObject tempEnemy = enemyPool.Pop();
                tempEnemy.GetComponentInChildren<Enemy>().SetSpeed(enemySpeed);
                tempEnemy.GetComponentInChildren<Enemy>().SetAttackSpeed(enemyAttackSpeed);
                tempEnemy.SetActive(true);
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void SpawnLoot(Enemy enemy) {

        // Random Roll for Loot
        int numLoot = LootRoll(enemy);

        // Already Loot nearby to Combine with?
        foreach ( PizzaToken token in tokenPoolTransform.GetComponentsInChildren<PizzaToken>() ) {
            float lootDistance = Vector3.Distance(enemy.transform.position,token.transform.position);

            if (lootDistance <= lootRange) {
                token.UpgradeToken(numLoot);
                return;
            }
        }

        // New Loot Position
        Vector3 lootPosition = new Vector3(enemy.transform.position.x, 0.0f, enemy.transform.position.z);

        // Pull from Loot Pool if any
        if (lootPool.Count > 0) {
                PizzaToken loot = lootPool.Pop();
                hiddenGridCounter--;
                loot.Reanimate(lootPosition);
                loot.GetComponentInChildren<PizzaToken>().UpgradeToken( numLoot-1 );

        } 
        
        // Create New Loot
        else {            
            GameObject loot = Instantiate(lootPrefab,lootPosition,Quaternion.identity,tokenPoolTransform.transform);
            loot.GetComponentInChildren<PizzaToken>().UpgradeToken( numLoot-1 );
        }
    }

    private int LootRoll(Enemy enemy) {
        return( Random.Range(minLoot, maxLoot+1) );
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

    private void UpdateWaveTextGUI() {
        waveText.text = "Minute " + waveNum.ToString();
    }

    private Vector3 GetNextHiddenGridPosition() {
        Vector3 gridPosition = new Vector3(
            hiddenGridOffset * ( hiddenGridCounter % 10.0f),
            hiddenGridYOffset + (-1.0f * hiddenGridOffset * Mathf.FloorToInt( hiddenGridOffset / 100.0f )),
            hiddenGridOffset * Mathf.FloorToInt( hiddenGridOffset / 10.0f )
        );
        return gridPosition;
    }
}
