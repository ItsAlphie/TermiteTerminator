using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class LevelManager : MonoBehaviour
{   
    
    [SerializeField] private GameObject GameOverScreen;
    public static UnityEvent OnWaveFinish = new UnityEvent();
    private static LevelManager _instance;
    public static LevelManager Instance{
        get{
            if(_instance == null){
                Debug.LogError("LevelManager instance is null");
            }
            return  _instance;  
        }
    }

    private void Awake(){
        _instance = this;
    }


    [SerializeField] private Transform[] Path;
    //tba -> maps and paths for each level


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver(){
        KillAll();
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(WaitForGameRestart());
    }

    private IEnumerator WaitForGameRestart() {
        yield return new WaitForSecondsRealtime(10);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void TriggerWaveFinish(){
        OnWaveFinish.Invoke();
        Debug.Log("Wave finished");
    }
    private void KillAll()
    {
        try
        {
            while (true)
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                byte[] sendbuf = Encoding.ASCII.GetBytes("k");
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.24.139"), 11000);

                s.SendTo(sendbuf, ep); s.Close();

                print("Message sent");
            }
        }
        catch (SocketException e)
        {
            print(e);
        }   
    }
}
