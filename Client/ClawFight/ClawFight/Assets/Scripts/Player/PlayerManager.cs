using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using message;

public class PlayerManager : ManagerBase<PlayerManager>
{
    public override void Init()
    {
        GameManager.instance.AddManager(this);
        EventManager.instance.RegistProto(EMessageType.SyncInfo, OnSyncInfo);
    }
    public override void OnDestroy()
    {
        EventManager.instance.UnRegistProto(EMessageType.SyncInfo, OnSyncInfo);
    }
    void OnSyncInfo(IMessage _m) {
        SyncInfo si = _m as SyncInfo;
        PlayerInfo pi = si.PlayerInfo;
        Debug.LogError("XBW~~ " + pi.PlayerID);
    }
}
