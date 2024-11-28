using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{

    private static MoneyManager _instance;
    public static MoneyManager Instance{
        get{
            if(_instance == null){
                Debug.LogError("MoneyManager instance is null");
            }
            return  _instance;  
        }
    }

     private void Awake(){
        _instance = this;
    }

    [SerializeField] private int currentMoney;
    [SerializeField] private int starterMoney;

    [SerializeField] private UnityEvent OnMoneyDeducted;
    [SerializeField] private UnityEvent OnMoneyAdded;

    public int CurrentMoney { get => currentMoney; set => currentMoney = value; }

    public void addMoney(int amount){
        currentMoney += amount;
    }

    public void deductMoney(int amount){
        currentMoney -= amount;
    }

    void Start(){
        currentMoney = starterMoney;
    }


}
