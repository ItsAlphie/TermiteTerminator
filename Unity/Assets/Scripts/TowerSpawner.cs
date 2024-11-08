using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

using System.Threading;
public class TowerSpawner : MonoBehaviour
{
    // UDP Settings
    private const int listenPort = 11069;
    UdpClient listener = new UdpClient(listenPort);
    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
    int towerCount = 10;

    // Game Related
    [SerializeField] GameObject TowerPrefab;
    List<List<double>> towerList = new List<List<double>>();
    int resolutionX = 1920;
    int resolutionY = 1080;

    void Start(){
        TowerSpawn();
        Thread thread = new Thread(Receive);
        thread.Start();
    }

    private void Receive(){
        try
        {
            while (true)
            {
                print("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                // print($"Received broadcast from {groupEP} :");
                // print($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");

                // Read out received data                
                float[,] matrix = processUDP(Encoding.ASCII.GetString(bytes, 0, bytes.Length), towerCount);
                
                // TODO: Position Towers
                
                // TODO: Spawn towers if they're new
                
                // Check if the data is received and processed properly
                // PrintMatrix(matrix);
            }
        }
        catch (SocketException e)
        {
            print(e);
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
    static float[,] processUDP(string packet, int towerCount)
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
        for (int i = 0; i < towerCount; i++){
            Instantiate(TowerPrefab, stashLocation, Quaternion.identity);
            // TODO spawn in invisible state
        }
    }
}