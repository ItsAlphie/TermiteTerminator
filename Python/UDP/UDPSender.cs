using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

using System.Threading;

public class UDPClient : MonoBehaviour
{
    private const int listenPort = 11069;
    IPAddress targetAddress = IPAddress.Parse("127.0.0.1");
    void Start()
    {
        Send("A message");
    }
    private void Send(string msg)
    {
        try
        {
            while (true)
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                byte[] sendbuf = Encoding.ASCII.GetBytes(msg);
                IPEndPoint ep = new IPEndPoint(targetAddress, listenPort);

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
