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

        }
        return message;
    }
}
