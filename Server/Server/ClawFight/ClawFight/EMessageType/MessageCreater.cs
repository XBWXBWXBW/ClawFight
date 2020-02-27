using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCreater
{
    public static Google.Protobuf.IMessage CreateMessage(EMessageType msgType) {
        Google.Protobuf.IMessage message = null;
        switch (msgType) {
            case EMessageType.CSP_JoinMatch:
                message = new message.CSP_JoinMatch();
                break;
case EMessageType.PlayerInfo:
                message = new message.PlayerInfo();
                break;
case EMessageType.SyncInfo:
                message = new message.SyncInfo();
                break;
case EMessageType.BeginMatch:
                message = new message.BeginMatch();
                break;
case EMessageType.CancelMatch:
                message = new message.CancelMatch();
                break;
case EMessageType.CreateRoom:
                message = new message.CreateRoom();
                break;

        }
        return message;
    }
}
