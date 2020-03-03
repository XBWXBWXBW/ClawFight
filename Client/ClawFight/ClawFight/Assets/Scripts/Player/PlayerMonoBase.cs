﻿using System.Collections;
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

    private void Start()
    {
        OnStart();
    }
    protected virtual void OnStart()
    {

    }

    public void SetMaterial(Material mat) {
        skinRender.sharedMaterial = mat;
    }

    public virtual void StartBorn()
    {
        transform.position = MapManager.instance.GetRandomPosInMap(playerData.eTeam);
    }
    private void Update()
    {
        OnUpdate();
    }
    protected virtual void OnUpdate() {

    }
}
