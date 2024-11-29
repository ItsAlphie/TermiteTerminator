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

}
