using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EConnectType
{
    TCP,
}
public class ConnectManager : ManagerBase<ConnectManager>
{
    public void Connect(EConnectType connectType) {
        switch (connectType) {
            case EConnectType.TCP:
                break;
        }
    }
    public void Connect() {
        TcpConnect tcpConnect = new TcpConnect();
        tcpConnect.Start();
    }
}
