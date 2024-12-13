using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Unity.VisualScripting;

abstract public partial class BasicSpell : MonoBehaviour
{
    public bool active = false;
    public bool cooldown = false;
    public float timer = 0.0f;
    [SerializeField] protected float spell_duration = 3.0f; 
    [SerializeField] protected float spell_cooldown = 7.0f;
    [SerializeField] protected int spellCost = 15;

    void Start(){
        UpdateUI(0, 3);
    }

    void Update(){
        if (active){
            if (timer > spell_duration)
            {
                active = false;
                ToggleOff();
                timer = 0;
                cooldown = true;
            }
            timer += UnityEngine.Time.deltaTime;
            if (Mathf.Abs(timer - Mathf.Round(timer)) < 0.01f) {
                int roundedTime = Mathf.RoundToInt(spell_duration - timer);
                UpdateUI(roundedTime, 1);
            }
        }
        if (cooldown){
            if (timer > spell_cooldown)
            {
                cooldown = false;
                timer = 0;
            }
            timer += UnityEngine.Time.deltaTime;
            if (Mathf.Abs(timer - Mathf.Round(timer)) < 0.01f) {
                int roundedTime = Mathf.RoundToInt(spell_cooldown - timer);
                UpdateUI(roundedTime, 2);
            }
        }
        if(!cooldown && !active){
            UpdateUI(0,3);
        }
    }

    public void CastSpell(float x, float y){
        float[] cali = CameraCalibration(x, y);
        TowerSpawner towerSpawner = LevelManager.Instance.GetComponent<TowerSpawner>();
        int resolutionX = towerSpawner.resolutionX;
        int resolutionY = towerSpawner.resolutionY;
        float X = cali[0] * resolutionX;
        float Y = cali[1] * resolutionY;
        bool outOfScreen = (X > resolutionX || Y > resolutionY) || (X <= 0 || Y <= 0);

        if (!outOfScreen)
        {
            if(!cooldown){
                if(!active){
                    if(MoneyManager.Instance.CurrentMoney >= spellCost){
                        MoneyManager.Instance.deductMoney(spellCost);
                        ToggleOn();
                        active = true;
                    }
                    else{
                        UIManager.Instance.showInsufficientFundsPopUp(Camera.main.ScreenToWorldPoint(new Vector3(X, Y, 0)));
                        SoundController.instance.PlayErrorSound();
                    }
                }
                // Keep moving the spell once activated
                Debug.Log("Placing spell at " + X + "/" + Y);
                Vector2 location = Camera.main.ScreenToWorldPoint(new Vector3(X, Y, 0));
                gameObject.transform.position = location;
            }
        }
    }

    public float[] CameraCalibration(float X, float Y){
        TowerSpawner towerSpawner = LevelManager.Instance.GetComponent<TowerSpawner>();
        float skewFactorX = towerSpawner.skewFactorX;
        float skewFactorY = towerSpawner.skewFactorY;

        // Horizontal skew
        if (X > 0.5f){
            float d = (X - 0.5f) / 0.5f;
            X = X * (1 - skewFactorX * d);
        }
        else{
            float d = (0.5f - X) / 0.5f;
            X = X * (1 + skewFactorX * d);
        }

        // Vertical skew
        if (Y > 0.5f){
            float d = (Y - 0.5f) / 0.5f;
            Y = Y * (1 - skewFactorY * d);
        }
        else{
            float d = (0.5f - X) / 0.5f;
            Y = Y * (1 + skewFactorY * d);
        }

        float[] returnArray = new float[2];
        returnArray[0] = X;
        returnArray[1] = Y;

        return returnArray;
    }
    abstract protected void spell_effect();

    abstract protected void finish_effect();

    void UpdateUI(int time, int color){
        UIManager UI = UIManager.Instance.GetComponent<UIManager>();
        if(color > 2){
            UI.updateFreezeReady();
        }
        else{
            UI.updateColorFreezeTime(color);
            UI.updateFreezeTime(time);
        }
    }

    void ToggleOff(){
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    void ToggleOn(){
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
}
