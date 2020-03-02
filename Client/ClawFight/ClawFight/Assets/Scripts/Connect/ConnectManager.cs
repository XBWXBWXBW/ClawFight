using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EConnectType
{
    TCP,
}
public class ConnectManager : ManagerBase<ConnectManager>
{
    public ConnectProxy connectProxy;
    public override void Init()
    {
        base.Init();
        connectProxy = new ConnectProxy();
    }
}
