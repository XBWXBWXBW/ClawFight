using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System;
using message;
using Google.Protobuf;
using System.IO;

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
            GameManager.instance.connectProxy.Connected();
        }
        catch {
        }
    }
    void EndSend(IAsyncResult ia) {
        try
        {
            tcpSocket.EndSend(ia);
        }
        catch {

        }
    }
}
