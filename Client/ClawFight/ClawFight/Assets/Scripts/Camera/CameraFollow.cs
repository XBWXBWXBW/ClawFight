using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0.59f, 2.0f, -1.45f);
    private Camera cam;
    private Transform mainPlayer;
    private bool battleStart = false;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
    }
    private void OnEnable()
    {
        EventManager.instance.RegistEvent(BattleEvents.BATTLEEVENT_BATTLE_START, OnBattleStart);
    }
    private void OnDisable()
    {
        EventManager.instance.UnRegistEvent(BattleEvents.BATTLEEVENT_BATTLE_START, OnBattleStart);
    }
    private void OnBattleStart()
    {
        if (PlayerManager.instance.mainPlayerInBattle == null)
        {
            mainPlayer = GameObject.FindObjectOfType<LocalPlayer>().transform;
        }
        else
        {
            mainPlayer = PlayerManager.instance.mainPlayerInBattle.transform;
        }
        battleStart = true;
    }

    private void LateUpdate()
    {
        if (!battleStart) return;
        CameraMove();
        CameraRotation();
    }
    private void CameraMove() {
        Vector3 offsetWorld = mainPlayer.transform.TransformPoint(offset);
        transform.position = offsetWorld;
    }
    private void CameraRotation()
    {
        float y = mainPlayer.localRotation.eulerAngles.y;
        float x = transform.localRotation.eulerAngles.x;
        x -= Input.GetAxis("Mouse Y") * GameManager.instance.rotateXSense;
        transform.localRotation = Quaternion.Euler(new Vector3(x, y, 0));
    }
}
