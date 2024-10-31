using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

using System.Threading;

public class UDPClient : MonoBehaviour
{
    private const int listenPort = 11000;
    UdpClient listener = new UdpClient(listenPort);
    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
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

                IPAddress targetAddress = IPAddress.Parse("127.0.0.1");

                byte[] sendbuf = Encoding.ASCII.GetBytes(msg);
                IPEndPoint ep = new IPEndPoint(targetAddress, 11069);

                s.SendTo(sendbuf, ep); s.Close();

                print("Message sent");
            }
        }
        catch (SocketException e)
        {
            print(e);
        }
        
    }
    private void OnDestroy()
    {
        listener.Close();
    }
}
