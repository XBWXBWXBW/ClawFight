using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager {
    void OnDestroy();
    void Init();
}

public class ManagerBase<T> : IManager where T : ManagerBase<T>, new ()
{
    private static T _instance;
    public static T instance {
        get {
            if (_instance == null) {
                _instance = new T();
                _instance.Init();
            }
            return _instance;
        }
    }
    public virtual void OnDestroy() { }
    public virtual void Init() { }
}
