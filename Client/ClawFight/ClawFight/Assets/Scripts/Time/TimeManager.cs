using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : ManagerBase<TimeManager>
{
    private float pingCurTime = 0;
    private float pingDelta = 0.1f;
    private Ping ping;
    public float pingTime = 0.0f;
    public override void Init()
    {
        base.Init();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    public void Update() {
        UpdatePing();
    }
    private void UpdatePing()
    {
        if (ping == null)
        {
            pingCurTime += Time.deltaTime;
            if (pingCurTime >= pingDelta)
            {
                ping = new Ping(GameManager.instance.ipAddress);
            }
        }
        else
        {
            if (ping.isDone)
            {
                pingTime = ping.time;
                ping.DestroyPing();
                ping = null;
            }
        }
    }
}
