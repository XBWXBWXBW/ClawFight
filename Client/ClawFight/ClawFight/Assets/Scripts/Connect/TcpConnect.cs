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
    byte[] bytes = new byte[1024];
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

            tcpSocket.BeginReceive(bytes, 0, bytes.Length, SocketFlags.None, EndReceive, tcpSocket);
        }
        catch {
        }
    }
    
    void EndReceive(IAsyncResult ia) {
        try
        {
            int _size = tcpSocket.EndReceive(ia);
            RawMessage rawMessage = new RawMessage();
            using (MemoryStream ms = new MemoryStream()) {
                using (CodedInputStream cis = new CodedInputStream(ms)) {
                    ms.Write(bytes, 0, _size);
                    ms.Position = 0;
                    rawMessage.MergeFrom(cis);
                }
            }
            Debug.LogError("XBW~~~ recevie " + rawMessage.MessageType);
            tcpSocket.BeginReceive(bytes, 0, bytes.Length, SocketFlags.None, EndReceive, tcpSocket);
        }
        catch {

        }
    }
}
