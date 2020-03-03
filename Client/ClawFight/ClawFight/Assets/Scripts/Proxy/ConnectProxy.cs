using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectProxy : ProxyBase
{
    TcpConnect tcpConnect;
    public void Connected() {
        GameManager.instance.tasks.AddLast(_Connected());
    }
    private IEnumerator _Connected() {
        yield return null;
        EventManager.instance.SendEvent(HallEvents.HALLEVENT_CONNECTED);
    }
    public void ReceiveProto(EMessageType messageType, IMessage message) {
        GameManager.instance.tasks.AddLast(_ReceiveProto(messageType,message));
    }
    private IEnumerator _ReceiveProto(EMessageType messageType, IMessage message) {
        yield return null;
        EventManager.instance.SendProto(messageType, message);
    }
    public void Connect()
    {
        tcpConnect = new TcpConnect();
        tcpConnect.Start();
    }
    public void SendMessage(EMessageType messageType, IMessage message) {
        tcpConnect.SendMessage(messageType, message);
    }
}
