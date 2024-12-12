using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] public GameObject towerFeedbackScreen;
    [SerializeField] public GameObject coinPopUp;

    private float towerFeedbackScreenTimer;

    private bool towerFeedbackScreenOn = false;
    private bool towerFeedbackScreenTimerOn = false;

    private TMP_Text currentMoneyDisplay;

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

    void Update(){
        if(towerFeedbackScreenTimerOn){
            towerFeedbackScreenTimer -= Time.deltaTime; 
            if(towerFeedbackScreenTimer <= 0){
                towerFeedbackScreen.SetActive(false);
                towerFeedbackScreenOn = false;
            }
        }
    }

    public void InitializeHUD(){
        currentMoneyDisplay = HUD.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        updateCurrentMoney();
    }

    public void setGameOverScreen(){
        gameOverScreen.SetActive(true);  
    }

    public void setWaveFinishedScreen(){
        waveFinishedScreen.SetActive(true);  
    }

    public void setTowerFeedbackScreen(){
        if(!towerFeedbackScreenOn){
            towerFeedbackScreenOn = true;
            towerFeedbackScreen.SetActive(true);
            initializeTowerFeedbackScreenTimer();
        }
    }

    public void disableTowerFeedbackScreen(){
        towerFeedbackScreenTimerOn = true;
    }

    public void initializeTowerFeedbackScreenTimer(){
        towerFeedbackScreenTimer = 5;
    }

    public void updateCurrentMoney(){
        currentMoneyDisplay.text = MoneyManager.Instance.CurrentMoney.ToString();
    }

    public void showMoneyDeducted(){
        currentMoneyDisplay.GetComponent<Animator>().SetTrigger("OnMoneyDeducted");
        updateCurrentMoney();
    }

    public void showMoneyAdded(){
        currentMoneyDisplay.GetComponent<Animator>().SetTrigger("OnMoneyAdded");
        updateCurrentMoney();
    }

    public void showCoinPopUp(Vector3 position, int value, bool positive){
        GameObject popUp = Instantiate(coinPopUp, position, Quaternion.identity);
        TMP_Text popUpValue = popUp.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if(positive){
            popUpValue.text = ("+ " + value).ToString();
        }
        else{
            popUpValue.text = ("- " + value).ToString();
        }
        Destroy(popUp, 4);
    }
}
