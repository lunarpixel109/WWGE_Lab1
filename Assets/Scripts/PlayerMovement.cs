using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CharacterController))]
class PlayerMovement : MonoBehaviour {

    CharacterController controller;
    public float speed = 5f;

    [Header ("Sprint Settings")]
    public float sprintIncrease = 2f;
    public float sprintFovIncrease = 30f;

    private Vector3 velocity;

    [Header("Jump Settings")]
    public float gravity = -9.8f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    private void Start() {
        controller = GetComponent<CharacterController>();
    }

    private void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Check if the player is on the ground
        
        // Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            speed += sprintIncrease; // increase the speed by the amount specified in the inspector
            FindAnyObjectByType<Camera>().fieldOfView += sprintFovIncrease; // Increase the FOV 
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            speed -= sprintIncrease; // reset the speed back to the normal speed
            FindAnyObjectByType<Camera>().fieldOfView -= sprintFovIncrease; // reset the FOV
        }


        // Get the player input
        float x = Input.GetAxis("Horizontal");
        float z =  Input.GetAxis("Vertical");
        Vector3 movementInput = transform.right * x + transform.forward * z;
        controller.Move(movementInput * speed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Add in instance velocity change to the y velocity when the player wants to jump
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = 5;
        }

        // if we are on the ground keep the y velocity at 0 so that we don't accumulate gravity
        if (isGrounded && velocity.y <= 0) {
            velocity.y = 0;
        }
    }


}