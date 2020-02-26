using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : ManagerBase<EventManager>
{
    private interface IAction { }
    private class ActionEvent<T> : IAction {
        public Action<T> action;
        public void RegistEvent(Action<T> _action){
            action -= _action;
            action += _action;
        }
        public void UnRegistEvent(Action<T> _action) {
            action -= _action;
        }
        public void SendEvent(T param) {
            action.Invoke(param);
        }
    }
   
    private Dictionary<string, IAction> actionTDict = new Dictionary<string, IAction>();
    private Dictionary<string, Action> actionDict = new Dictionary<string, Action>();
    private Dictionary<EMessageType, Action<IMessage>> messageDict = new Dictionary<EMessageType, Action<IMessage>>();
    public void RegistEventT<T>(string eventName, Action<T> action) {
        if (!actionTDict.ContainsKey(eventName))
        {
            ActionEvent<T> at_new = new ActionEvent<T>();
            actionTDict.Add(eventName, at_new);
        }
        ActionEvent<T> at = actionTDict[eventName] as ActionEvent<T>;
        at.RegistEvent(action);
    }
    public void UnRegisEventT<T>(string eventName, Action<T> action) {
        if (actionTDict.ContainsKey(eventName)) {
            ActionEvent<T> at = actionTDict[eventName] as ActionEvent<T>;
            at.UnRegistEvent(action);
            if (at.action == null) {
                actionTDict.Remove(eventName);
            }
        }
    }
    public void SendEventT<T>(string eventName, T param) {
        if (actionTDict.ContainsKey(eventName)) {
            (actionTDict[eventName] as ActionEvent<T>).SendEvent(param);
        }
    }
    public void RegistEvent(string eventName, Action action) {
        if (!actionDict.ContainsKey(eventName))
        {
            actionDict.Add(eventName, action);
        }
        else {
            actionDict[eventName] -= action;
            actionDict[eventName] += action;
        }
    }
    public void UnRegistEvent(string eventName, Action action) {
        if (actionDict.ContainsKey(eventName)) {
            actionDict[eventName] -= action;
            if (actionDict[eventName] == null) {
                actionDict.Remove(eventName);
            }
        }
    }
    public void SendEvent(string eventName) {
        if (actionDict.ContainsKey(eventName)) {
            actionDict[eventName].Invoke();
        }
    }
    public void RegistProto(EMessageType messageType, Action<IMessage> action) {
        if (!messageDict.ContainsKey(messageType))
        {
            messageDict.Add(messageType, action);
        }
        else {
            messageDict[messageType] -= action;
            messageDict[messageType] += action;
        }
    }
    public void UnRegistProto(EMessageType messageType, Action<IMessage> action) {
        if (messageDict.ContainsKey(messageType))
        {
            messageDict[messageType] -= action;
            if (messageDict[messageType] == null) {
                messageDict.Remove(messageType);
            }
        }
    }
    public void SendProto(EMessageType messageType, IMessage m) {
        if (messageDict.ContainsKey(messageType))
        {
            messageDict[messageType].Invoke(m);
        }
    }
}
