using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : ManagerBase<ViewManager>
{
    public GameObject canvas;
    private Dictionary<EViewType, ViewBase> viewDict = new Dictionary<EViewType, ViewBase>();
    public override void Init()
    {
        base.Init();
        
        ViewBase[] _v = canvas.GetComponentsInChildren<ViewBase>(true);
        for (int i = 0; i < _v.Length; i++)
        {
            viewDict.Add(_v[i].viewType, _v[i]);
        }

        EventManager.instance.RegistEvent(HallEvents.HALLEVENT_ENTER_PLAY, OnEnterPlay);
    }
    public override void OnDestroy()
    {
        EventManager.instance.UnRegistEvent(HallEvents.HALLEVENT_ENTER_PLAY, OnEnterPlay);
        base.OnDestroy();
    }

    private void OnEnterPlay()
    {
        foreach (var e in viewDict) {
            if (e.Value.isHall)
            {
                e.Value.gameObject.SetActive(false);
            }
        }
    }

    public ViewBase GetView(EViewType viewType) {
        if (viewDict.ContainsKey(viewType)) {
            return viewDict[viewType];
        }
        return null;
    }
    public void ShowView(EViewType eViewType) {
        var v = GetView(eViewType);
        if (v == null) {
            return;
        }
        v.gameObject.SetActive(true);
    }
    public void HideView(EViewType eViewType) {
        var v = GetView(eViewType);
        if (v == null)
        {
            return;
        }
        v.gameObject.SetActive(false);
    }
    public void HideHallView()
    {
        foreach (var e in viewDict) {
            if (e.Value.isHall) {
                HideView(e.Key);
            }
        }
    }
}
