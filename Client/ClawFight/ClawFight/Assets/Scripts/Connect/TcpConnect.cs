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
            using (MemoryStream ms_raw = new MemoryStream()) {
                using (CodedInputStream cis_raw = new CodedInputStream(ms_raw)) {
                    ms_raw.Write(bytes, 0, _size);
                    ms_raw.Position = 0;
                    rawMessage.MergeFrom(cis_raw);
                }
            }
            ByteString bs = rawMessage.MessageBody;
            CodedInputStream cis_body = bs.CreateCodedInput();
            IMessage m = MessageCreater.CreateMessage((EMessageType)rawMessage.MessageType);
            m.MergeFrom(cis_body);
            GameManager.instance.connectProxy.ReceiveProto((EMessageType)rawMessage.MessageType, m);
            tcpSocket.BeginReceive(bytes, 0, bytes.Length, SocketFlags.None, EndReceive, tcpSocket);
        }
        catch {

        }
    }

    public void SendMessage(EMessageType messageType, IMessage message)
    {
        try
        {
            RawMessage rawMessage = new RawMessage();
            rawMessage.MessageType = (int)messageType;
            using (MemoryStream ms_body = new MemoryStream()) {
                using (CodedOutputStream cos_body = new CodedOutputStream(ms_body)) {
                    message.WriteTo(cos_body);
                    cos_body.Flush();
                    ms_body.Position = 0;
                    ByteString bs = ByteString.FromStream(ms_body);
                    rawMessage.MessageBody = bs;
                }
            }
            using (MemoryStream ms_raw = new MemoryStream()) {
                using (CodedOutputStream cos_raw = new CodedOutputStream(ms_raw)) {
                    rawMessage.WriteTo(cos_raw);
                    cos_raw.Flush();
                    byte[] buffer = ms_raw.GetBuffer();
                    tcpSocket.BeginSend(buffer, 0, (int)ms_raw.Position, SocketFlags.None, EndSend, null);
                }
            }
        }
        catch {

        }
    }
    void EndSend(IAsyncResult ia)
    {
        try
        {
            tcpSocket.EndSend(ia);
        }
        catch {

        }
    }
}
