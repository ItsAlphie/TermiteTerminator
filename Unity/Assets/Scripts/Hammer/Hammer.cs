using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.ComponentModel.Design;

public class Hammer : BasicTower
{
    [SerializeField] float healBoost = 3;
    [SerializeField] int healAmount = 1;
    int hammerID = TowerSpawner.hammerID;
    private float timer = 0.0f;
    private float waitTime = 1.0f;

    void Update()
    {
        if (timer > waitTime){
            ProximityCheck();
            timer = 0;
        }
        timer += UnityEngine.Time.deltaTime;
    }

    public void ProximityCheck(){
        // Check if the hammer is close to other towers
        List<GameObject> towers = TowerSpawner.Instance.towers;
        GameObject levelManager = GameObject.Find("LevelManager");
        CommunicationController cmCtrl = levelManager.GetComponent<CommunicationController>();
        GameObject hammer = towers[hammerID];
        Vector2 locationHammer = hammer.transform.position;

        foreach (GameObject tower in towers){
            if(tower != hammer){
                TowerHealthController healthCtrl = tower.GetComponent<TowerHealthController>();
                BasicTower bTower = tower.GetComponent<BasicTower>();
                Vector2 locationTower = tower.transform.position;
                float distance = Vector2.Distance(locationHammer, locationTower);
                print("Distance to tower " + tower + " is " + distance);
                
                if(distance <= 1){
                    if(bTower.State == TowerState.Broken){
                        healthCtrl.repair();
                    }
                    if(boosted){
                        healthCtrl.heal((int)Mathf.Round(healBoost*healAmount));
                    }
                    else{
                        healthCtrl.heal(healAmount);
                    }
                }
            }
        }

    }
}
