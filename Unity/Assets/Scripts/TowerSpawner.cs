using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
public class TowerSpawner : MonoBehaviour
{
    // UDP Settings
    private const int listenPort = 11069;
    UdpClient listener = new UdpClient(listenPort);
    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
    int towerCount = 10;

    // Game Related
    [SerializeField] GameObject TowerPrefab;
    [SerializeField] GameObject LightTowerPrefab;
    private List<GameObject> towers = new List<GameObject>();
    int resolutionX = Screen.width;
    int resolutionY = Screen.height;

    // Thread-safe queue to handle received matrices
    private ConcurrentQueue<float[,]> matrixQueue = new ConcurrentQueue<float[,]>();

    void Start(){
        print(Screen.currentResolution);
        TowerSpawn();
        Thread thread = new Thread(Receive);
        thread.Start();
    }

    private void Receive(){
        try
        {
            while (true)
            {
                print("Waiting for positioning broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                print($"Received Positioning");
                //print($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");

                // Read out received data                
                float[,] matrix = ProcessUDP(Encoding.ASCII.GetString(bytes, 0, bytes.Length), towerCount);
                matrixQueue.Enqueue(matrix);

                // Check if the data is received and processed properly
                // PrintMatrix(matrix);
            }
        }
        catch (SocketException e)
        {
            print(e);
        }
    }
    void Update()
    {
        // Process any matrices received by the UDP listener
        while (matrixQueue.TryDequeue(out float[,] matrix))
        {
            ProcessTowers(matrix);
        }
    }
    private void OnDestroy(){
        listener.Close();
    }

    void PrintMatrix(float[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                row += matrix[i, j].ToString("F2") + " ";
            }
            Debug.Log(row);
        }
        Debug.Log("---------------------");
    }
    static float[,] ProcessUDP(string packet, int towerCount)
    {
        float[,] matrix = new float[towerCount, 4];
        
        // Remove unwanted characters from the received string
        string cleanData = packet.Replace("[", "").Replace("]", "").Replace("\n", "");
        string[] values = cleanData.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // Parse values into matrix
        if (values.Length >= towerCount * 4)
        {
            for (int i = 0; i < towerCount; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix[i, j] = float.Parse(values[i * 4 + j]);
                }
            }
        }
        else
        {
            Debug.LogWarning("Received data is too short for the specified matrix dimensions.");
        }
        return matrix;
    }

    private void TowerSpawn(){
        Vector2 stashLocation = Camera.main.ScreenToWorldPoint(new Vector3 (-resolutionX,-resolutionY,0));

        for (int i = 1; i <= towerCount; i++){
            if (i == 1){
                GameObject clone = Instantiate(LightTowerPrefab, stashLocation, Quaternion.identity);
                clone.name = "Tower_" + i;
                clone.active = false;
                towers.Add(clone);
            }
            else{
                GameObject clone = Instantiate(TowerPrefab, stashLocation, Quaternion.identity);
                clone.name = "Tower_" + i;
                clone.active = false;
                towers.Add(clone);
            }
        }
    }

    private void ProcessTowers(float[,] matrix){
        print("Processing Towers NOW");
        for (int i = 0; i < towerCount; i++){
            float X = matrix[i,1] * resolutionX;
            float Y = matrix[i,2] * resolutionY;
            if ((X == 0 && Y == 0) || (X > resolutionX || Y > resolutionY) || (X < 0 || Y < 0)){
                towers[i].active = false;
            }
            else{
            	towers[i].active = true;
                print("Putting tower " + i + " at " + X + "/"+ Y);
                Vector2 location = Camera.main.ScreenToWorldPoint(new Vector3 (X, Y, 0));
                towers[i].transform.position = location;
            }
        }
    }
}
