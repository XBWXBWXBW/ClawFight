using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectProxy : ProxyBase
{
    public void Connected() {
        GameManager.instance.tasks.Add(_Connected());
    }
    private IEnumerator _Connected() {
        yield return null;
        EventManager.instance.SendEvent(HallEvents.HALLEVENT_CONNECTED);
    }
    public void ReceiveProto(EMessageType messageType, IMessage message) {
        GameManager.instance.tasks.Add(_ReceiveProto(messageType,message));
    }
    private IEnumerator _ReceiveProto(EMessageType messageType, IMessage message) {
        yield return null;
        EventManager.instance.SendProto(messageType, message);
    }
}
