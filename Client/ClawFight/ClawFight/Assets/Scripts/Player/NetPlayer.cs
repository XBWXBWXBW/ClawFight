using message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayer : PlayerMonoBase
{
    public void UpdatePos(PlayerMoveSyncInfo msg) {
        var _p = msg.CurPos;
        Vector3 pos = new Vector3(_p.X, _p.Y, _p.Z);
        transform.position = pos;
    }
}
