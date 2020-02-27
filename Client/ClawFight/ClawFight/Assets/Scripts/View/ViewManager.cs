using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : ManagerBase<ViewManager>
{
    private Dictionary<EViewType, ViewBase> viewDict = new Dictionary<EViewType, ViewBase>();
    public override void Init()
    {
        base.Init();
        ViewBase[] _v = GameObject.FindObjectsOfType<ViewBase>();
        for (int i = 0; i < _v.Length; i++)
        {
            viewDict.Add(_v[i].viewType, _v[i]);
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
