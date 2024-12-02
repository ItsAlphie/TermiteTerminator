using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class LevelManager : MonoBehaviour
{   
    
    private int level = 1;

    private int environment = 0; //To do: change this to an enum

    public bool GameOver = false;

    [SerializeField] private UnityEvent OnWaveFinish;
    [SerializeField] private UnityEvent OnGameOver;

    private static LevelManager _instance;
    public static LevelManager Instance{
        get{
            if(_instance == null){
                Debug.LogError("LevelManager instance is null");
            }
            return  _instance;  
        }
    }

    private void Awake(){
        _instance = this;
    }


    [SerializeField] private Transform[] Path;
    //tba -> maps and paths for each level


    // Start is called before the first frame update
    void Start()
    {
        MoneyManager.Instance.initializeMoney(100);
        UIManager.Instance.InitializeHUD();
        RepairAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerGameOver(){
        KillAll();
        GameOver = true;
        OnGameOver.Invoke();
        Debug.Log("Game Over");
        Time.timeScale = 0;
        StartCoroutine(WaitForGameRestart());
    }
    int GetLevel(){
        return level;
    }

    public int GetEnvironment(){
        return environment;
    }

    private IEnumerator WaitForGameRestart() {
        yield return new WaitForSecondsRealtime(10);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void TriggerWaveFinish(){
        OnWaveFinish.Invoke();
        Debug.Log("Wave finished");
    }
    private void KillAll(){
        List<GameObject> towers = TowerSpawner.towers;
        CommunicationController cmCtrl = gameObject.GetComponent<CommunicationController>();
        print("Killing all towers");
        foreach (GameObject towerObject in towers){
            BasicTower tower = towerObject.GetComponent<BasicTower>();
            cmCtrl.SendMsg("k", tower);
        }
    }

    private void RepairAll(){
        List<GameObject> towers = TowerSpawner.towers;
        CommunicationController cmCtrl = gameObject.GetComponent<CommunicationController>();
        print("Killing all towers");
        foreach (GameObject towerObject in towers){
            BasicTower tower = towerObject.GetComponent<BasicTower>();
            cmCtrl.SendMsg("r", tower);
        }
    }
}
