using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    [Header("UI Objects")]
    [SerializeField] public GameObject gameOverScreen;
    [SerializeField] public GameObject waveFinishedScreen;
    [SerializeField] public GameObject HUD;


    public static UIManager Instance{
        get{
            if(_instance == null){
                Debug.LogError("UIManager instance is null");
            }
            return  _instance;  
        }
    }

    private void Awake(){
        _instance = this;
    }

    public void setGameOverScreen(){
        gameOverScreen.SetActive(true);  
    }

    public void setWaveFinishedScreen(){
        waveFinishedScreen.SetActive(true);  
    }

}
