using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{   
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject EnemyPrefab2;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private Wave enemyWave;

    private float timeUntilSpawn;

    public static List<GameObject> enemyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        initializeEnemyWave();
    }

    // Update is called once per frame
    void Update()
    {   
        Debug.Log("TIME SET");
        GameObject Clone;
        timeUntilSpawn -= Time.deltaTime; //reduce time by amount of time that has passed in a frame
        if(timeUntilSpawn <= 0){
            int i = Random.Range(0, enemyWave.waveSequence.Count);
            Clone = Instantiate(enemyWave.waveSequence[i], SpawnPoint.transform.position, Quaternion.identity);
            enemyList.Add(Clone);
            initializeEnemyWave();
        }
    }



    private void initializeEnemyWave(){
        timeUntilSpawn = enemyWave.timeBetweenSpawn;
    }

    // private IEnumerator SpawnEnemyWave(){

    // }
}
