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

    // Start is called before the first frame update
    void Start()
    {
        Thread thread = new Thread(Listen);
        thread.Start();
    }

    private void Listen()
    {
        try
        {
            while (true)
            {
                byte[] bytes = listener.Receive(ref groupEP);
                if (Encoding.ASCII.GetString(bytes, 0, bytes.Length) == "b")
                {
                    shouldBoost = true;
                }
            }
        }
        catch (SocketException e)
        {
            print(e);
        }
    }

    void Update()
    {
        if (shouldBoost)
        {
            shouldBoost = false; // Reset the flag
            BoostAllTowers();
        }
    }

    void BoostAllTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            tower.GetComponent<BasicTower>().Booster = true;
        }
    }
}
