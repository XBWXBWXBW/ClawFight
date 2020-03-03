﻿using message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayer : PlayerMonoBase
{
    private LinkedList<Vector3> posList = new LinkedList<Vector3>();
    public void UpdatePos(PlayerMoveSyncInfo msg) {
        var _p = msg.CurPos;
        Vector3 pos = new Vector3(_p.X, _p.Y, _p.Z);
        posList.AddLast(pos);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        UpdatePos();
    }
    private void UpdatePos() {
        if (posList.Count <= 0) return;
        Vector3 v = posList.First.Value;
        Vector3 curPos = transform.position;
        while (Vector3.Distance(curPos, v) <= 0.01f)
        {
            posList.RemoveFirst();
            if (posList.Count <= 0) return;
            v = posList.First.Value;
        }
        curPos = Vector3.Lerp(curPos, v, 0.5f);
        transform.position = curPos;
    }
}
