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
    int matrixRows = 10;
    int matrixCols = 4;

    // Game Related
    [SerializeField] GameObject TowerPrefab;
    List<List<double>> towerList = new List<List<double>>();

    void Start(){
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
                    // Position Towers
                        // Spawn towers if they're new
                
                float[,] matrix = processUDP(Encoding.ASCII.GetString(bytes, 0, bytes.Length), matrixRows, matrixCols);

                // Log the matrix to verify (or use it as needed)
                PrintMatrix(matrix);
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
    static float[,] processUDP(string packet, int rows, int cols)
    {
        float[,] matrix = new float[rows, cols];
        
        // Remove unwanted characters from the received string
        string cleanData = packet.Replace("[", "").Replace("]", "").Replace("\n", "");
        string[] values = cleanData.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // Parse values into matrix
        if (values.Length >= rows * cols)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = float.Parse(values[i * cols + j]);
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
        Debug.Log("Click");
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(TowerPrefab, mousePosition, Quaternion.identity);
    }
}