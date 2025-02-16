using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float lookSensitivity = 2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool isSprinting;

    private float verticalLookRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center
        Cursor.visible = false; // Hide cursor
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
        if (isSprinting)
        {
            GenerateSound();
        }
    }

    public void OnMove(UnityEngine.InputSystem.InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnSprint(UnityEngine.InputSystem.InputValue value)
    {
        isSprinting = value.isPressed;
        Debug.Log($"Sprint Pressed: {isSprinting}");
    }

    public void OnLook(UnityEngine.InputSystem.InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnPullUpCamera()
    {
        Debug.Log("Camera pulled up!");
        // Implement camera activation logic here
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.right * movementInput.x + transform.forward * movementInput.y;
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        controller.Move(moveDirection * speed * Time.deltaTime);
    }

    private void HandleLook()
    {
        // Horizontal rotation (yaw)
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);

        // Vertical rotation (pitch)
        verticalLookRotation -= lookInput.y * lookSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    void GenerateSound()
    {
        EnemyAI enemy = FindObjectOfType<EnemyAI>();
        if (enemy != null)
        {
            enemy.HearSound(cameraTransform.position);
        }
    }
}