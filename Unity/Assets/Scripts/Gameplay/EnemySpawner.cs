using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{   
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject EnemyPrefab2;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private Wave enemyWave;
    
    private bool waveOngoing = false;

    private float timeUntilSpawn;

    private int enemySpawnIndex = 0;

    public static List<GameObject> enemyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        initializeEnemyWave();
    }

    // Update is called once per frame
    void Update()
    {   
        if(waveOngoing){
            if(!isEndOfWave()){
            timeUntilSpawn -= Time.deltaTime; 
            if(timeUntilSpawn <= 0){
                resetSpawnCountdown();
                spawnEnemy();
                }
            }
        else{
            if(!enemyList.Any()){
                LevelManager.Instance.TriggerWaveFinish();
                waveOngoing = false;
                }
            }
        }
        
    }

    private void resetSpawnCountdown(){
        timeUntilSpawn = enemyWave.timeBetweenSpawn;
    }

    private bool isEndOfWave(){
        if(enemySpawnIndex == enemyWave.waveSequence.Count){
            return true;
        }
        return false;
    }

    private void initializeEnemyWave(){
        resetSpawnCountdown();
        waveOngoing = true;
    }

    private void spawnEnemy(){
        GameObject Clone;
        Clone = Instantiate(enemyWave.waveSequence[enemySpawnIndex], SpawnPoint.transform.position, Quaternion.identity);
        enemyList.Add(Clone);
        enemySpawnIndex++;
    }

    // private IEnumerator SpawnEnemyWave(){

    // }
}
