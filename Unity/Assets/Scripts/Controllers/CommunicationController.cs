using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
public class CommunicationController : MonoBehaviour
{
    private const int listenPort = 11000;
    UdpClient listener = new UdpClient(listenPort);
    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
    TowerSpawner towerSpawner;
    public ConcurrentQueue<Action> mainThreadActions = new ConcurrentQueue<Action>();
    public ConcurrentQueue<float[]> spellData = new ConcurrentQueue<float[]>();
    private static CommunicationController _instance;

    public static CommunicationController Instance{
            get{
                if(_instance == null){
                    Debug.LogError("CommunicationController instance is null");
                }
                return  _instance;  
            }
        }

        private void Awake(){
            _instance = this;
        }

    // Start is called before the first frame update
    void Start()
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        towerSpawner = TowerSpawner.Instance;
        
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
                byte[] bytes = listener.Receive(ref groupEP);
                string senderIP = groupEP.Address.ToString();
                int size = bytes.Length;
                if (senderIP.Equals("127.0.0.1")){
                    if(size > 25){
                        towerSpawner.ReceiveTowerInfo(bytes);
                    }
                    else{
                        ProcessSpell(bytes);
                    }
                }
                else{
                    mainThreadActions.Enqueue(() => BoostTower(bytes, senderIP));
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

        List<GameObject> towers = TowerSpawner.Instance.towers;
        GameObject towerObject = towers[int.Parse(towerID)-1];
        BasicTower tower = towerObject.GetComponent<BasicTower>();

        tower.boosted = true;
    }

    void Update()
    {
        // Process all actions queued for the main thread
        // First the boosts, then the spells
        while (mainThreadActions.TryDequeue(out var action))
        {
            action?.Invoke();
        }

        while (spellData.TryDequeue(out float[] data))
        {
            List<GameObject> spells = TowerSpawner.Instance.spells;
            GameObject spellObject = spells[(int) data[0]];
            BasicSpell spell = spellObject.GetComponent<BasicSpell>();
            spell.CastSpell(data[1],data[2]);
        }
    }
    public void SendMsg(string msg, BasicTower receivingTower)
    {
        try
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            byte[] sendbuf = Encoding.ASCII.GetBytes(msg);
            IPAddress IP = receivingTower.GetIP();
            IPEndPoint ep = new IPEndPoint(IP, listenPort);

            s.SendTo(sendbuf, ep); s.Close();
        }
        catch (SocketException e)
        {
            print(e);
        }   
    }

    public void ProcessSpell(byte[] bytes){
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        string[] values = message.Split(',');

        float[] data = new float[3];
        data[0] = Int32.Parse(values[0]); // Index
        data[1] = float.Parse(values[1]); // X
        data[2] = float.Parse(values[2]); // Y

        spellData.Enqueue(data);
    }
}
