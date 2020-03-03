using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using message;

public class LocalPlayer : PlayerMonoBase
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    CharacterController characterController;
    private MoveComponent moveComponent;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        moveComponent = new MoveComponent()
        {
            characterController = characterController,
            speed = speed,
            jumpSpeed = jumpSpeed,
            gravity = gravity,
            transform = transform,
        };
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        CSP_MoveSync msg = new CSP_MoveSync()
        {
            PlayerID = playerData.ID,
            CurPos = new Vector3Msg()
            {
                X = transform.position.x,
                Y = transform.position.y,
                Z = transform.position.z,
            },
        };
        ConnectManager.instance.connectProxy.SendMessage(EMessageType.CSP_MoveSync, msg);
        moveComponent.Update();
    }
}
