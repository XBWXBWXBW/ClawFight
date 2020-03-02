using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : PlayerMonoBase
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    CharacterController characterController;
    private const string HORIZONTAL = "Horizontal", VERTICAL = "Vertical", JUMP = "Jump";

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (characterController.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis(HORIZONTAL), 0, Input.GetAxis(VERTICAL));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton(JUMP)) {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
