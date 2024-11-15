using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{   
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject EnemyPrefab2;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;

    private float timeUntilSpawn;

    public static List<GameObject> enemyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        setTimeUntilSpawn();
    }

    // Update is called once per frame
    void Update()
    {   
        GameObject Clone;
        timeUntilSpawn -= Time.deltaTime; //reduce time by amount of time that has passed in a frame
        if(timeUntilSpawn <= 0){
            if(Random.Range(0, 5) == 1){
                Clone = Instantiate(EnemyPrefab2, SpawnPoint.transform.position, Quaternion.identity);
            } else {
                Clone = Instantiate(EnemyPrefab, SpawnPoint.transform.position, Quaternion.identity);
            }
            enemyList.Add(Clone);
            setTimeUntilSpawn();
        }
    }

    private void setTimeUntilSpawn(){
        timeUntilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
