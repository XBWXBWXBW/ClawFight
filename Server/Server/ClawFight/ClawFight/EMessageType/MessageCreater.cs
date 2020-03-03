using System.Collections;
using System.Collections.Generic;

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
case EMessageType.CSP_JoinRoom:
                message = new message.CSP_JoinRoom();
                break;
case EMessageType.SCP_JoinRoom:
                message = new message.SCP_JoinRoom();
                break;
case EMessageType.SCP_JoinGame:
                message = new message.SCP_JoinGame();
                break;
case EMessageType.CSP_JoinTeam:
                message = new message.CSP_JoinTeam();
                break;
case EMessageType.SCP_JoinTeam:
                message = new message.SCP_JoinTeam();
                break;
case EMessageType.CSP_ReadyToPlay:
                message = new message.CSP_ReadyToPlay();
                break;
case EMessageType.SCP_ReadyToPlay:
                message = new message.SCP_ReadyToPlay();
                break;
case EMessageType.SCP_EnterPlay:
                message = new message.SCP_EnterPlay();
                break;
case EMessageType.CSP_MoveSync:
                message = new message.CSP_MoveSync();
                break;
case EMessageType.PlayerMoveSyncInfo:
                message = new message.PlayerMoveSyncInfo();
                break;
case EMessageType.SCP_MoveSync:
                message = new message.SCP_MoveSync();
                break;
case EMessageType.CSP_BeginPlay:
                message = new message.CSP_BeginPlay();
                break;
case EMessageType.SCP_BeginPlay:
                message = new message.SCP_BeginPlay();
                break;

        }
        return message;
    }
}
