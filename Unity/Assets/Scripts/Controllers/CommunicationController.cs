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
    private float boostTime = 3;
    TowerSpawner towerSpawner;

    // Start is called before the first frame update
    void Start()
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        towerSpawner = levelManager.GetComponent<TowerSpawner>();
        
        print("Starting Communication thread");
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
        try
        {
            while (true)
            {
                print("Waiting for messages");
                byte[] bytes = listener.Receive(ref groupEP);
                string senderIP = groupEP.Address.ToString();
                print("Got message from " + senderIP);
                if (senderIP.Equals("127.0.0.1")){
                    print("Handling towers");
                    towerSpawner.ReceiveTowerInfo(bytes);
                }
                else{
                    print("Handling boost");
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
        string towerID = towerIP.Substring(towerIP.Length-1);
        print("I think I got a message from tower " + towerID);
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
/*
    //TODO: send message method
    private void SendMessage(string msg, BasicTower tower)
    {
        try
        {
            while (true)
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                byte[] sendbuf = Encoding.ASCII.GetBytes(msg);
                string IP = tower.GameObject.name;
                IPAddress targetAddress = IPAddress.Parse();
                IPEndPoint ep = new IPEndPoint(targetAddress, listenPort);

                s.SendTo(sendbuf, ep); s.Close();
            }
        }
        catch (SocketException e)
        {
            print(e);
        }   
    }*/
}
