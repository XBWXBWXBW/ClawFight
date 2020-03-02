using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : ManagerBase<MapManager>
{
    private Map _curMap;
    public Map curMap {
        get {
            if (_curMap == null) {
                _curMap = GameObject.FindObjectOfType<Map>();
            }
            return _curMap;
        }
    }
    public override void Init()
    {
        base.Init();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    public Vector3 GetRandomPosInMap(ETeam eTeam) {
        return curMap.GetRandomPos(eTeam);
    }
}
