using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent
{
    public CharacterController characterController;
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private const string HORIZONTAL = "Horizontal", VERTICAL = "Vertical", JUMP = "Jump";
    private const string MOUSE_X = "Mouse X", MOUSE_Y = "Mouse Y";
    public Transform transform;
    // Update is called once per frame
    public void Update()
    {
        UpdateMove();
        UpdateRotate();
    }
    private void UpdateMove() {
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis(HORIZONTAL), 0, Input.GetAxis(VERTICAL));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton(JUMP))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }
    private void UpdateRotate() {
        float mouseX = Input.GetAxis(MOUSE_X) * GameManager.instance.rotateXSense;
        float mouseY = Input.GetAxis(MOUSE_Y) * GameManager.instance.rotateYSense;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, mouseX, 0));
    }
}
