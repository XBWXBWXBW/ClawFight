using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using message;

public enum EMessageType {
    CSP_JoinMatch = 1,
}
public class MessageCreater
{
    public static IMessage CreateMessage(EMessageType messageType) {
        IMessage message = null;
        switch (messageType)
        {
            case EMessageType.CSP_JoinMatch:
                message = new CSP_JoinMatch();
                break;
        }
        return message;
    }
}
