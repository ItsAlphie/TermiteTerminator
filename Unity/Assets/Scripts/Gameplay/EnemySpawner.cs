using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class EnemySpawner : MonoBehaviour
{   
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject EnemyPrefab2;
    [SerializeField] private GameObject SpawnPoint;
    private Wave enemyWave;

    [SerializeField] private List<Wave> waveList;

    private static EnemySpawner _instance;
    public static EnemySpawner Instance{
        get{
            if(_instance == null){
                Debug.LogError("EnemySpawner instance is null");
            }
            return  _instance;  
        }
    }

    private void Awake(){
        _instance = this;
    }
    
    private bool waveOngoing = false;

    private float timeUntilSpawn;

    private int enemySpawnIndex = 0;

    public List<GameObject> enemyList = new List<GameObject>();

    private int currentWave = 0;

    [SerializeField] private float timeBetweenWaves = 10f;

    private float timeWaveEnded;

    // Start is called before the first frame update
    void Start()
    {
        if(waveList.Any()){
            enemyWave = waveList[0];
            initializeEnemyWave();
        }
        else{
            Debug.LogError("No waves in waveList");
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(waveOngoing){
            if(!isEndOfWave()){ //If the wave is not over
            timeUntilSpawn -= Time.deltaTime; 
            if(timeUntilSpawn <= 0){
                resetSpawnCountdown();
                spawnEnemy();
                }
            }
            else{
                if(!enemyList.Any()){ //If the wave is over and there are no enemies left
                    Debug.Log("All enemies dead");
                    if(currentWave == waveList.Count - 1){ //If the last wave is over
                            //finish game, you won
                            //LevelManager.Instance.TriggerGameOver();
                            Debug.Log("Game Over");
                    }
                    else{ //If there are more waves to come
                        Debug.Log("Wave " + currentWave + " ended");
                        LevelManager.Instance.TriggerWaveFinish();
                        waveOngoing = false;
                        timeWaveEnded = Time.time;
                        
                    }
                    
                }
            }
        }
        else{ //If the wave is over and the time between waves has passed
            if(Time.time - timeWaveEnded >= timeBetweenWaves){
                currentWave++;
                enemyWave = waveList[currentWave];
                initializeEnemyWave();
                Debug.Log("Wave " + currentWave + " started");
                LevelManager.Instance.TriggerWaveStart();
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
        enemySpawnIndex = 0;
    }

    private void spawnEnemy(){
        GameObject Clone;
        Clone = Instantiate(enemyWave.waveSequence[enemySpawnIndex], SpawnPoint.transform.position, Quaternion.identity);
        enemyList.Add(Clone);
        enemySpawnIndex++;
    }
}
