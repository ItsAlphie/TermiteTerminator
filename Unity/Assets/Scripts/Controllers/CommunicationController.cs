using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
public class CommunicationController : MonoBehaviour
{
    private const int listenPort = 11000;
    UdpClient listener = new UdpClient(listenPort);
    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
    public volatile bool shouldBoost = false;
    private float boostTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        Thread towerThread = new Thread(ReceiveMessages);
        towerThread.Start();
    }

    /*
    * This method checks where the message came from.
    * If the message is sent locally, it is sent by the local python script and is thus forwarded to the TowerSpawner script.
    * Otherwise the message is from one of the towers and is handled appropriately.
    */
    private void ReceiveMessages()
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        TowerSpawner towerSpawner = levelManager.GetComponent<TowerSpawner>();
        
        try
        {
            while (true)
            {
                byte[] bytes = listener.Receive(ref groupEP);
                string senderIP = groupEP.Address.ToString();
                if (senderIP.Equals("127.0.0.1")){
                    towerSpawner.ReceiveTowerInfo(bytes);
                }
                else{
                    BoostTower(bytes, senderIP);
                }
            }
        }
        catch (SocketException e)
        {
            print(e);
        }
    }

    /**
    * Next part of the code first finds the tower that sent the boost message.
    * It then sets the boost boolean to true and will turn it off after a set time.
    */
    private void BoostTower(byte[] bytes, string towerIP)
    {
        string towerID = towerIP.Substring(towerIP.Length-3);
        GameObject towerObject = GameObject.Find("Tower_" + towerID);
        BasicTower tower = towerObject.GetComponent<BasicTower>();
        tower.Booster = true;
        StartCoroutine(BoostReset(tower));
    }
    private IEnumerator BoostReset(BasicTower tower)
    {
        yield return new WaitForSeconds(boostTime);
        tower.Booster = false;
    }
}
