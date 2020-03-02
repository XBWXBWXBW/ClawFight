using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonoBase : EntityMonoBase
{
    private SkinnedMeshRenderer _skinRender;
    public SkinnedMeshRenderer skinRender {
        get {
            if (_skinRender == null) {
                _skinRender = GetComponentInChildren<SkinnedMeshRenderer>();
            }
            return _skinRender;
        }
    }
    public PlayerData playerData;
    public void SetMaterial(Material mat) {
        skinRender.sharedMaterial = mat;
    }

    public virtual void StartBorn()
    {
        transform.position = MapManager.instance.GetRandomPosInMap();
    }
}
