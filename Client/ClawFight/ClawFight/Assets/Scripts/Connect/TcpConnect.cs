using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System;

public class TcpConnect : ConnectBase
{
    Socket tcpSocket;
    public void Start() {
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("172.20.16.71"), 50001);
        try
        {
            tcpSocket.BeginConnect(endPoint, EndConnect, tcpSocket);
        }
        catch {
        }
    }
    void EndConnect(IAsyncResult ia) {
        try
        {
            tcpSocket.EndConnect(ia);
        }
        catch {
        }
    }
}
